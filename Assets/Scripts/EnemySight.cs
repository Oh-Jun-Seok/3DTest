using UnityEngine;

public class EnemySight : MonoBehaviour
{
    public GameObject enemy;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
            if (other.GetComponent<Player>().visible)
                enemy.GetComponent<Enemy>().chaseTarget = other.gameObject.transform;
    }
}
