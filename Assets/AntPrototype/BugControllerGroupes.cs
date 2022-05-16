using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BugControllerGroupes : MonoBehaviour
{
    [SerializeField] List<ScriptLef> bugs;
    [SerializeField] Transform target;
    bool send;

    public bool Send
    {
        get
        {
            return send;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetTargetToBugs ()
    {
        send = true;
        if (target != null)
        {
            for (int i = 0; i < bugs.Count; i++)
            {
                bugs[i].Target = target;
            }
        }
        else
        {
            Debug.Log("Pas de target");
        }
    }
}
