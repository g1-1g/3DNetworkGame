using UnityEngine;
using UnityEngine.AI;

public class PatrolStateOfBear : IState
{
    private readonly BearController _bear;

    private bool _isPatrolling;

    public PatrolStateOfBear(BearController bear)
    {
        _bear = bear;
    }

    public void OnEnter()
    {
        _bear.Animator.SetSpeedRatio(0.5f);
        _bear.Agent.speed = _bear.Stat.WalkSpeed;
    }

    public void OnExit()
    {
        _isPatrolling = false;
        _bear.Animator.SetSpeedRatio(0f);
        _bear.Agent.ResetPath();
    }

    public void OnUpdate()
    {
        Patrol();
        FindTarget();
    }

    private void Patrol()
    {
        if (!_isPatrolling)
        {
            _isPatrolling = true;
            _bear.Agent.SetDestination(GetReachablePoint());
        }
        else 
        {
            if (_bear.Agent.remainingDistance <= _bear.Agent.stoppingDistance)
            {
                _isPatrolling = false;
            }
        }
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

    public Vector3 GetReachablePoint(
          int areaMask = NavMesh.AllAreas,
          int maxTries = 20)
    {
        for (int i = 0; i < 20; i++)
        {
            Vector2 r = Random.insideUnitCircle * _bear.Stat.PatrolRange;
            Vector3 candidate = _bear.transform.position + new Vector3(r.x, 0f, r.y);

            // 1) 후보 좌표를 NavMesh 위 좌표로 보정
            if (!NavMesh.SamplePosition(candidate, out NavMeshHit hit, 1.5f, areaMask))
                continue;

            // 2) 실제 경로가 완전히 이어지는지 검사
            NavMeshPath path = new NavMeshPath();
            bool hasPath = NavMesh.CalculatePath(_bear.transform.position, hit.position, areaMask, path);

            if (hasPath && path.status == NavMeshPathStatus.PathComplete)
            {
                _bear.Agent.SetDestination(hit.position);
                return hit.position;
            }
        }
        return _bear.transform.position;
    }
}