using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DialogueEditor;
using UnityEngine.InputSystem;
public class RobotTutoController : MonoBehaviour
{
    [SerializeField] PlayerMovementAdvanced player;
    [SerializeField] GameObject grappin;
    [SerializeField] NPCConversation[] myConversation;
    [SerializeField] Input inputActions;
    ConversationManager conversationManager;
    [SerializeField] Transform[] checkPointRobotPoints;
    int currentTutoId;
    

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

        conversationManager = ConversationManager.Instance;
        //Inputs
        inputActions = new Input();

        inputActions.Dialogue.Press.performed += SelectOption;
        inputActions.Dialogue.Press.canceled -= SelectOption;
        inputActions.Dialogue.NextButton.performed += NextOption;
        inputActions.Dialogue.PreviousButton.performed += PreviousOption;

        inputActions.InGame.Jump.performed += CheckJump;

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
    }

    // Update is called once per frame
    void Update()
    {
        LookAtPlayer();

        switch(currentTutoId)
        {
            case 1:
                CheckDoubleJump();
                return;
            case 2:
                MoveNextCheckPoint(checkPointRobotPoints[1].position);
                return;
            case 3:
                grappin.SetActive(true);
                MoveNextCheckPoint(checkPointRobotPoints[2].position);
                return;
            case 4:
                MoveNextCheckPoint(checkPointRobotPoints[3].position);
                return;

        }
        
        /*if (conversationManager.IsConversationActive)
        {
            DesactivePlayerMovement();
        }
        else
        {
            ActivePlayerMovement();
            //dont want player to move while in dialogue
            Debug.Log(conversationManager.IsConversationActive);
        }*/
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
        transform.position = position;
    }
}