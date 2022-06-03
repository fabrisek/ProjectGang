using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CinematicController : MonoBehaviour
{
    public static CinematicController Instance;
    [SerializeField] CinemachineVirtualCamera virtualCameraOnRail;
    [SerializeField] List<Transform> pointOfIntrest;

    [SerializeField] CinemachineSmoothPath path;
    [SerializeField] CinemachineDollyCart cart;
    [SerializeField] float speedCart;

    [SerializeField] GameObject camera;
    [SerializeField] float speedRotate;
    Transform target;



    bool atEnd;

    bool startCine;

    public void SetStartCinematci ()
    {
        startCine = true;
        atEnd = false;
        InitCine();

    }

    public bool AtEnd
    {
        get
        {
            return atEnd;
        }
    }

    public bool StartCine
    {
        get
        {
            return startCine;
        }
    }
    // Start is called before the first frame update

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);    // Suppression d'une instance précédente (sécurité...sécurité...)
        Instance = this;
        InitCine();
    }
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        if (startCine)
        {
            StartCinematique();
        }


    }

    void InitCine ()
    {
        camera.SetActive(true);
        cart.m_Position = 0;
        if (pointOfIntrest[0] != null)
        {
            Quaternion rotationRef = Quaternion.LookRotation(new Vector3(pointOfIntrest[0].position.x - camera.transform.position.x, pointOfIntrest[0].position.y - camera.transform.position.y, pointOfIntrest[0].position.z - camera.transform.position.z).normalized);
            camera.transform.rotation = Quaternion.RotateTowards(camera.transform.rotation, rotationRef, 100000);
        }
        else
        {
            Debug.Log("Il n y a pas de point d intret mex");
        }
    }

    void StartCinematique ()
    {
        if (!atEnd)
        {

            SetTargetToCamera();
            MoveCart();
            if (target != null)
            {
                RotateToTarget();
            }
        }
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
            startCine = false;
            if(LevelManager.Instance != null)
            {
                camera.SetActive(false);
                LevelManager.Instance.InitLevelManager();
               

            }
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
                t = i;
            }
        }
      //  Debug.Log(t);
        return t;
        
    }

    void SetTargetToCamera()
    {
        int pointToLook = CheckTheIntrestToLook();
        
        /*if (pointToLook != -1)
        {
            virtualCameraOnRail.LookAt = pointOfIntrest[pointToLook];
                }*/

        target = pointOfIntrest[pointToLook];
    }

    void RotateToTarget()
    {
        Quaternion rotationRef = Quaternion.LookRotation(new Vector3(target.position.x - camera.transform.position.x, target.position.y - camera.transform.position.y, target.position.z - camera.transform.position.z).normalized);
        camera.transform.rotation = Quaternion.RotateTowards(camera.transform.rotation, rotationRef, speedRotate * Time.deltaTime);
    }
}
