using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLook : MonoBehaviour
{
    EnemyMovement _enemyMovement;

    float LookAngle = 30f;
    float LookDistance = 7f;

    [SerializeField] LayerMask TargetMask;
    [SerializeField] LayerMask ObstacleMask;

    private void Start()
    {
        _enemyMovement = GetComponent<EnemyMovement>();
    }
    private void Update()
    {
        Look();
    }
    private void Look()
    {
        Vector3 myPosition = transform.position + Vector3.up;

        float detectobstacleDistance = 2f;
        // 전방 장애물 감지
        if (Physics.Raycast(transform.position, transform.forward, detectobstacleDistance, ObstacleMask))
            _enemyMovement.SetPatrolPoint(transform.eulerAngles.y - 180);

        //Debug Ray
        {
            Vector3 AngleToDir(float angle) // 각도를 백터로 변환
            {
                float radian = angle * Mathf.Deg2Rad;
                return new Vector3(Mathf.Sin(radian), 0f, Mathf.Cos(radian));
            }

            Vector3 rightDir = AngleToDir(transform.eulerAngles.y + LookAngle);
            Vector3 leftDir = AngleToDir(transform.eulerAngles.y - LookAngle);
            Debug.DrawRay(myPosition, rightDir * LookDistance, Color.blue);
            Debug.DrawRay(myPosition, leftDir * LookDistance, Color.blue);
            Debug.DrawRay(myPosition * 1.0f, transform.forward * detectobstacleDistance, Color.yellow);
        }

        Collider[] Targets = Physics.OverlapSphere(myPosition, LookDistance, TargetMask);

        if (Targets.Length == 0) return;

        foreach (Collider EnemyColli in Targets) // 충돌체 판단
        {
            Vector3 targetPos = EnemyColli.transform.position + Vector3.up;
            Vector3 targetDir = (targetPos - myPosition).normalized;
            float targetAngle = Mathf.Acos(Vector3.Dot(transform.forward, targetDir)) * Mathf.Rad2Deg;

            if (targetAngle * 0.5 <= LookAngle && !Physics.Raycast(myPosition, targetDir, LookDistance, ObstacleMask)) // 충돌체가 플레이어임
            {
                Debug.DrawLine(myPosition, targetPos, Color.red);

                _enemyMovement.FindTarget(EnemyColli.transform);
            }
        }
    }
}
