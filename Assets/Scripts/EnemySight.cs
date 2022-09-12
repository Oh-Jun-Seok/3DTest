using UnityEngine;

public class EnemySight : MonoBehaviour
{
    public GameObject enemy;
    private void Start()
    {
        enemy = transform.parent.gameObject;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if (other.GetComponent<Player>().visible)
                enemy.GetComponent<Enemy>().chaseTarget = other.gameObject.transform;
            else
                enemy.GetComponent<Enemy>().chaseTarget = null;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
            enemy.GetComponent<Enemy>().chaseTarget = null;
    }
}
