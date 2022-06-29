using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using FirstGearGames.SmoothCameraShaker;

public class PlayerMovementAdvanced : MonoBehaviour
{
    public static PlayerMovementAdvanced Instance;
    [Header("Movement")]
    private float moveSpeed;
    [SerializeField] float speedMax;
    private float desiredMoveSpeed;
    private float lastDesiredMoveSpeed;
    [SerializeField] float walkSpeed;
    [SerializeField] float acceleration;
    [SerializeField] float accelerationTimer;
    float accelerationTimeReset;

    private float resetWalkSpeed;
    [SerializeField] float sprintSpeed;
    [SerializeField] float slideSpeed;
    [SerializeField] float wallrunSpeed;

    [SerializeField] float speedIncreaseMultiplier;
    [SerializeField] float slopeIncreaseMultiplier;

    [SerializeField] float groundDrag;


    [Header("Jumping")]
    [SerializeField] float jumpForce;
    [SerializeField] float jumpForceDown;
    [SerializeField] float jumpCooldown;
    [SerializeField] float airMultiplier;
    bool readyToJump;
    bool canDoubleJump = true;
    float timeBeforeLand = 0.2f;

    [SerializeField] ShakeData jumpShake;
    [SerializeField] ShakeData runShake;
    public bool CanDoubleJump
    {
        get
        {
            return canDoubleJump;
        }
    }
    public void SetCanDoubleJump(bool a)
    {
        canDoubleJump = a;
    }

    [Header("Crouching")]
    [SerializeField] float crouchSpeed;
    [SerializeField] float crouchYScale;
    private float startYScale;

    [Header("Ground Check")]
    [SerializeField] float playerHeight;
    [SerializeField] LayerMask whatIsGround;
    bool grounded;
    [SerializeField] float timeToJump;
    private float resetTimeToJump;
    public bool canJump;

    [Header("Slope Handling")]
    [SerializeField] float maxSlopeAngle;
    private RaycastHit slopeHit;
    private bool exitingSlope;


    [SerializeField] Transform orientation;
    float horizontalInput;
    float verticalInput;

    public static Vector3 moveDirection;

    Rigidbody rb;
    public Rigidbody GetRB()
    {
        return rb;
    }
    private bool inputActivated;
    WallRunningAdvanced wallRunningAdvanced;
    bool exitingWall;
    float timeWallDoubleJump = 0.8f;
    float resetWallTimeDoubleJump = 0.8f;
    bool grappling;

    [SerializeField] PlayerCam playerCam;
    StatsPlayer stat;
    [SerializeField] Animator grappinAnimator;

    public void setGrapplin(bool g)
    {
        grappling = g;
    }
    public bool GetInputActivated
    { 
        get
        {
            return inputActivated;
        }
        set
        {
            inputActivated = value;
        }
    }
    public float VerticalInput
    {
        get
        {
            return verticalInput;
        }
    }

        [SerializeField] MovementState state;
    public enum MovementState
    {
        walking,
        sprinting,
        wallrunning,
        air
    }

    public bool sliding;
    public bool crouching;
    public bool wallrunning;

    bool jumpDown;
    public bool hasDoubleJumped;
    public float deltaTime;
    public float timeToPress;

    bool stateGroundOld;

    private void Awake()
    {
        Instance = this;
        hasDoubleJumped = false;
        inputActivated = false;
        stateGroundOld = true;
    }

    public void PlayerJumpDown(bool a)
    {
        jumpDown = a;
    }


    public void ActiveSlowTime(bool b)
    {
        GetComponent<CompetenceRalentie>().ActiveSlowTime(b);
    }

    public void Pause()
    {
        if (HudControllerInGame.Instance.StateMenu == ActualMenu.Pause)
        {


            if (Timer.Instance.GetTimer() != 0 && PlayerDeath.Instance.isDead == false && FinishLine.Instance.isWin == false)
            {


                if (Time.timeScale > 0)
                {
                    Timer.Instance.StopTimer();
                    playerCam.enabled = false;
                    Rumbler.instance.StopRumble();
                    Time.timeScale = 0;
                    AudioManager.instance.ChangeMixerSnapShot("Paused", 0.2f);
                    HudControllerInGame.Instance.OpenPauseMenu();
                    
                    if (InputManager.currentControlDevice == ControlDeviceType.KeyboardAndMouse)
                    {
                        Cursor.lockState = CursorLockMode.None;
                        Cursor.visible = true;
                    }
                }
                else
                {

                    Timer.Instance.LaunchTimer();
                    playerCam.enabled = true;
                    Time.timeScale = 1;
                    AudioManager.instance.ChangeMixerSnapShot("Normal", 0.2f);
                    HudControllerInGame.Instance.ClosePauseMenu();
                    Cursor.visible = false;
                    Cursor.lockState = CursorLockMode.Confined;
                }
            }
        }
        else
        {
            HudControllerInGame.Instance.Back();
        }
    }

    public void Pause(InputAction.CallbackContext callback)
    {
        if (HudControllerInGame.Instance.StateMenu == ActualMenu.Pause)
        {
            if (Timer.Instance.GetTimer() != 0 && PlayerDeath.Instance.isDead == false && FinishLine.Instance.isWin == false)
            {
                if (Time.timeScale > 0)
                {
                    Timer.Instance.StopTimer();
                    playerCam.enabled = false;
                    Rumbler.instance.StopRumble();
                    Time.timeScale = 0;
                    HudControllerInGame.Instance.OpenPauseMenu();
                    if (InputManager.currentControlDevice == ControlDeviceType.KeyboardAndMouse)
                    {
                        Cursor.lockState = CursorLockMode.None;
                        Cursor.visible = true;
                    }
                }
                else
                {
                    Timer.Instance.LaunchTimer();
                    playerCam.enabled = true;
                    Time.timeScale = 1;
                    HudControllerInGame.Instance.ClosePauseMenu();
                    Cursor.visible = false;
                    Cursor.lockState = CursorLockMode.Confined;
                }
            }
            else
            {
                HudControllerInGame.Instance.Back();
            }
        }
    }

    private void Start()
    {
        //statPlayer
        //stat = Data_Manager.Instance.GetData().statPlayer;

        wallRunningAdvanced = GetComponent<WallRunningAdvanced>();

        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        startYScale = transform.localScale.y;
        readyToJump = true;

        //initializing resets
        resetTimeToJump = timeToJump;
        resetWalkSpeed = walkSpeed;
        accelerationTimeReset = accelerationTimer;
        grappling = false;

        SetCanDoubleJump(false);
        
    }

    private void Update()
    {
        // ground check
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight, whatIsGround);
        if(!stateGroundOld && grounded)
        {
            AudioManager.instance.playSoundEffect(19, 1);
        }

        //time To Jump if not on ground;
        if (grounded)
        {
            stateGroundOld = grounded;
            timeToJump = resetTimeToJump;
            canJump = true;
            rb.useGravity = false;
        }
        else
        {
            stateGroundOld = grounded;
            rb.useGravity = true ;
            timeToJump -= Time.deltaTime;
            if(timeToJump <= 0)
            {
                canJump = false;
            }
        }

        MyInput();
       
        StateHandler();

        
    }

    private void FixedUpdate()
    {
        MovePlayer();
        SpeedControl();
        Accelerate();
        // handle drag
        if (grounded)
        {
            rb.drag = groundDrag;
        }
        else
            rb.drag = 0;

        if (horizontalInput != 0 || verticalInput != 0)
        {
            inputActivated = true;
        }

        //wall run doubleJump
        if (wallRunningAdvanced.GetExitingWall())
        {
            exitingWall = true;
        }
        if (exitingWall)
        {
            timeWallDoubleJump -= Time.deltaTime;
            if (timeWallDoubleJump <= 0)
            {
                canDoubleJump = true;
                exitingWall = false;
                timeWallDoubleJump = resetWallTimeDoubleJump;
            }
        }


        //DownForce if button stop press
        if (grounded || wallrunning || grappling)
        {
            jumpDown = false;
        }
        else if (jumpDown || timeToPress < 0 && !(HudControllerInGame.Instance.InMenu))
        {
            rb.AddForce(Vector3.down * jumpForceDown * 100 * Time.deltaTime);
        }
        else
        {
            timeToPress -= Time.deltaTime;
        }

        //limit velocity
        if(new Vector3(rb.velocity.x, 0, rb.velocity.z).magnitude > speedMax)
        {
            rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z).normalized * speedMax + Vector3.up * rb.velocity.y;
        }
    }

    //Acceleration+Momentum
    private void Accelerate()
    {
        if(new Vector2 (verticalInput, horizontalInput).magnitude >= 0.2f && verticalInput > -0.2f)
        {
            accelerationTimer -= Time.deltaTime;
            if(accelerationTimer<0 && walkSpeed< speedMax)
            {
                walkSpeed += acceleration*Time.deltaTime;

                accelerationTimer = accelerationTimeReset;
            }
            
        }
        if(new Vector2(verticalInput, horizontalInput).magnitude <= 0.2f && walkSpeed > resetWalkSpeed)
        {
            walkSpeed -= resetWalkSpeed/2*Time.deltaTime;
            accelerationTimer = accelerationTimeReset;
        }
    }
    private void MyInput()
    {
        horizontalInput =  InputManager.Instance.GetPlayerMovement().x;
        verticalInput = InputManager.Instance.GetPlayerMovement().y;

       
    }
    
    //get inputs

    public void GetPlayerJump()
    {
        // when to jump
        if (readyToJump && (grounded || canJump) && !wallrunning)
        {
            readyToJump = false;

            Jump();

            Invoke(nameof(ResetJump), jumpCooldown);

            canDoubleJump = true;
        }
        else if ( readyToJump && canDoubleJump && !wallrunning)
        {
            DoubleJump();
            canDoubleJump = false;
        }

        if (grounded)
        {
            canDoubleJump = true;
        }
    }

    private void StateHandler()
    {
        // Mode - Wallrunning
        if (wallrunning)
        {
            state = MovementState.wallrunning;
            desiredMoveSpeed = wallrunSpeed;
        }

        // Mode - Walking
        else if (grounded)
        {
            state = MovementState.walking;
            desiredMoveSpeed = walkSpeed;
        }

        // Mode - Air
        else
        {
            state = MovementState.air;
        }

        // check if desired move speed has changed drastically
        if (Mathf.Abs(desiredMoveSpeed - lastDesiredMoveSpeed) > 4f && moveSpeed != 0)
        {
            StopAllCoroutines();
            StartCoroutine(SmoothlyLerpMoveSpeed());
        }
        else
        {
            moveSpeed = desiredMoveSpeed;
        }

        lastDesiredMoveSpeed = desiredMoveSpeed;
    }

    private IEnumerator SmoothlyLerpMoveSpeed()
    {
        // smoothly lerp movementSpeed to desired value
        float time = 0;
        float difference = Mathf.Abs(desiredMoveSpeed - moveSpeed);
        float startValue = moveSpeed;

        while (time < difference)
        {
            moveSpeed = Mathf.Lerp(startValue, desiredMoveSpeed, time / difference);
            time += Time.deltaTime * speedIncreaseMultiplier;

            yield return null;
        }

        moveSpeed = desiredMoveSpeed;
    }

    private void MovePlayer()
    {
        // calculate movement direction
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;


        // on ground
        if (grounded)
        {
            rb.AddForce(moveDirection * moveSpeed * 10f, ForceMode.Force);
            if(rb.velocity.magnitude>10f)
            {
                //CameraShakerHandler.Shake(runShake);
            }

            //slow down player if no inputs
            if (moveDirection.magnitude == 0f)
            {
                rb.velocity = new Vector3(rb.velocity.x / 1.05f, rb.velocity.y, rb.velocity.z / 1.05f);
            }
        }
        // in air
        else
        {
            //slow down player if no inputs
            if(moveDirection.magnitude == 0f)
            {
                rb.velocity= new Vector3(rb.velocity.x / 1.05f, rb.velocity.y, rb.velocity.z/1.05f);
            }
            else
            {
                rb.AddForce(moveDirection * moveSpeed * 10f * airMultiplier, ForceMode.Force);
            }    
        }
    }

    private void SpeedControl()
    {
         Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

         // limit velocity if needed
         if (flatVel.magnitude > moveSpeed)
         {
            if (!grappling)
            {
                Vector3 limitedVel = flatVel.normalized * moveSpeed;
                rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
            }
            else
            {
                Vector3 limitedVel = flatVel.normalized * moveSpeed;
                rb.velocity = new Vector3(limitedVel.x*1.5f, rb.velocity.y, limitedVel.z*1.5f);
            }
         }
        
    }
    public void Jump()
    {
        //DownForceAfterInput
        timeToPress = 1f;


        // reset y velocity
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        //JumpForce
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        canJump = false;

        //feedBack
        AudioManager.instance.playSoundEffect(1, 1f);
        //CameraShakerHandler.Shake(jumpShake);


        if (HudControllerInGame.Instance.InMenu == false)
        {
            Rumbler.instance.RumbleConstant(2f, 2f, 0.15f);
            Rumbler.instance.RumbleConstant(2f, 2f, 0.15f);
        }
    }
    private void DoubleJump()
    {
        if(Physics.Raycast(transform.position, Vector3.down, playerHeight*1.3f, whatIsGround))
        {
            Jump();
        }
        else
        {
            //DownForceAfterInput
            timeToPress = 1f;


            hasDoubleJumped = true;
            // reset y velocity
            rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
            //JumpForce
            rb.AddForce(transform.up * jumpForce * 0.8f, ForceMode.Impulse);
            canJump = false;
            jumpDown = false;

            //feedBack
            AudioManager.instance.playSoundEffect(1, 1f);
            //CameraShakerHandler.Shake(jumpShake);
            Rumbler.instance.RumbleConstant(2f, 2f, 0.15f);
            Rumbler.instance.RumbleConstant(2f, 2f, 0.15f);
            if (!grappling)
            {
                grappinAnimator.SetTrigger("Flip");
            }
        }
    }
    private void ResetJump()
    {
        readyToJump = true;
    }
}
