using UnityEngine;

public class IdleStateOfBear : IState
{
    private readonly BearController _bear;

    public IdleStateOfBear(BearController bear)
    {
        _bear = bear;
    }
    public void OnEnter()
    {
        _bear.Animator.SetSpeedRatio(0);
        _bear.StateMachine.ChangeState(_bear.PatrolState);
    }

    public void OnExit()
    {

    }

    public void OnUpdate()
    {
        FindTarget();
    }

    private void FindTarget()
    {
        Collider[] hits = Physics.OverlapSphere(_bear.transform.position, _bear.Stat.SenseRange);
        foreach (var hit in hits)
        {
            if (hit.CompareTag("Player"))
            {
                _bear.SetTarget(hit.transform);
                _bear.StateMachine.ChangeState(_bear.ChaseState);
                break;
            }
        }
    }
}
