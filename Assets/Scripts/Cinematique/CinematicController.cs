using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CinematicController : MonoBehaviour
{
    public static CinematicController Instance;
    [SerializeField] GameObject brainCamera;
    [SerializeField] List<Transform> pointOfIntrest;

    [SerializeField] CinemachineSmoothPath path;
    [SerializeField] CinemachineDollyCart cart;
    [SerializeField] float _speedCart;

    [SerializeField] GameObject cam;
    [SerializeField] float speedRotate;
    Transform target;



    bool atEnd;

    bool startCine;

    public void SetStartCinematci ()
    {
        brainCamera.SetActive(true);
        startCine = true;
        atEnd = false;
        brainCamera.SetActive(true);
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
        
        cart.m_Position = 0;
        if (pointOfIntrest[0] != null)
        {
            Quaternion rotationRef = Quaternion.LookRotation(new Vector3(pointOfIntrest[0].position.x - cam.transform.position.x, pointOfIntrest[0].position.y - cam.transform.position.y, pointOfIntrest[0].position.z - cam.transform.position.z).normalized);
            cam.transform.rotation = Quaternion.RotateTowards(cam.transform.rotation, rotationRef, 100000);
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
            cart.m_Position += Time.deltaTime * _speedCart;
        }
        else if(!atEnd )
        {
            atEnd = true;
            startCine = false;
            if(LevelManager.Instance != null)
            {

                FinishCinematic();

            }
        }
    }

    public void FinishCinematic ()
    {
        if (TransitionScript.Instance != null)
        {
            // StopCoroutine(TransitionScript.Instance.CoroutineFadeV2());
            TransitionScript.Instance.Fade(2);
            Debug.Log("Salut");
        }
        atEnd = true;
        startCine = false;
        LevelManager.Instance.enableCam();
        brainCamera.SetActive(false);
        cam.SetActive(false);
        LevelManager.Instance.InitLevelManager();
    }

    int CheckTheIntrestToLook ()
    {
        float dist = Vector3.Distance(cam.transform.position, pointOfIntrest[0].position);
        int t = 0;
        for(int i =0; i< pointOfIntrest.Count;i++)
        {
            if(Vector3.Distance(cam.transform.position, pointOfIntrest[i].position) < dist)
            {
                dist = Vector3.Distance(cam.transform.position, pointOfIntrest[i].position);
                t = i;
            }
        }
        return t;
        
    }

    void SetTargetToCamera()
    {
        int pointToLook = CheckTheIntrestToLook();

        target = pointOfIntrest[pointToLook];
    }

    void RotateToTarget()
    {
        Quaternion rotationRef = Quaternion.LookRotation(new Vector3(target.position.x - cam.transform.position.x, target.position.y - cam.transform.position.y, target.position.z - cam.transform.position.z).normalized);
        cam.transform.rotation = Quaternion.RotateTowards(cam.transform.rotation, rotationRef, speedRotate * Time.deltaTime);
    }
}
