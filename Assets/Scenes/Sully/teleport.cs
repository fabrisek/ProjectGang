using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FirstGearGames.SmoothCameraShaker;

public class teleport : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] Transform destination;
    [SerializeField] float timerRalentieEffect;
    [SerializeField] ShakeData teleportShake;

    // Update is called once per frame
    void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.GetComponent<Rigidbody>()!= null)
        {
            col.transform.position = destination.position;
            if(col.gameObject.GetComponent<PlayerMovementAdvanced>()==true)
            {

                //FeedBack
                CameraShakerHandler.Shake(teleportShake);
                AudioManager.instance.playSoundEffect(6, 0.8f);
                Rumbler.instance.RumbleConstant(0.5f, 1.5f, 2f);
            }
        }
    }
}
