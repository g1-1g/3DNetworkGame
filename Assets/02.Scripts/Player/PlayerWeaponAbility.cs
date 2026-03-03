using System;
using Photon.Pun;
using UnityEngine;

public class PlayerWeaponAbility : PlayerAbility
{
    [SerializeField] private float _upgradeScaleAdd = 0.1f;
    [SerializeField] private float _upgradeArrange = 300;

    private ColliderBridge _bridge;

    private Vector3 _originalScale;

    protected override void Awake()
    {
        base.Awake();

        _bridge = GetComponentInChildren<ColliderBridge>();

        _bridge.OnTriggerEnterEvent += HandleTrigger;
    }
    private void Start()
    {
        DeactiveCollider();
        _originalScale = _bridge.gameObject.transform.localScale;
        ScoreManager.Instance.OnPlayerScoreChanged += HandlerUpgrade;
    }

    private void HandlerUpgrade()
    {
        if(!_owner.PhotonView.IsMine) return;

        int upgradeLevel = (int)(PlayerScore.GetScore(_owner.PhotonView.Owner) / _upgradeArrange);
        _owner.PhotonView.RPC(nameof(Upgrade), RpcTarget.All, upgradeLevel);
    }

    public void ActiveCollider()
    {
        _bridge.Collider.enabled = true;
    }

    public void DeactiveCollider()
    {
        _bridge.Collider.enabled = false;
    }

    private void HandleTrigger(Collider other)
    {
        if (!_owner.PhotonView.IsMine) return;
        if (other.transform == _owner.transform) return;

        IDamageable damageable = other.gameObject.GetComponent<IDamageable>();

        if (damageable == null) return;

        // 포톤에서는 Room 안에서 플레이어마다 고유 식별자(ID)인 ActorNumber를 가지고 있다.
        int actorNumber = PhotonNetwork.LocalPlayer.ActorNumber;

        PlayerController otherPlayer = other.gameObject.GetComponent<PlayerController>();

        otherPlayer.PhotonView.RPC(nameof(damageable.TakeDamage), RpcTarget.All, _owner.Stat.Damage, actorNumber);
    }

    [PunRPC]
    private void Upgrade(int upgradeLevel)
    {
        Debug.Log("Upgrade Weapon!");
        _bridge.gameObject.transform.localScale = _originalScale + _originalScale * _upgradeScaleAdd * upgradeLevel;
    }

    private void OnDestroy()
    {
        if (_bridge == null) return;
        _bridge.OnTriggerEnterEvent -= HandleTrigger;
    }
}
