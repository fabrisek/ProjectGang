using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchAnim : MonoBehaviour
{
    Animator animator;
    [SerializeField] string launchAnimTriggerName;
    [SerializeField] float distanceToLaunch;
    [SerializeField] Transform position;
    GameObject player;
    bool animLaunched;

    // Start is called before the first frame update
    void Start()
    {
        player = GameManager.gameManager.GetCamera();
        animator = GetComponent<Animator>();
        animLaunched = false;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(animLaunched);
        if (Mathf.Abs((player.transform.position - position.position).magnitude) < distanceToLaunch && !animLaunched)
        {
            LaunchAnimation();
            animLaunched = true;
        }
    }

    public void LaunchAnimation()
    {
        animator.SetTrigger(launchAnimTriggerName);
    }
}
