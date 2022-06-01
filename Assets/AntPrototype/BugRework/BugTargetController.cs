using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BugTargetController : MonoBehaviour
{
    [SerializeField] public BugTargetFoot[] targetFeet1;
    [SerializeField] public BugTargetFoot[] targetFeet2;
    int actuelFootMove1;
    
    bool moveFoot1;
    int maxFoot1;
    int actuelFootMove2;
   
    bool moveFoot2;
    int maxFoot2;

    [SerializeField] Transform target;
    [SerializeField] float latence;
   
    [SerializeField] float speed;
    [SerializeField] float speedRotate;
    [SerializeField] float minTimeToChangeDirection;
    [SerializeField] float maxTimeToChangeDirection;

    Vector3 direction;
    bool changeDirection1;
   


    GroupesBugs groupesBugsAssign;
    int indexTarget;
    int indexGroupesBug;

    [SerializeField] float speedStepUp;
    [SerializeField] Transform rayPositionStepUp;
    float tStepUp;
    float oldY;

    public float gravity;
    Rigidbody rb;

    [SerializeField] GameObject viewBug;
    [SerializeField] LayerMask playerLayer;
    public Transform Target
    {
        set
        {
            target = value;
        }
    }

    public GroupesBugs GroupesBugsAssign
    {
        set
        {
            groupesBugsAssign = value;
        }
    }

    public int IndexTarget
    {
        set
        {
            indexTarget = value;
        }
    }

    public int IndexGroupesBug
    {
        set
        {
            indexGroupesBug = value;
        }
    }

    private void Awake()
    {
        InitBug();
    }
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        Movebug();
        StepUpBug();

        MoveTargetsFeets(); 
       
        StartStep();

        if(rb.useGravity)
        {
            rb.AddForce(Vector3.down * gravity);
        }
    }

    public void InitBug ()
    {
        tStepUp = 1;
        changeDirection1 = true;
        maxFoot1 = targetFeet1.Length;
        actuelFootMove1 = -1;
        actuelFootMove2 = -1;

        for (int i = 0; i< targetFeet1.Length; i++)
        {
            targetFeet1[i].InitBug();
            targetFeet1[i].SpeedBug = speed;
        }

        for (int i = 0; i < targetFeet2.Length; i++)
        {
            targetFeet2[i].InitBug();
            targetFeet2[i].SpeedBug = speed;
        }
    }

    private void Movebug()
    {
        if (target != null)
        {
            if (Vector3.Distance(new Vector3(target.position.x, 0, target.position.z), new Vector3(transform.position.x, 0, transform.position.z)) > latence)
            {
                MoveToTarget();
                RotateToTarget();
            }
            else
            {
                if(groupesBugsAssign != null)
                {
                    
                    groupesBugsAssign.ChangeTarget(indexTarget, indexGroupesBug);
                }
            }
        }
        
    }

    void RotateToTarget()
    {
        Quaternion rotationRef = Quaternion.LookRotation(new Vector3(target.position.x - transform.position.x, 0, target.position.z - transform.position.z).normalized);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotationRef, speedRotate * Time.deltaTime);
    }

    void MoveToTarget()
    {
        if (changeDirection1)
        {
            StopCoroutine(CouroutineChangeTime());
            direction = DirectionToTarget();
            changeDirection1 = false;
            StartCoroutine(CouroutineChangeTime());
        }
       
        transform.Translate(direction * speed * Time.deltaTime);
    }

    Vector3 DirectionToTarget()
    {
        float X;
        X = Random.Range(-0.7f, 0.7f);
        return new Vector3(X, 0, 1).normalized;
    }

    IEnumerator CouroutineChangeTime()
    {
        float time = Random.Range(minTimeToChangeDirection, maxTimeToChangeDirection);
        yield return new WaitForSeconds(time);
        changeDirection1 = true;
    }

    void MoveTargetsFeets ()
    {
        for(int i = 0; i< targetFeet1.Length; i++)
        {
            targetFeet1[i].MoveTargetTerrin();
        }

        for (int i = 0; i < targetFeet2.Length; i++)
        {
            targetFeet2[i].MoveTargetTerrin();
        }
    }

    void StartStep()
    {
        if (!moveFoot1)
        {
            int nextFoot = CheckDist(targetFeet1);
            if (nextFoot != -1)
            {
                
                actuelFootMove1 = nextFoot;
                targetFeet1[actuelFootMove1].InitMoveStep();
                targetFeet1 = ChangePriorityFeet(actuelFootMove1, targetFeet1);
                moveFoot1 = true;
            }
            else
            {
               
            }
        }
        else
        {
            moveFoot1 = targetFeet1[actuelFootMove1].MoveStep();
        }

        if (!moveFoot2)
        {
            int nextFoot = CheckDist(targetFeet2);
            if (nextFoot != -1)
            {
              
                actuelFootMove2 = nextFoot;
                targetFeet2[actuelFootMove2].InitMoveStep();
                targetFeet2 = ChangePriorityFeet(actuelFootMove2, targetFeet2);
                moveFoot2 = true;
            }
            else
            {
               
            }
        }
        else
        {
            moveFoot2 = targetFeet2[actuelFootMove2].MoveStep();
        }
    }

    int CheckDist(BugTargetFoot[] targetFeet)
    {
        float[] distFoot = new float[targetFeet.Length];
        for (int i = 0; i < targetFeet.Length; i++)
        {

            distFoot[i] = targetFeet[i].DistToOrigine();
        }

        float distRef = 0.4f;
        int actuelFootMove = -1;

        for (int i = 0; i < targetFeet.Length; i++)
        {
            if ((targetFeet[i].StepDistance +0.1f < distFoot[i] && distRef < targetFeet[i].StepDistance + 0.1f))
            {
               
                    distRef = distFoot[i];
                    actuelFootMove = i;
                
                
            }
        }
        return actuelFootMove;
    }


    BugTargetFoot[] ChangePriorityFeet (int index, BugTargetFoot[] targetFeet)
    {
        BugTargetFoot[] targetFeetTemp = targetFeet;
        targetFeet = new BugTargetFoot[targetFeetTemp.Length];
        int t = 0;
        for(int i = 0;i< targetFeetTemp.Length;i++)
        {
            if(i != index)
            {
                targetFeet[t] = targetFeetTemp[i];
                t++;
            }

        }

        targetFeet[targetFeet.Length - 1] = targetFeetTemp[index];

        return targetFeet;
    }

    
    



    





  

    void SetSpeedFeet (float speed)
    {
        for (int i = 0; i < targetFeet1.Length; i++)
        {
            targetFeet1[i].SpeedBug = speed;
        }

        for (int i = 0; i < targetFeet2.Length; i++)
        {
            targetFeet1[i].SpeedBug = speed;
        }
    }

    // ========== Pour Ce lever ==========

    void StepUpBug ()
    {
        if(CheckIfStepUp() && tStepUp >= 1)
        {
            InitStepUp();
        }
        if(tStepUp <1)
        {
            transform.position = new Vector3(transform.position.x, CalculNewY(),transform.position.z);
        }
    }

    void InitStepUp()
    {
        oldY = transform.position.y;
        tStepUp = 0;
    }

    float CalculNewY ()
    {
        tStepUp += Time.deltaTime * speedStepUp;
        float newY = Mathf.Lerp(oldY, oldY + 1.2f,tStepUp);
       
        return newY;
    }

    bool CheckIfStepUp()
    {
        
        RaycastHit[] hits = Physics.RaycastAll(rayPositionStepUp.position, Vector3.forward, 2);
        if (hits.Length >= 1)
        {
            for(int i =0;i<hits.Length;i++)
            {
                if(hits[i].transform.gameObject.layer == LayerMask.NameToLayer("Water"))
                {
                    
                    return true;
                }
            }
        }

        return false;
    }




    public void DestroyMe ()
    {
        Destroy(gameObject);
    }


    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.transform.name + " " + other.transform.gameObject.layer+ " "+ (other.gameObject.layer == playerLayer));
        Debug.Log(other.transform.gameObject.layer);
        if (other.gameObject.layer ==  7)
        {
            Debug.Log(other.transform.name + " " + "On me voie");
            viewBug.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {

        if (other.gameObject.layer == playerLayer)
        {
            viewBug.SetActive(false);
        }

    }

}
