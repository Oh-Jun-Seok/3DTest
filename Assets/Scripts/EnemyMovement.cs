using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(EnemyLook))]
[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]
public class EnemyMovement : MonoBehaviour
{
    float ChaseTime = 10f;
    float Smoothness = 3f;
    float SprintSpeed = 6;
    float NormalSpeed = 1;
    float PatrolAngle = 90f;
    float NearDistance = 0.3f;
    Vector3 GravityVector = new Vector3(0, -9.8f, 0);

    int _aniSpeedID;
    float _speed;
    bool _isChasing = false;
    bool _isFindSomething = false;
    Vector3 _look;
    Transform _navTarget;
    Rigidbody _rigid;
    Animator _animator;
    NavMeshAgent _agent;
    Coroutine _chasing;
    Coroutine _patroling;

    private void Start()
    {
        _rigid = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
        _agent = GetComponent<NavMeshAgent>();

        AssignAnimationIDs();

        _patroling = StartCoroutine(PatrolTimer());
    }
    private void AssignAnimationIDs() // 애니메이터 파라미터값을 해쉬를 통해 int로 변경
    {
        _aniSpeedID = Animator.StringToHash("Speed");
    }
    private void Update()
    {
        Move();
    }
    private void Move()
    {
        float targetSpeed = NormalSpeed;
        if (!_isChasing) // 순찰
        {
            if (!_isFindSomething) // 일반적 순찰
            {
                _agent.isStopped = true;

                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(_look), Time.deltaTime * Smoothness);

                _rigid.velocity = _look * targetSpeed;
            }
            else // 무언가를 발견
            {
                _agent.isStopped = false;
                _agent.speed = targetSpeed;
                _agent.SetDestination(_navTarget.position);

                if (_agent.remainingDistance < NearDistance)
                    _isFindSomething = false;
            }
        }
        else // 추적
        {
            _agent.isStopped = false;
            targetSpeed = SprintSpeed;
            _agent.speed = targetSpeed;
            _agent.SetDestination(_navTarget.position);
        }
        _animator.SetFloat(_aniSpeedID, targetSpeed);
    }
    private IEnumerator ChaseTimer()
    {
        yield return new WaitForSeconds(ChaseTime);
        _isChasing = false;
    }
    private IEnumerator PatrolTimer()
    {
        while (true)
        {
            float patrolTime = Random.Range(3f, 6f);

            SetPatrolPoint(transform.eulerAngles.y);

            yield return new WaitForSeconds(patrolTime);
        }
    }

    // public fuction
    public void SetPatrolPoint(float _angle)
    {
        float lookingAngle = _angle + Random.Range(PatrolAngle, -PatrolAngle);  //캐릭터가 바라보는 방향의 각도
        float radian = lookingAngle * Mathf.Deg2Rad;

        Vector3 lookDir = new Vector3(Mathf.Sin(radian), 0f, Mathf.Cos(radian));

        _look = lookDir.normalized;
    }
    public void FindTarget(Transform _target)
    {
        _navTarget = _target;
        _isChasing = true;
        _isFindSomething = false;
        _chasing = StartCoroutine(ChaseTimer());
    }
    public void FindSomething(Transform _target)
    {
        _navTarget = _target;
        _isFindSomething = true;
    }
}

