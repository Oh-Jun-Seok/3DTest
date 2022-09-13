using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamController : MonoBehaviour
{
    public Transform objectToFollow; // 따라갈 객체
    public Transform realCamera; // 따라다니는 카메라 정보

    private Vector3 StartVector = new Vector3(0, 1.3f, 0);
    private float FollowSpeed = 10f; // 따라갈 속도
    private float Sensitivity = 100f; // 마우스 민감도
    private float ClampAngle = 70f; // 최대 시야각
    private float MinDistance = 1f; // 최소 거리
    private float MaxDistance = 2f; // 최대 거리
    private float Smoothness = 10f; // 카메라 움직임 부드러움

    // 마우스 인풋용 변수
    private float _rotX;
    private float _rotY;

    private float _finalDistance; // 최종 거리
    private Vector3 _dirNormalized; // 카메라 방향
    private Vector3 _finalDir; // 최종 방향


    private void Start()
    {
        _rotX = transform.localRotation.eulerAngles.x;
        _rotY = transform.localRotation.eulerAngles.y;

        _dirNormalized = realCamera.localPosition.normalized; // 방향
        _finalDistance = realCamera.localPosition.magnitude; // 크기

    }
    private void Update()
    {
        _rotX += -(Input.GetAxis("Mouse Y") * Sensitivity * Time.deltaTime);
        _rotY += Input.GetAxis("Mouse X") * Sensitivity * Time.deltaTime;
        // 마우스 y축을 기준으로 움직이면 유니티의 x축을 기준으로 회전하는 꼴이므로 반대로 함

        _rotX = Mathf.Clamp(_rotX, -ClampAngle,ClampAngle); // 최대 최소각 설정
        Quaternion rot = Quaternion.Euler(_rotX, _rotY, 0); // 각도
        transform.rotation = rot;
    }
    private void LateUpdate()
    {
        transform.position = Vector3.MoveTowards(transform.position, objectToFollow.position, FollowSpeed * Time.deltaTime); // 따라다니기

        _finalDir = transform.TransformPoint(_dirNormalized * MaxDistance); // 방향을 바라본다

        RaycastHit hit;

        if(Physics.Linecast(transform.position, _finalDir, out hit))
            _finalDistance = Mathf.Clamp(hit.distance, MinDistance, MaxDistance);
        else
            _finalDistance = MaxDistance;

        realCamera.localPosition = Vector3.Lerp(realCamera.localPosition, _dirNormalized * _finalDistance, Time.deltaTime * Smoothness);
    }
}
