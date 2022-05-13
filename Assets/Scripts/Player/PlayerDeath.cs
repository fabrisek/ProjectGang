using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeath : MonoBehaviour
{
    private void Die()
    {
        //Desactive les controles
        HudControllerInGame.Instance.OpenDeathPanel();
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 6)
        {
            Debug.Log(other.gameObject);
            Die();
        }
    }
}
