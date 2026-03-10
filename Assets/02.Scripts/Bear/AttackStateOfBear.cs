using UnityEngine;

public class AttackStateOfBear : IState
{
    private readonly BearController _bear;

    public AttackStateOfBear(BearController bear)
    {
        _bear = bear;
    }

    public void OnEnter()
    {
        _bear.OnAttackFinishedEvent += HandleAttackFinished;
        _bear.HitBox.OnHit += HandleHit;
        _bear.Animator.PlayAttack(EBearAttackType.Attack1);
    }

    private void HandleHit(Collider other)
    {
        if (other.TryGetComponent(out IDamageable damageable))
        {
            damageable.TakeDamage(_bear.Stat.Damage, 0);
        }
    }

    public void OnExit()
    {
        _bear.OnAttackFinishedEvent -= HandleAttackFinished;
        _bear.HitBox.OnHit -= HandleHit;
    }

    public void OnUpdate()
    {

    }

    public void HandleAttackFinished()
    {
        _bear.StateMachine.ChangeState(_bear.IdleState);
    }
}
