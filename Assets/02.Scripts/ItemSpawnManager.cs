using Photon.Pun;
using UnityEngine;

public class ItemSpawnManager : LocalSingleton<ItemSpawnManager>
{
    [SerializeField] private GameObject[] items;

    public void Spawn(Vector3 position)
    {
        GameObject prefab = items[Random.Range(0, items.Length)];
        var item = PhotonNetwork.Instantiate(prefab.name, position, Quaternion.identity);

        Rigidbody rb = item.GetComponent<Rigidbody>();

        if (rb != null)
        {
            // 약간 랜덤한 방향 + 위쪽 힘
            Vector3 randomDir = new Vector3(
                Random.Range(-1f, 1f),
                Random.Range(0.6f, 1.2f),
                Random.Range(-1f, 1f)
            ).normalized;

            float force = Random.Range(4f, 7f);
            rb.AddForce(randomDir * force, ForceMode.Impulse);
        }
    }

}
