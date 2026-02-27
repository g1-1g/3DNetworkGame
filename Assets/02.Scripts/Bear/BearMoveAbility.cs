using System.Diagnostics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class BearMoveAbility : BearAbility
{
    private float _currentSpeed;

    private NavMeshAgent _agent;

    [SerializeField] private float _acceleration = 8;
    [SerializeField] private float _patrolRadius = 10;

    private bool _isTracing;
    private bool _isPatrolling;

    protected override void Awake()
    {
        base.Awake();

        _agent = GetComponent<NavMeshAgent>();
    }

    public bool Trace(Vector3 position)
    {
        if (!_isTracing || _agent.destination != position)
        {        
            Run(position);

            _isTracing = true;
        }

        if (_agent.remainingDistance < 0.1f)
        {
            return true;
        }

        return false;
        
    }

    public void Patrol()
    {
        if (!_isPatrolling)
        {
            Vector3 result;

            while (true)
            {
                if (TryGetRandomNavMeshPoint(transform.position, _patrolRadius, out result))
                {
                    break;
                }
            }

            Walk(result);

            _isPatrolling = true;
        }
        else if(_agent.remainingDistance < 0.1f)
        {
            Stop();
        }  
    }

    // center 기준 반경 radius 안에서 랜덤 포인트 (균일 분포)
    public static Vector3 RandomPointInCircle(Vector3 center, float radius)
    {
        float r = Mathf.Sqrt(Random.value) * radius; // 균일하게 뽑기 핵
        
          float theta = Random.value * Mathf.PI * 2f;

        float x = r * Mathf.Cos(theta);
        float z = r * Mathf.Sin(theta);

        return center + new Vector3(x, 0f, z);
    }

    // NavMesh 위 점으로 보정
    public static bool TryGetRandomNavMeshPoint(Vector3 center, float radius, out Vector3 result)
    {
        Vector3 candidate = RandomPointInCircle(center, radius);

        if (NavMesh.SamplePosition(candidate, out NavMeshHit hit, 2f, NavMesh.AllAreas))
        {
            result = hit.position;
            return true;
        }

        result = center;
        return false;
    }

    public void Walk(Vector3 position)
    {
        Move(position, _context.Stat.WalkSpeed);
    }

    public void Run(Vector3 position)
    {
        Move(position, _context.Stat.RunSpeed);
    }

    private void Move(Vector3 position, float speed)
    {
        _currentSpeed = Mathf.MoveTowards(_currentSpeed, speed, _acceleration * Time.deltaTime);
        _agent.speed = _currentSpeed;

        if (_agent.destination == null || _agent.destination != position)
        {
            _agent.SetDestination(transform.position);     
        }
    }

    public void Stop()
    {
        _agent.ResetPath();

        _isPatrolling = false;
        _isTracing = false;
    }
}
