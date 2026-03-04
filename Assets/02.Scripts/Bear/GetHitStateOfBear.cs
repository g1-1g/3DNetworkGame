using UnityEngine;

public class GetHitStateOfBear : IState
{
    private readonly BearController _bear;

    public GetHitStateOfBear(BearController bear)
    {
        _bear = bear;
    }
    public void OnEnter()
    {
        _bear.OnGetHitFinishedEvent += HandleGetHitFinished;
        _bear.Animator.SetGetHitTrigger();
    }

    public void OnExit()
    {
        _bear.OnGetHitFinishedEvent -= HandleGetHitFinished;
    }

    public void OnUpdate()
    {
 
    }

    public void HandleGetHitFinished()
    {
        _bear.StateMachine.ChangeState(_bear.IdleState);
    }
}
