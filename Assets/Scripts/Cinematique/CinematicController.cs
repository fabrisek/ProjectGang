using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CinematicController : MonoBehaviour
{
    
    [SerializeField] CinemachineVirtualCamera virtualCameraOnRail;
    [SerializeField] List<Transform> pointOfIntrest;

    [SerializeField] CinemachineSmoothPath path;
    [SerializeField] CinemachineDollyCart cart;
    [SerializeField] float speedCart;

   
    bool atEnd;
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        if (!atEnd)
        {
           
            SetTargetToCamera();
        }
       
            MoveCart();
        
    }

    void MoveCart ()
    {
        if (path.PathLength > cart.m_Position)
        {
            cart.m_Position += Time.deltaTime * speedCart;
        }
        else if(!atEnd )
        {
            atEnd = true;
        }
    }

    int CheckTheIntrestToLook ()
    {
        float dist = Vector3.Distance(virtualCameraOnRail.transform.position, pointOfIntrest[0].position);
        int t = 0;
        for(int i =0; i< pointOfIntrest.Count;i++)
        {
            if(Vector3.Distance(virtualCameraOnRail.transform.position, pointOfIntrest[i].position) < dist)
            {
                dist = Vector3.Distance(virtualCameraOnRail.transform.position, pointOfIntrest[i].position);
            }
        }
        return t;
        
    }

    void SetTargetToCamera()
    {
        int pointToLook = CheckTheIntrestToLook();
        
        if (pointToLook != -1)
        {
            virtualCameraOnRail.LookAt = pointOfIntrest[pointToLook];
                } 
    }
}
