using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FirstGearGames.SmoothCameraShaker;

public class WallRunningAdvanced : MonoBehaviour
{
    public static WallRunningAdvanced Instance;
    [Header("Wallrunning")]
    public LayerMask whatIsWall;
    public LayerMask whatIsGround;
    public float wallRunForce;
    public float wallJumpUpForce;
    public float wallJumpSideForce;
    public float wallClimbSpeed;
    public float maxWallRunTime;
    private float wallRunTimer;
    private float timerFoostep;

    [Header("CameraEffects")]
    [SerializeField] float tilt;
    [SerializeField] float fovWall;
    [SerializeField] float fovNormal;
    [SerializeField] ShakeData wallRunShake;

    [Header("Input")]
    private bool upwardsRunning;
    private bool downwardsRunning;
    private float horizontalInput;
    private float verticalInput;

    [Header("Detection")]
    public float wallCheckDistance;
    public float minJumpHeight;
    private RaycastHit leftWallhit;
    private RaycastHit rightWallhit;
    private bool wallLeft;
    private bool wallRight;

    [Header("Exiting")]
    private bool exitingWall;
    public bool GetExitingWall()
    {
        return exitingWall;
    }

    public float exitWallTime;
    private float exitWallTimer;

    [Header("Gravity")]
    public bool useGravity;
    public float gravityCounterForce;

    [Header("References")]
    public Transform orientation;
    public PlayerCam cam;
    private PlayerMovementAdvanced pm;
    private Rigidbody rb;


    //bug doublejumpwall
    float timerDouble = 0.3f;
    bool stopWallRun;

    private void Awake()
    {
        Instance = this;
        
    }

    //get inputs


    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        pm = GetComponent<PlayerMovementAdvanced>();
        timerFoostep = 0f;
    }

    private void Update()
    {
        CheckForWall();
        StateMachine();

        timerDouble -= Time.deltaTime;
        if(timerDouble<0&&stopWallRun)
        {
            stopWallRun = false;
            pm.SetCanDoubleJump(true);
        }
        

    }

    private void FixedUpdate()
    {
        if (pm.wallrunning)
            WallRunningMovement();
    }

    private void CheckForWall()
    {
        wallRight = Physics.Raycast(transform.position, orientation.right, out rightWallhit, wallCheckDistance, whatIsWall);
        wallLeft = Physics.Raycast(transform.position, -orientation.right, out leftWallhit, wallCheckDistance, whatIsWall);
    }

    private bool AboveGround()
    {
        return !Physics.Raycast(transform.position, Vector3.down, minJumpHeight, whatIsGround);
    }

    private void StateMachine()
    {
        // Getting Inputs
        horizontalInput = InputManager.Instance.GetPlayerMovement().x;
        verticalInput = InputManager.Instance.GetPlayerMovement().y;

        /*upwardsRunning = Input.GetKey(upwardsRunKey);
        downwardsRunning = Input.GetKey(downwardsRunKey);*/

        // State 1 - Wallrunning
        if((wallLeft || wallRight) && verticalInput > 0 && AboveGround() && !exitingWall)
        {
            if (!pm.wallrunning)
                StartWallRun();

            // wallrun timer
            if (wallRunTimer > 0)
            {
                wallRunTimer -= Time.deltaTime;
            }
            
            if(wallRunTimer <= 0 && pm.wallrunning)
            {
                exitingWall = true;
                exitWallTimer = exitWallTime;
            }


            //FeedBack
            timerFoostep -= Time.deltaTime;
            if (timerFoostep <= 0)
            {
                AudioManager.instance.playSoundEffect(5, 1f);
                Rumbler.instance.RumbleConstant(1f, 1f, 0.1f);
                timerFoostep += 0.25f;
                
                //CameraShakerHandler.Shake(wallRunShake);
            }
            
        }

        // State 2 - Exiting
        else if (exitingWall)
        {
            if (pm.wallrunning)
                StopWallRun();

            if (exitWallTimer > 0)
                exitWallTimer -= Time.deltaTime;

            if (exitWallTimer <= 0)
                exitingWall = false;
        }

        // State 3 - None
       else
       {
            if (pm.wallrunning)
                StopWallRun();
       }
       
    }

    private void StartWallRun()
    {
        Debug.Log("wallRunStart");
        //Rumbler.instance.RumblePulse(0.5f, 1.5f, 0.1f, 1f);
        pm.wallrunning = true;
        wallRunTimer = maxWallRunTime;

        rb.velocity = new Vector3(rb.velocity.x,0f, rb.velocity.z);

        // apply camera effects
        if (wallLeft) cam.DoTilt(-tilt);
        if (wallRight) cam.DoTilt(tilt);
        
    }

    private void WallRunningMovement()
    {
        rb.useGravity = useGravity;

        Vector3 wallNormal = wallRight ? rightWallhit.normal : leftWallhit.normal;

        Vector3 wallForward = Vector3.Cross(wallNormal, transform.up);

        if ((orientation.forward - wallForward).magnitude > (orientation.forward - -wallForward).magnitude)
            wallForward = -wallForward;

        // forward force
        rb.AddForce(wallForward * wallRunForce, ForceMode.Force);

        // upwards/downwards force
        if (upwardsRunning)
            rb.velocity = new Vector3(rb.velocity.x, wallClimbSpeed, rb.velocity.z);
        if (downwardsRunning)
            rb.velocity = new Vector3(rb.velocity.x, -wallClimbSpeed, rb.velocity.z);

        // push to wall force
        if (!(wallLeft && horizontalInput > 0) && !(wallRight && horizontalInput < 0))
            rb.AddForce(-wallNormal * 100, ForceMode.Force);

        // weaken gravity
        if (useGravity)
            rb.AddForce(transform.up * gravityCounterForce, ForceMode.Force);
    }

    private void StopWallRun()
    {
        pm.wallrunning = false;
        
        // reset camera effects
        cam.DoTilt(0f);
        timerFoostep = 0.01f;

        Debug.Log("wallRunStop");
        
        timerDouble = 0.3f;
        stopWallRun = true;
    }

    public void WallJump()
    {
        if (pm != null)
        {


            if (pm.wallrunning)
            {
                Debug.Log("wallRunJump");
                pm.PlayerJumpDown(false);
                pm.SetCanDoubleJump(false);
                // enter exiting wall state
                exitingWall = true;
                exitWallTimer = exitWallTime;

                Vector3 wallNormal = wallRight ? rightWallhit.normal : leftWallhit.normal;

                Vector3 forceToApply = transform.up * wallJumpUpForce + wallNormal * wallJumpSideForce;

                // reset y velocity and add force
                rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
                rb.AddForce(forceToApply, ForceMode.Impulse);

                //SoundEffect
                AudioManager.instance.playSoundEffect(1, 1f);
            }
        }
    }
}
