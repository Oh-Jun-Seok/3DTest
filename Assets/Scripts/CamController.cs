using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamController : MonoBehaviour
{
    public Transform objectToFollow; // ���� ��ü
    public Transform realCamera; // ����ٴϴ� ī�޶� ����

    private Vector3 StartVector = new Vector3(0, 1.3f, 0);
    private float FollowSpeed = 10f; // ���� �ӵ�
    private float Sensitivity = 100f; // ���콺 �ΰ���
    private float ClampAngle = 70f; // �ִ� �þ߰�
    private float MinDistance = 1f; // �ּ� �Ÿ�
    private float MaxDistance = 2f; // �ִ� �Ÿ�
    private float Smoothness = 10f; // ī�޶� ������ �ε巯��

    // ���콺 ��ǲ�� ����
    private float _rotX;
    private float _rotY;

    private float _finalDistance; // ���� �Ÿ�
    private Vector3 _dirNormalized; // ī�޶� ����
    private Vector3 _finalDir; // ���� ����


    private void Start()
    {
        _rotX = transform.localRotation.eulerAngles.x;
        _rotY = transform.localRotation.eulerAngles.y;

        _dirNormalized = realCamera.localPosition.normalized; // ����
        _finalDistance = realCamera.localPosition.magnitude; // ũ��

    }
    private void Update()
    {
        _rotX += -(Input.GetAxis("Mouse Y") * Sensitivity * Time.deltaTime);
        _rotY += Input.GetAxis("Mouse X") * Sensitivity * Time.deltaTime;
        // ���콺 y���� �������� �����̸� ����Ƽ�� x���� �������� ȸ���ϴ� ���̹Ƿ� �ݴ�� ��

        _rotX = Mathf.Clamp(_rotX, -ClampAngle,ClampAngle); // �ִ� �ּҰ� ����
        Quaternion rot = Quaternion.Euler(_rotX, _rotY, 0); // ����
        transform.rotation = rot;
    }
    private void LateUpdate()
    {
        transform.position = Vector3.MoveTowards(transform.position, objectToFollow.position, FollowSpeed * Time.deltaTime); // ����ٴϱ�

        _finalDir = transform.TransformPoint(_dirNormalized * MaxDistance); // ������ �ٶ󺻴�

        RaycastHit hit;

        if(Physics.Linecast(transform.position, _finalDir, out hit))
            _finalDistance = Mathf.Clamp(hit.distance, MinDistance, MaxDistance);
        else
            _finalDistance = MaxDistance;

        realCamera.localPosition = Vector3.Lerp(realCamera.localPosition, _dirNormalized * _finalDistance, Time.deltaTime * Smoothness);
    }
}
