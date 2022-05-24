using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementAdvanced : MonoBehaviour
{
    [Header("Movement")]
    private float moveSpeed;
    private float desiredMoveSpeed;
    private float lastDesiredMoveSpeed;
    [SerializeField] float walkSpeed;
    [SerializeField] float acceleration;
    private float resetWalkSpeed;
    [SerializeField] float sprintSpeed;
    [SerializeField] float slideSpeed;
    [SerializeField] float wallrunSpeed;

    [SerializeField] float speedIncreaseMultiplier;
    [SerializeField] float slopeIncreaseMultiplier;

    [SerializeField] float groundDrag;

    [Header("Jumping")]
    [SerializeField] float jumpForce;
    [SerializeField] float jumpCooldown;
    [SerializeField] float airMultiplier;
    bool readyToJump;

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

    [SerializeField] GlitchEffect glitch;
    [SerializeField] Transform orientation;
    float horizontalInput;
    float verticalInput;

    public static Vector3 moveDirection;

    Rigidbody rb;

    private bool inputActivated;
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

    public TextMeshProUGUI text_speed;
    public TextMeshProUGUI text_mode;
    private Input inputActions;

    private void Awake()
    {
        inputActivated = false;
        inputActions = new Input();
        inputActions.InGame.SlowTime.performed += ActiveSlowTime;
        inputActions.InGame.SlowTime.canceled += ActiveSlowTime;
    }

    private void ActiveSlowTime(InputAction.CallbackContext callback)
    {
        if (callback.performed)
        {
            Time.timeScale = 0.6f;
            AudioManager.instance.ChangePitch(.95f);
            glitch.enabled = true;
        }

        if (callback.canceled)
        {
            Time.timeScale = 1f;
            glitch.enabled = false;
            AudioManager.instance.ChangePitch(1f);
        }
    }

    private void OnEnable()
    {
        inputActions.Enable();
    }
    private void OnDisable()
    {
        inputActions.Disable();
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        startYScale = transform.localScale.y;
        readyToJump = true;

        //initializing resets
        resetTimeToJump = timeToJump;
        resetWalkSpeed = walkSpeed;
    }

    private void Update()
    {
        // ground check
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);

        //time To Jump if not on ground;
        if (grounded)
        {
            timeToJump = resetTimeToJump;
            canJump = true;
        }
        else
        {
            timeToJump -= Time.deltaTime;
            if(timeToJump <= 0)
            {
                canJump = false;
            }
        }
        

        MyInput();
        SpeedControl();
        StateHandler();
        Accelerate();

        // handle drag
        if (grounded)
            rb.drag = groundDrag;
        else
            rb.drag = 0;

        if(horizontalInput != 0||verticalInput!=0)
        {
            inputActivated = true;
        }
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void Accelerate()
    {
        if(verticalInput >= 0.5f)
        {
            walkSpeed *= acceleration;
        }
        if(verticalInput <= 0.1f)
        {
            walkSpeed = resetWalkSpeed;
        }
    }
    private void MyInput()
    {
        horizontalInput = GetPlayerMovement().x;
        verticalInput = GetPlayerMovement().y;

        // when to jump
        if (GetPlayerJump() > 0.1f && readyToJump && (grounded || canJump))
        {
            readyToJump = false;

            Jump();

            Invoke(nameof(ResetJump), jumpCooldown);
        }
    }
    
    //get inputs
    public Vector2 GetPlayerMovement()
    {
        return inputActions.InGame.Move.ReadValue<Vector2>();
    }
    public float GetPlayerJump()
    {
        return inputActions.InGame.Jump.ReadValue<float>();
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

            print("Lerp Started!");
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
             Vector3 limitedVel = flatVel.normalized * moveSpeed;
             rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
         }
        
    }

    private void Jump()
    {
        // reset y velocity
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        //JumpForce
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        canJump = false;

        //SoundEffect
        AudioManager.instance.playSoundEffect(1);
    }
    private void ResetJump()
    {
        readyToJump = true;
    }
}
