using System;
using System.Collections;
using Photon.Pun;
using UnityEngine;

public class SpawnManager : LocalSingleton<SpawnManager>
{
    public Transform[] SpawnPositions;
    public PlayerBinder PlayerContext;
    public float RespawnTime = 5f;

    private BoxCollider _spawnArea;

    private GameObject _player;

    public event Action OnRespawn;

    protected override void Awake()
    {
        base.Awake();

        _spawnArea = GetComponent<BoxCollider>();
    }
    public void Spawn()
    {
        if (SpawnPositions == null)
        {
            Debug.LogWarning("등록된 스폰 포인트가 없습니다.");
            return;
        }

        //Vector3 spawnPos = SpawnPositions[UnityEngine.Random.Range(0, SpawnPositions.Length)].position;
        Vector3 spawnPos = GetRandomPointInBox(_spawnArea);

        // 리소스 폴더에서 "Player" 이름을 가진 프리팹을 생성하고, 서버에 등록함
        // 리소스 폴더는 좋지 않음 => 다른 방법을 찾아보자
        _player = PhotonNetwork.Instantiate("Player", spawnPos, Quaternion.identity);

        if (_player == null) return;
        _player.GetComponent<PlayerController>().OnDie += HandlePlayerDie;
        
        PlayerContext.SetPlayer(_player);
    }

    public IEnumerator RespawnCoroutine()
    {
        yield return new WaitForSeconds(RespawnTime);

        RespawnNow();
    }

    public void HandlePlayerDie(EDieType type)
    {
        switch (type)
        {
            case EDieType.DelayedRespawn:
                StartCoroutine(RespawnCoroutine());
                break;
            case EDieType.InstantRespawn:
                RespawnNow();
                break;
        }  
    }

    public void RespawnNow()
    {
        if (_player == null) return;

        if (SpawnPositions == null)
        {
            Debug.LogWarning("등록된 스폰 포인트가 없습니다.");
            return;
        }

        //Vector3 spawnPos = SpawnPositions[UnityEngine.Random.Range(0, SpawnPositions.Length)].position;
        Vector3 spawnPos = GetRandomPointInBox(_spawnArea);

        var cc = _player.GetComponent<CharacterController>();
        if (cc != null)
        {
            cc.enabled = false;
        }

        _player.transform.position = spawnPos;

        if (cc != null)
        {
            cc.enabled = true;
        }

        OnRespawn?.Invoke();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        if (_player == null) return;
        _player.GetComponent<PlayerController>().OnDie -= HandlePlayerDie;
    }

    private Vector3 GetRandomPointInBox(BoxCollider box)
    {
        var bounds = box.bounds;
        var min = bounds.min;
        var max = bounds.max;

        return new Vector3(
            UnityEngine.Random.Range(min.x, max.x),
            UnityEngine.Random.Range(min.y, max.y),
            UnityEngine.Random.Range(min.z, max.z)
        );
    }
}
