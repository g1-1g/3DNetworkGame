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
        _bear.Animator.PlayAttack(EBearAttackType.Attack1);
    }

    public void OnExit()
    {
        _bear.OnAttackFinishedEvent -= HandleAttackFinished;
    }

    public void OnUpdate()
    {

    }

    public void HandleAttackFinished()
    {
        _bear.StateMachine.ChangeState(_bear.IdleState);
    }
}
