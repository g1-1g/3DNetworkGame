using Photon.Pun;
using UnityEngine;
using static UnityEngine.UI.GridLayoutGroup;

public class PlayerController : MonoBehaviour, IPunObservable, IDamageable
{
    public PlayerStat Stat { get; private set; }
    public PhotonView PhotonView { get; private set; }

    private PlayerAnimator _animator;

    private EGameState _gameState;

    public EGameState GameState => _gameState;
    void Awake()
    {
        Stat = GetComponent<PlayerStat>();
        PhotonView = GetComponent<PhotonView>();
        _animator = GetComponent<PlayerAnimator>();

        SetGameState(EGameState.Game);
    }


    // 데이터 동기화를 위한 데이터 읽기(전송), 쓰기(수신) 메서드. 자동으로 호출됨
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        // 스트림 : "시냇물"처럼 데이터가 멈추지 않고 연속적으로 흐르는 데이터 흐름
        //        : 서버에서 주고받을 데이터가 담겨있는 변수

        // 읽기 / 쓰기 모드
        if (stream.IsWriting)
        {
            // 이 PhotonView의 데이터를 보내줘야 하는 상황
            stream.SendNext(Stat.Health);
            stream.SendNext(Stat.Stamina);
        }
        if (stream.IsReading)
        {
            // 이 PhotonView의 데이터를 받아야 하는 상황

            // 보낸 순서대로 받는다
            // 보낼 데이터가 많은 경우 Json으로 변환하여 받는다
            Stat.SetHealth ((float)stream.ReceiveNext());
            Stat.SetStamina((float)stream.ReceiveNext());
        }
    }

    [PunRPC]
    public void TakeDamage(float Damage)
    {
        if (!PhotonView.IsMine) return;
        if (GameState != EGameState.Game) return;

        Stat.ConsumeHealth(Damage);
        Debug.Log("아프다");

        if (Stat.Health <= 0)
        {
            Debug.Log("죽음");
            SetGameState (EGameState.Dead);
            PhotonView.RPC(nameof(_animator.SetDieTrigger), RpcTarget.All);
        }
    }

    public void SetGameState(EGameState state)
    {
        _gameState = state;
    }
}
