using UnityEngine;

public class BearState : MonoBehaviour
{
    private EMonsterState _state;

    public EMonsterState State => _state;

    private BearAttackAbility _attackAbility;
    private BearMoveAbility _moveAbility;
    private BearGetHitAbility _getHitAbility;

    private void Awake()
    {
        _attackAbility = GetComponent<BearAttackAbility>();
        _moveAbility = GetComponent<BearMoveAbility>();
        _getHitAbility = GetComponent<BearGetHitAbility>();
    }

    private void Start()
    {
        
    }
    private void Update()
    {
        switch (_state)
        {
            case EMonsterState.Idle:
                break;
            case EMonsterState.Patrol:
                _moveAbility.Patrol();
                break;
            case EMonsterState.Trace:
                //_moveAbility.Trace();
                break;
            case EMonsterState.Sleep:
                break;
            case EMonsterState.Attack:
                break;
            case EMonsterState.Damaged:
                break;
            case EMonsterState.Die:
                break;
        }
    }

    public void SetState(EMonsterState state)
    {
        switch (state)
        {
            case EMonsterState.Idle:
                _moveAbility.Stop();
                break;
            case EMonsterState.Patrol:
                _moveAbility.Stop();
                break;
            case EMonsterState.Trace:
                _moveAbility.Stop();
                break;
            case EMonsterState.Sleep: 
                break;
            case EMonsterState.Attack: 
                break;
            case EMonsterState.Damaged: 
                break;
            case EMonsterState.Die: 
                break;
        }

        _state = state;
    }

}
