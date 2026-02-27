using Photon.Pun;
using UnityEngine;

public class BearAnimator : MonoBehaviour
{
    private Animator _animator;

    private readonly int _speedRatioHash = Animator.StringToHash("Blend");
    private readonly int _attackTriggerHash = Animator.StringToHash("Attack");
    private readonly int _attackTypeHash = Animator.StringToHash("AttackType");

    private readonly int _isDieHash = Animator.StringToHash("Die");
    private readonly int _getHitTriggerHash = Animator.StringToHash("GetHit");

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void SetSpeedRatio(float ratio)
    {
        _animator.SetFloat(_speedRatioHash, ratio);
    }

    public void SetAttackTrigger()
    {
        _animator.SetTrigger(_attackTriggerHash);
    }

    [PunRPC]
    public void SetGetHitTrigger()
    {
        _animator.SetTrigger(_getHitTriggerHash);
    }

    [PunRPC]
    public void PlayAttack(EPlayerAttackType type)
    {
        _animator.SetInteger(_attackTypeHash, (int)type);
        SetAttackTrigger();
    }

    [PunRPC]
    public void SetIsDie(bool isDie)
    {
        _animator.SetBool(_isDieHash, isDie);
    }
}
