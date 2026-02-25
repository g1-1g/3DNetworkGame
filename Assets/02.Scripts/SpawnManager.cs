using Photon.Pun;
using UnityEngine;

public class SpawnManager : LocalSingleton<SpawnManager>
{
    public Transform[] SpawnPositions;
    public PlayerContext PlayerContext;

    private GameObject prefab;
    public void Spawn()
    {
        if (SpawnPositions == null)
        {
            Debug.LogWarning("등록된 스폰 포인트가 없습니다.");
            return;
        }

        Vector3 spawnPos = SpawnPositions[Random.Range(0, SpawnPositions.Length)].position;

        // 리소스 폴더에서 "Player" 이름을 가진 프리팹을 생성하고, 서버에 등록함
        // 리소스 폴더는 좋지 않음 => 다른 방법을 찾아보자
        prefab = PhotonNetwork.Instantiate("Player", spawnPos, Quaternion.identity);
        PlayerContext.SetPlayer(prefab);
    }

    public void Respawn()
    {
        if (SpawnPositions == null)
        {
            Debug.LogWarning("등록된 스폰 포인트가 없습니다.");
            return;
        }

        Vector3 spawnPos = SpawnPositions[Random.Range(0, SpawnPositions.Length)].position;

        prefab.transform.position = spawnPos;
    }
}
