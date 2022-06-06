using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ByPass
{
    public class JumpPad : MonoBehaviour
    {
        [SerializeField] float force;
        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer == 7)
            {
                //InputManager.Instance.CameraShake(ventiloShake);
                AudioManager.instance.playSoundEffect(6, 0.8f);
                Rumbler.instance.RumbleConstant(0.5f, 1.5f, 0.3f);
            }

            if (other.GetComponent<Rigidbody>() != null && other.gameObject.layer == 7)
            {
                Rigidbody rb = other.GetComponent<Rigidbody>();
                rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

                other.GetComponent<PlayerMovementAdvanced>().SetCanDoubleJump(true);

                if (other.GetComponent<CompetenceRalentie>().IsRalentie())
                {
                    rb.AddForce(force * transform.up * 1.5f, ForceMode.Impulse);
                }

                else
                    rb.AddForce(force * transform.up, ForceMode.Impulse);
            }
        }

    }
}
