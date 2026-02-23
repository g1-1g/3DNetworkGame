using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerAnimator : MonoBehaviour
{
    private Animator _animator;

    private readonly int _speedRatioHash = Animator.StringToHash("Blend");
    private readonly int _attackTriggerHash = Animator.StringToHash("Attack");
    private readonly int _attackTypeHash = Animator.StringToHash("AttackType");

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

    public void PlayAttack(AttackType type)
    {
        _animator.SetInteger(_attackTypeHash, (int)type);
        SetAttackTrigger();
    }
}
