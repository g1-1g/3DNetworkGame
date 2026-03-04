
public class BearStateMachine
{
    private IState _currentState;

    public IState CurrentState => _currentState;
    public void Init(IState initialState)
    {
        _currentState = initialState;
        _currentState.OnEnter();
    }

    public void Update()
    {
        _currentState?.OnUpdate();
    }

    public void ChangeState(IState newState)
    {
        _currentState.OnExit();
        _currentState = newState;
        _currentState.OnEnter();
    } 
}
