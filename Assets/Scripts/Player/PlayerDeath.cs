using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FirstGearGames.SmoothCameraShaker;

public class PlayerDeath : MonoBehaviour
{
    public static PlayerDeath Instance;
    [SerializeField] PlayerCam playerCam;
    bool grounded;
    [SerializeField]LayerMask whatIsGround;
    bool isDead;
    [SerializeField] ShakeData deathShake;
    private void Awake()
    {
        Instance = this;
    }
    public bool GetIsDead() { return isDead; }
    private void Update()
    {
        grounded = Physics.Raycast(transform.position, Vector3.down, 2f * 0.5f + 0.2f, whatIsGround);
        if (grounded)
        {
            Die();
        }
    }
    public void Die()
    {
        if (!isDead && !FinishLine.Instance.isWin) 
        {
            playerCam.enabled = false;
            gameObject.GetComponent<PlayerMovementAdvanced>().enabled = false;
            //Desactive les controles
            HudControllerInGame.Instance.OpenDeathPanel();
            isDead = true;

            //FeedBack
            CameraShakerHandler.Shake(deathShake);

        }        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 4)
        {
            Die();
            AudioManager.instance.playSoundEffect(7);
            Rumbler.instance.RumbleConstant(2f, 2f, 1.5f);
        }
        if (other.gameObject.layer == 12)
        {
            if(other.gameObject.tag == "Laser")
            {
                Die();
                AudioManager.instance.playSoundEffect(8);
                Rumbler.instance.RumbleConstant(2f, 2f, 1);
            }
        }
    }
}
