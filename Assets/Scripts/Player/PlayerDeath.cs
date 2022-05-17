using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeath : MonoBehaviour
{
    bool grounded;
    [SerializeField]LayerMask whatIsGround;
    bool isDead;
    private void Update()
    {
        grounded = Physics.Raycast(transform.position, Vector3.down, 2f * 0.5f + 0.2f, whatIsGround);
        print(grounded);
        if (grounded)
        {
            Die();
        }
    }
    private void Die()
    {
        if (!isDead)
        {
            //Desactive les controles
            HudControllerInGame.Instance.OpenDeathPanel();
            isDead = true;
        }

        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 4)
        {
            Debug.Log(other.gameObject);
            Die();
        }
    }
}
