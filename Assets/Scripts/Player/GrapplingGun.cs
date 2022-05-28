using UnityEngine;

using UnityEngine.InputSystem;
using FirstGearGames.SmoothCameraShaker;
using TMPro;
using UnityEngine.UI;
public class GrapplingGun : MonoBehaviour
{
    private Vector3 grapplePoint;
    public LayerMask whatIsGrappleable;
    public Transform gunTip, camera, player;
    public float maxDistance = 100f;
    public float spring = 4.5f;
    public float damper = 7f;
    public float massScale = 4.5f;
    public float forcePull = 1000f;
    public float minDistanceMultiplier;
    public float maxDistanceMultiplier;
    private SpringJoint joint;
    public bool startGrapple;
    RaycastHit justHit;
    float timerHit;
    public Image crossHair;
    public Image crossHairNormal;
    public Image crossHairLocked;

    public Input inputActions;
    [SerializeField] ShakeData grapplinShake;
    [SerializeField] PlayerMovementAdvanced playerMovementAdvanced;
    
    private void OnEnable()
    {
        inputActions.Enable();
    }
    private void OnDisable()
    {
        inputActions.Disable();
    }


    void Awake()
    {

        //Inputs
        inputActions = new Input();

        inputActions.InGame.Grappling.performed += StartGrapple;
        inputActions.InGame.Grappling.canceled += StopGrapple;

        timerHit = 0.5f;

    }

    void Update()
    {
        if(startGrapple)
        {
            playerMovementAdvanced.GetComponent<Rigidbody>().AddForce((grapplePoint - transform.position) * forcePull);
        }
        RaycastHit hit;
        if (Physics.Raycast(camera.position, camera.forward, out hit, maxDistance, whatIsGrappleable))
        {
            if(justHit.collider != hit.collider && !IsGrappling() && timerHit <= 0)
            {
                AudioManager.instance.playSoundEffect(14, 1f);
                justHit = hit;
                timerHit = 0.5f;
            }
            crossHair.sprite = crossHairLocked.sprite;
        }
        else
        {
            crossHair.sprite = crossHairNormal.sprite;
        }
        timerHit -= Time.unscaledDeltaTime;
    }

    //Called after Update
    

    // Call whenever we want to start a grapple
    private void StartGrapple(InputAction.CallbackContext callback)
    {
        if(callback.performed)
        {
            RaycastHit hit;
            if (Physics.Raycast(camera.position, camera.forward, out hit, maxDistance, whatIsGrappleable))
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

                //SoundEffect
                AudioManager.instance.playSoundEffect(0, 1f);
                CameraShakerHandler.Shake(grapplinShake);

                
                startGrapple = true;
                playerMovementAdvanced.setGrapplin(true);
            }
        }
    }


    // Call whenever we want to stop a grapple
 

    private Vector3 currentGrapplePosition;

    private void StopGrapple(InputAction.CallbackContext callback)
    {
        if (callback.canceled)
        {
            Destroy(joint);


            if (startGrapple)
            { playerMovementAdvanced.SetCanDoubleJump(true); }

            startGrapple = false;

            playerMovementAdvanced.setGrapplin(false);
            crossHair.sprite = crossHairNormal.sprite;
        }
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
