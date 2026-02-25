using System;
using System.Collections;
using Photon.Pun;
using UnityEngine;

public class SpawnManager : LocalSingleton<SpawnManager>
{
    public Transform[] SpawnPositions;
    public PlayerContext PlayerContext;
    public float RespawnTime = 5f;

    private GameObject prefab;

    public event Action OnRespawn;
    public void Spawn()
    {
        if (SpawnPositions == null)
        {
            Debug.LogWarning("등록된 스폰 포인트가 없습니다.");
            return;
        }

        Vector3 spawnPos = SpawnPositions[UnityEngine.Random.Range(0, SpawnPositions.Length)].position;

        // 리소스 폴더에서 "Player" 이름을 가진 프리팹을 생성하고, 서버에 등록함
        // 리소스 폴더는 좋지 않음 => 다른 방법을 찾아보자
        prefab = PhotonNetwork.Instantiate("Player", spawnPos, Quaternion.identity);

        if (prefab == null) return;
        prefab.GetComponent<PlayerController>().OnDie += HandlePlayerDie;

        PlayerContext.SetPlayer(prefab);
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

        var cc = prefab.GetComponent<CharacterController>();
        if (cc != null)
        {
            cc.enabled = false;
        }
    }

    public void RespawnNow()
    {
        if (prefab == null) return;

        if (SpawnPositions == null)
        {
            Debug.LogWarning("등록된 스폰 포인트가 없습니다.");
            return;
        }

        Vector3 spawnPos = SpawnPositions[UnityEngine.Random.Range(0, SpawnPositions.Length)].position;


        var cc = prefab.GetComponent<CharacterController>();
        if (cc != null)
        {
            cc.enabled = false;
        }

        prefab.transform.position = spawnPos;

        if (cc != null)
        {
            cc.enabled = true;
        }

        OnRespawn?.Invoke();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        if (prefab == null) return;
        prefab.GetComponent<PlayerController>().OnDie -= HandlePlayerDie;
    }
}
