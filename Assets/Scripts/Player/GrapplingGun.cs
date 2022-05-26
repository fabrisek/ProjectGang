using UnityEngine;

using UnityEngine.InputSystem;
using FirstGearGames.SmoothCameraShaker;
public class GrapplingGun : MonoBehaviour
{

    private LineRenderer lr;
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
    bool startGrapple;

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
        lr = GetComponent<LineRenderer>();

        //Inputs
        inputActions = new Input();

        inputActions.InGame.Grappling.performed += StartGrapple;
        inputActions.InGame.Grappling.canceled += StopGrapple;

    }

    void Update()
    {
        if(startGrapple)
        {
            playerMovementAdvanced.GetComponent<Rigidbody>().AddForce((grapplePoint - transform.position) * forcePull);
        }
    }

    //Called after Update
    void LateUpdate()
    {
        DrawRope();
    }

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

                lr.positionCount = 2;
                currentGrapplePosition = gunTip.position;

                //SoundEffect
                AudioManager.instance.playSoundEffect(0);
                CameraShakerHandler.Shake(grapplinShake);

                playerMovementAdvanced.SetCanDoubleJump(true);
                startGrapple = true;
                playerMovementAdvanced.setGrapplin(true);
            }
        }
    }


    // Call whenever we want to stop a grapple
    private void StopGrapple(InputAction.CallbackContext callback)
    {
        if(callback.canceled)
        {
            lr.positionCount = 0;
            Destroy(joint);
            startGrapple = false;
            playerMovementAdvanced.setGrapplin(false);
        }
    }

    private Vector3 currentGrapplePosition;

    void DrawRope()
    {
        //If not grappling, don't draw rope
        if (!joint) return;

        currentGrapplePosition = Vector3.Lerp(currentGrapplePosition, grapplePoint, Time.deltaTime * 8f);

        lr.SetPosition(0, gunTip.position);
        lr.SetPosition(1, currentGrapplePosition);
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
