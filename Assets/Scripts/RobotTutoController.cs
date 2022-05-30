using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DialogueEditor;
using UnityEngine.InputSystem;
using TMPro;
public class RobotTutoController : MonoBehaviour
{
    [SerializeField] PlayerMovementAdvanced player;
    [SerializeField] GameObject grappin;
    [SerializeField] NPCConversation[] myConversation;
    [SerializeField] Input inputActions;
    ConversationManager conversationManager;
    [SerializeField] Transform[] checkPointRobotPoints;
    [SerializeField] ParticleSystem MovementExplosion;
    [SerializeField] Transform[] respawnPositions;
    [SerializeField] TextMeshProUGUI taskText;
    public Transform RespawnPosition(int position)
    {
        return respawnPositions[position];
    }
    int currentTutoId;
    bool haveMoved;

    
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
        currentTutoId = -1;

        
        //Inputs
        inputActions = new Input();

        inputActions.Dialogue.Press.performed += SelectOption;
        inputActions.Dialogue.Press.canceled -= SelectOption;
        inputActions.Dialogue.NextButton.performed += NextOption;
        inputActions.Dialogue.PreviousButton.performed += PreviousOption;

        inputActions.InGame.Jump.performed += CheckJump;


        haveMoved = false;

    }
    

    public void SelectOption(InputAction.CallbackContext callback)
    {
        if (callback.performed)
        {
            conversationManager.PressSelectedOption();
        }
    }
    public void NextOption(InputAction.CallbackContext callback)
    {
        if (callback.performed)
        {
            conversationManager.SelectNextOption();
        }
    }
    public void PreviousOption(InputAction.CallbackContext callback)
    {
        if (callback.performed)
        {
            conversationManager.SelectPreviousOption();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        grappin.SetActive(false);
        conversationManager = ConversationManager.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        LookAtPlayer();

        switch(currentTutoId)
        {
            case 0:
                taskText.text = "Press A/Space to jump";
                break;

            case 1:
                CheckDoubleJump();
                taskText.text = "Do a double jump";
                break;

            case 2:
                if(!conversationManager.IsConversationActive)
                {
                    taskText.text = "Follow Mr Robot";
                    haveMoved = false;
                    MoveNextCheckPoint(checkPointRobotPoints[1].position);
                    
                }
                break;

            case 3:
                if (!conversationManager.IsConversationActive)
                {
                    taskText.text = "RB/RightMouseButton to Grapple";
                    haveMoved = false;
                    grappin.SetActive(true);
                    MoveNextCheckPoint(checkPointRobotPoints[2].position);
                }
                break;

            case 4:
                if (!conversationManager.IsConversationActive)
                {
                    taskText.text = "Follow Mr Robot";
                    haveMoved = false;
                    MoveNextCheckPoint(checkPointRobotPoints[3].position);
                }
                break;

            case 5:
                if (!conversationManager.IsConversationActive)
                {
                    taskText.text = "Go to the FinishLine";
                    haveMoved = false;
                    MoveNextCheckPoint(checkPointRobotPoints[4].position);
                }
                break;


        }
        
        if (conversationManager.IsConversationActive)
        {
            DesactivePlayerMovement();
            taskText.gameObject.SetActive(false);
        }
        else
        {
            ActivePlayerMovement();
            taskText.gameObject.SetActive(true);
            //dont want player to move while in dialogue

        }
    }

    public void CheckJump(InputAction.CallbackContext callback)
    {
        if (callback.performed)
        {
            if (player.enabled && currentTutoId == 0)
            {
                LaunchTuto(1);
            }
        }
    }
    public void CheckDoubleJump()
    {
        if (player.enabled && player.hasDoubleJumped && currentTutoId == 1)
        {
           LaunchTuto(2);
        }
    }
    void LookAtPlayer()
    {
        transform.forward = (player.transform.position - transform.position);
    }

    void DesactivePlayerMovement()
    {
        player.enabled = false;
        player.GetRB().useGravity = true;
    }
    void ActivePlayerMovement()
    {
        player.enabled = true;
    }
    public void LaunchTuto(int checkPointId)
    {
        conversationManager.StartConversation(myConversation[checkPointId]);
        currentTutoId = checkPointId;
    }

    public void MoveNextCheckPoint(Vector3 position)
    {
        if(!haveMoved)
        {
            transform.position = position;
            //MovementExplosion.Play();
            //AudioManager.instance.playSoundEffect(17, 1f);
            haveMoved = true;
        }
    }
}