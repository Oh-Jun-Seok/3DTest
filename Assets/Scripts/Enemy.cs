using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
public class Enemy : MonoBehaviour
{
    public float speed;
    public Transform chaseTarget;
    public GameObject[] patrolTarget;

    Rigidbody rigi;
    NavMeshAgent nav;
    Animator anim;

    bool isChasing = false;
    float alertTime = 0;
    int patrolindex = 0;
    // Start is called before the first frame update
    void Start()
    {
        rigi = GetComponent<Rigidbody>();
        nav = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (chaseTarget != null)
            isChasing = true;
        else
            isChasing = false;
        Debug.Log(isChasing);
        Debug.Log(chaseTarget);
        if (!isChasing && alertTime <= 0)
            Patrol();
        else if (isChasing)
            Chase();
        else 
        {
            alertTime = alertTime <= 0 ? 3 : alertTime;
            Alert();
            alertTime -= Time.deltaTime;
        }
    }
    private void FixedUpdate()
    {
        FreezeVel();
    }
    void FreezeVel()
    {
        rigi.velocity = Vector3.zero;
        rigi.angularVelocity = Vector3.zero;
    }
    void Patrol()
    {
        /*anim.SetBool("Chase", false);
        anim.SetBool("Alert", false);
        if (transform.position == patrolTarget[patrolindex].transform.position)
            patrolindex = (patrolindex + 1) % 3;
        transform.position = Vector3.MoveTowards(transform.position, patrolTarget[patrolindex].transform.position, speed);
    */}
    void Chase()
    {
        nav.SetDestination(chaseTarget.position);
        anim.SetBool("Run", true);
        anim.SetBool("Alert", false);
    }
    void Alert()
    {
        anim.SetBool("Alert", true);
    }
}
