using UnityEngine;

using UnityEngine.InputSystem;
using FirstGearGames.SmoothCameraShaker;
using TMPro;
using UnityEngine.UI;
public class GrapplingGun : MonoBehaviour
{
    public static GrapplingGun Instance;
    private Vector3 grapplePoint;
    public LayerMask whatIsGrappleable;
    public Transform gunTip, cam, player;
    float maxDistance = 75f;
    public float spring = 4.5f;
    public float damper = 7f;
    public float massScale = 4.5f;
    public float forcePull = 1000f;
    public float minDistanceMultiplier;
    public float maxDistanceMultiplier;
    private SpringJoint joint;
    public bool startGrapple;
    RaycastHit justHit;
    Vector3 justHitVelocity;
    float timerHit;
    Transform hitPoint;


    public Image crossHair;
    public Image crossHairNormal;
    public Image crossHairLocked;
    


    [SerializeField] ShakeData grapplinShake;
    [SerializeField] PlayerMovementAdvanced playerMovementAdvanced;
    [SerializeField] GameObject particuleHit;
    [SerializeField] CompetenceRalentie competenceRalentie;

    [SerializeField] Animator grappinAnimator;
    private  Rigidbody rbHit;

    void Awake()
    {
        particuleHit.SetActive(false);
        particuleHit.transform.parent = null;
        particuleHit.transform.localScale = Vector3.one*0.3f;
        //Inputs
        Instance = this;
        timerHit = 0.5f;
    }

    private void FixedUpdate()
    {
        if (startGrapple)
        {
            playerMovementAdvanced.GetComponent<Rigidbody>().AddForce((grapplePoint - transform.position) * forcePull);
        }
    }
    void Update()
    {
        
        if(competenceRalentie.IsRalentie())
        {
            maxDistance = 30f;
        }
        else
        {
            maxDistance = 15f;
        }
        RaycastHit hit;
        if (Physics.Raycast(cam.position, cam.forward, out hit, maxDistance, whatIsGrappleable))
        {
            if(justHit.collider != hit.collider && !IsGrappling() && timerHit <= 0)
            {
                AudioManager.instance.playSoundEffect(14, 1f);
                justHit = hit;
                timerHit = 0.5f;
            }
            //particuleGunTipLock.SetActive(true);
            crossHair.sprite = crossHairLocked.sprite;
            crossHair.color = Color.red;
        }
        else
        {
            
            crossHair.sprite = crossHairNormal.sprite;
            crossHair.color = Color.white;
            //if (!IsGrappling())
            //particuleGunTipLock.SetActive(false);
        }
        timerHit -= Time.unscaledDeltaTime;
        //grapplePoint = justHit.collider.transform.position;

    }

    //Called after Update
    

    // Call whenever we want to start a grapple
    public void StartGrapple()
    {

            RaycastHit hit;
            if (Physics.Raycast(cam.position, cam.forward, out hit, maxDistance, whatIsGrappleable))
            {
                grapplePoint = hit.point;
                
                joint = player.gameObject.AddComponent<SpringJoint>();
                joint.autoConfigureConnectedAnchor = false;
                joint.connectedAnchor = grapplePoint;

                float distanceFromPoint = Vector3.Distance(player.position, grapplePoint);

                //The distance grapple will try to keep from grapple point. 
                joint.maxDistance = distanceFromPoint * maxDistanceMultiplier;
                joint.minDistance = distanceFromPoint * minDistanceMultiplier;

                //Adjust these values to fit your game.
                joint.spring = spring;
                joint.damper = damper;
                joint.massScale = massScale;

               /* if (justHit.collider.TryGetComponent<Rigidbody>(out rbHit))
                {
                rbHit.isKinematic = true;
                justHitVelocity = rbHit.velocity;
                }*/

                //SoundEffect
                AudioManager.instance.playSoundEffect(0, 1f);
                //InputManager.Instance.CameraShake(grapplinShake);

                //particule
                particuleHit.transform.position = grapplePoint;
                
                particuleHit.SetActive(true);
                
                startGrapple = true;
                playerMovementAdvanced.setGrapplin(true);

                //anim
                grappinAnimator.SetBool("Grappling", true);
            }
    }
    // Call whenever we want to stop a grapple
    public void StopGrapple()
    {

            Destroy(joint);
            particuleHit.SetActive(false);
            if (startGrapple)
            { playerMovementAdvanced.SetCanDoubleJump(true); }

            startGrapple = false;

            playerMovementAdvanced.setGrapplin(false);
            crossHair.sprite = crossHairNormal.sprite;
            crossHair.color = Color.white;

            //anim
            grappinAnimator.SetBool("Grappling", false);
            /*if (justHit.collider.TryGetComponent<Rigidbody>(out rbHit))
            {
                rbHit.isKinematic = false;
                rbHit.velocity = justHitVelocity;
            }*/

    }
    

    public bool IsGrappling()
    {
        return joint != null;
    }

    public Vector3 GetGrapplePoint()
    {
        return grapplePoint;
    }
}
