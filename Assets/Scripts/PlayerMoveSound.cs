using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveSound : MonoBehaviour
{
    public PlayerMovement playerMovement;

    float MaxMoveSound = 10f;
    float MinMoveSound = 0f;
    float WalkMoveSound = 0.5f;
    float SprintMoveSound = 2f;
    float WalkMoveSoundTerm = 0.3f;
    float SprintMoveSoundTerm = 0.2f;

    SphereCollider _sphereCollider;

    private void Start()
    {
        _sphereCollider = GetComponent<SphereCollider>();

        StartCoroutine(Co_PlusWalkSound());
        StartCoroutine(Co_PlusSprintSound());
    }
    private void Update()
    {
        //_sphereCollider.radius -= 0.01f;
        if (_sphereCollider.radius < MinMoveSound)
            _sphereCollider.radius = MinMoveSound;
    }
    private IEnumerator Co_PlusWalkSound()
    {
        while (true)
        {
            if (playerMovement.GetIsMoving() && (Input.GetAxis("Sprint") == 0))
            {
                _sphereCollider.radius += WalkMoveSound;
                _sphereCollider.radius = Mathf.Min(MaxMoveSound, _sphereCollider.radius);
            }
            yield return new WaitForSeconds(WalkMoveSoundTerm);
        }
    }
    private IEnumerator Co_PlusSprintSound()
    {
        while (true)
        {
            if (playerMovement.GetIsMoving() && (Input.GetAxis("Sprint") > 0))
            {
                _sphereCollider.radius += SprintMoveSound;
                _sphereCollider.radius = Mathf.Min(MaxMoveSound, _sphereCollider.radius);
            }
            yield return new WaitForSeconds(SprintMoveSoundTerm);
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            other.gameObject.GetComponent<EnemyMovement>().FindSomething(transform);
        }
    }
}
