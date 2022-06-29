using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FirstGearGames.SmoothCameraShaker;

public class Teleporter : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] Transform destination;
    [SerializeField] float timerRalentieEffect;
    [SerializeField] ShakeData teleportShake;

    // Update is called once per frame
    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.GetComponent<Rigidbody>() != null)
        {
            Rigidbody rb = col.gameObject.GetComponent<Rigidbody>();
            col.transform.position = destination.position;

            if (col.gameObject.GetComponent<PlayerMovementAdvanced>() == true)
            {
                //FeedBack
                CameraShakerHandler.Shake(teleportShake);
                AudioManager.instance.playSoundEffect(6, 0.8f);
                Rumbler.instance.RumbleConstant(0.5f, 1.5f, 2f);

            }
            else if (col.gameObject.GetComponent<MenuAntCrontroller>() == true)
            {
                //FeedBack
                CameraShakerHandler.Shake(teleportShake);
                AudioManager.instance.playSoundEffect(6, 0.8f);
                Rumbler.instance.RumbleConstant(0.5f, 1.5f, 2f);

            }
            rb.velocity = new Vector3(rb.velocity.x,0,rb.velocity.z);
        }
    }
}
