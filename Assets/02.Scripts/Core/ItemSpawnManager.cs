using Photon.Pun;
using UnityEngine;
using UnityEngine.UIElements;

public class ItemSpawnManager : LocalSingleton<ItemSpawnManager>
{
    [SerializeField] private GameObject[] items;
    private PhotonView _photonView;

    [SerializeField] private float _autoDropInterval;
    [SerializeField] private bool _autoDropEnabled = true;

    private BoxCollider _autoDropArea;
    private float _autoDropTimer;

    protected override void Awake()
    {
        base.Awake();
        _photonView = GetComponent<PhotonView>();
        _autoDropArea = GetComponent<BoxCollider>();
    }

    private void Update()
    {
        HandleAutoDropTimer(Time.deltaTime);
    }

    // 우리의 약속 : 방장에게 룸 관련해서 뭔가 요청을 할 때는 메서드 명에 Request로 시작하는 것이 유지보수면에서 유리
    public void RequestMakeItems(Vector3 position)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            // 방장이라면 그냥 함수 호출
            Spawn(position);
        }
        else
        {
           // 방장이 아니라면 방장의 함수 호출
            _photonView.RPC(nameof(Spawn), RpcTarget.MasterClient, position);
        }
    }

    [PunRPC]
    public void Spawn(Vector3 position)
    {
        // 소유자가 나가면 해당 네트워크 게임 오브젝트도 삭제된다.
        // 플레이어가 룸을 나가면 그 플레이어가 생성/소유한 모든 네트워크 게임 오브젝트는 삭제 된다.
        // 즉, 플레이어 생명 주기를 갖고 있다.
        
        GameObject prefab = items[Random.Range(0, items.Length)];

        // 그래서 플레이어 생명 주기가 아닌 룸 생명 주기로 만들어야 한다.
        var item = PhotonNetwork.InstantiateRoomObject(prefab.name, position, Quaternion.identity);

        // 포톤에는 룸 안에 방장 (Master Client)이 있다.
        // 방을 만든 사람이 방장
        // 방장은 양도 할 수 있다
        // 방장이 게임을 나가면 자동으로 그 다음으로 들어온 사람이 방장이 된다.
        // InstantiateRoomObject은 방장에게만 권한이 있다.
        // 방장에게 RPC로 요청을 보내야 한다.

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

    private void HandleAutoDropTimer(float deltaTime)
    {
        if (!_autoDropEnabled) return;
        if (_autoDropInterval <= 0f) return;
        if (_autoDropArea == null) return;
        if (!PhotonNetwork.IsMasterClient) return;

        _autoDropTimer += deltaTime;
        if (_autoDropTimer < _autoDropInterval) return;

        _autoDropTimer -= _autoDropInterval;

        var spawnPos = GetRandomPointInBox(_autoDropArea);
        Spawn(spawnPos);
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
