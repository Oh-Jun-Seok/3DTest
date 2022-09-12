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
    public float walkSpeed;
    public float runSpeed;
    public Transform chaseTarget;
    public GameObject[] patrolTarget;

    NavMeshAgent nav;
    Animator anim;

    bool isChasing = false;
    bool isback = false;
    float alertTime = 0;
    int patrolindex = 0;
    // Start is called before the first frame update
    void Start()
    {
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
        if (!isChasing && alertTime <= 0)
            Patrol();
        else if (isChasing)
            Chase();
        else 
        {
            Alert();
            alertTime -= Time.deltaTime;
        }
    }
    void Patrol()
    {
        anim.SetBool("Run", false);
        anim.SetBool("Alert", false);

        alertTime = 3f;
        nav.speed = walkSpeed;
        nav.SetDestination(patrolTarget[patrolindex].transform.position);
    }
    void Chase()
    {
        nav.speed = runSpeed;
        nav.SetDestination(chaseTarget.position);
        anim.SetBool("Run", true);
        anim.SetBool("Alert", false);
    }
    void Alert()
    {
        anim.SetBool("Alert", true);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Patrol")
        {
            if (patrolindex + 1 == patrolTarget.Length)
                isback = true;
            else if (patrolindex == 0)
                isback = false;
            if (!isback)
                patrolindex++;
            else
                patrolindex--;
        }
    }
}
