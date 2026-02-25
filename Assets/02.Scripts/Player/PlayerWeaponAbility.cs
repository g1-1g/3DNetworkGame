using Photon.Pun;
using UnityEngine;

public class PlayerWeaponAbility : PlayerAbility
{
    private ColliderBridge _bridge;

    protected override void Awake()
    {
        base.Awake();

        _bridge = GetComponentInChildren<ColliderBridge>();

        _bridge.OnTriggerEnterEvent += HandleTrigger;
    }
    private void Start()
    {
        DeactiveCollider();
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

        PlayerController otherPlayer = other.gameObject.GetComponent<PlayerController>();

        otherPlayer.PhotonView.RPC(nameof(damageable.TakeDamage), RpcTarget.All, _owner.Stat.Damage);
    }

    private void OnDestroy()
    {
        if (_bridge == null) return;
        _bridge.OnTriggerEnterEvent -= HandleTrigger;
    }
}
