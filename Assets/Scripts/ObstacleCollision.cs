using UnityEngine;

public class ObstacleCollision : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 7)
        {
            if(other.gameObject.GetComponent<PlayerDeath>()!= null)
            {
                other.gameObject.GetComponent<PlayerDeath>().Die();
            }
        }
    }
}
