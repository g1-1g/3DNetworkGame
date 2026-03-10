using UnityEngine;

public class ChaseStateOfBear : IState
{
    private readonly BearController _bear;

    public ChaseStateOfBear(BearController bear)
    {
        _bear = bear;
    }
    public void OnEnter()
    {
        _bear.Animator.SetSpeedRatio(1);
        _bear.Agent.speed = _bear.Stat.RunSpeed;
    }
    public void OnExit()
    {
        if (_bear.Target != null)
        {
            _bear.SetTarget(null);
        }
        _bear.Agent.ResetPath();
    }
    public void OnUpdate()
    {
        if (_bear.Target == null || Vector3.Distance(_bear.transform.position, _bear.Target.position) > _bear.Stat.ChaseRange)
        {
            _bear.StateMachine.ChangeState(_bear.IdleState);
            return;
        }

        if (Vector3.Distance(_bear.transform.position, _bear.Target.position) < _bear.Stat.AttackRange)
        {
            _bear.StateMachine.ChangeState(_bear.AttackState);
            return;
        }

        _bear.Agent.SetDestination(_bear.Target.position);
    }
}