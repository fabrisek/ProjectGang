using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DialogueEditor;
using UnityEngine.InputSystem;
public class RobotTutoController : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] NPCConversation[] myConversation;
    [SerializeField] Input inputActions;

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

        inputActions.Dialogue.Press.performed += SelectOption;
        inputActions.Dialogue.Press.canceled -= SelectOption;
        inputActions.Dialogue.NextButton.performed += NextOption;


    }

    public void SelectOption(InputAction.CallbackContext callback)
    {
        if(callback.performed)
        {
            ConversationManager.Instance.PressSelectedOption();
        }
    }
    public void NextOption(InputAction.CallbackContext callback)
    {
        if (callback.performed)
        {

        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        LookAtPlayer();
    }
    void LookAtPlayer()
    {
        transform.forward = (player.transform.position - transform.position);
    }
    public void LaunchTuto(int checkPointId)
    {
        ConversationManager.Instance.StartConversation(myConversation[checkPointId]);
    }
}
