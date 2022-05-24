using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FovCamera : MonoBehaviour
{
    [SerializeField] float joystickValue;
    Animator camAnimator;
    public PlayerMovementAdvanced playerMovementAdvanced;

    // Start is called before the first frame update
    void Start()
    {
        camAnimator = GetComponent<Animator>();
        camAnimator.gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if( playerMovementAdvanced.VerticalInput > joystickValue)
        {
            camAnimator.SetBool("fovUp", true);
        }
        else
        {
            camAnimator.SetBool("fovUp", false);
        }
    }
}
