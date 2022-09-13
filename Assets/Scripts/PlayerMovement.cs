using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerMovement : MonoBehaviour
{
    public Transform startPoint;

    float SprintSpeed = 6;
    float NormalSpeed = 2;
    float Smoothness = 10f;
    Vector3 GravityVector = new Vector3(0, -9.8f, 0);

    int _aniSpeedID;
    float _sAxis;
    bool isMoving;

    Camera _camera;
    Animator _animator;
    CharacterController _controller;
    private void Start()
    {
        _camera = Camera.main;
        _animator = GetComponent<Animator>();
        _controller = GetComponent<CharacterController>();

        AssignAnimationIDs();
    }
    private void AssignAnimationIDs() // 애니메이터 파라미터값을 해쉬를 통해 인트로 변경
    {
        _aniSpeedID = Animator.StringToHash("Speed");
    }
    private void Update()
    {
        Move();
    }
    private void LateUpdate()
    {
        Vector3 playerRotate = Vector3.Scale(_camera.transform.forward, new Vector3(1, 0, 1));
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(playerRotate), Time.deltaTime * Smoothness);
        // A상태에서 B상태로 스무스하게 움직이는 것
    }
    private void Move()
    {
        _sAxis = Input.GetAxis("Sprint");

        Vector3 vec_moving = Input.GetAxis("Horizontal") * transform.TransformDirection(Vector3.right) + Input.GetAxis("Vertical") * transform.TransformDirection(Vector3.forward);

        float targetSpeed = (_sAxis > 0) ? SprintSpeed : NormalSpeed;
        isMoving = true;
        if (vec_moving.magnitude == 0)
        {
            isMoving = false;
            targetSpeed = 0;
        }

        _controller.Move((vec_moving * targetSpeed + GravityVector) * Time.deltaTime);

        _animator.SetFloat(_aniSpeedID, targetSpeed);
    }
    public bool GetIsMoving()
    {
        return isMoving;
    }    
}

