using System;
using Photon.Pun;
using UnityEngine;
using UnityEngine.AI;

public class BearController : MonoBehaviour
{
    private Transform _target;
    public BearAnimator Animator { get; private set; }

    public NavMeshAgent Agent { get; private set; }

    public PhotonView PhotonView { get; private set; }

    public BearStateMachine StateMachine { get; private set; } = new BearStateMachine();

    public IdleStateOfBear IdleState { get; private set; }
    public ChaseStateOfBear ChaseState { get; private set; }
    public PatrolStateOfBear PatrolState { get; private set; }
    public AttackStateOfBear AttackState { get; private set; }

    public BearStat Stat { get; private set; }
    
    public Transform Target => _target;

    public HitBox HitBox;

    public event Action OnAttackFinishedEvent;
    public event Action OnGetHitFinishedEvent;

    void Awake()
    {
        Animator = GetComponent<BearAnimator>();
        Stat = GetComponent<BearStat>();
        Agent = GetComponent<NavMeshAgent>();
        PhotonView = GetComponent<PhotonView>();

        IdleState = new IdleStateOfBear(this);
        ChaseState = new ChaseStateOfBear(this);
        PatrolState = new PatrolStateOfBear(this);
        AttackState = new AttackStateOfBear(this);
    }

    private void Start()
    {
        StateMachine.Init(PatrolState);
    }

    private void Update()
    {
        StateMachine.Update();
    }

    public void SetTarget(Transform transform)
    {
        _target = transform;
    }

    public void OnAttackFinished()  // 애니메이션 이벤트
    {
        OnAttackFinishedEvent?.Invoke();
    }

    public void OnGetHitFinished()
    {
        OnGetHitFinishedEvent?.Invoke();
    }
}
