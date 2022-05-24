using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BugTargetController : MonoBehaviour
{
    [SerializeField] public BugTargetFoot[] targetFeet;
    int actuelFootMove;
    bool moveFoot;
    int maxFoot;

    [SerializeField] Transform target;
    [SerializeField] float latence;
   
    [SerializeField] float speed;
    [SerializeField] float speedRotate;
    [SerializeField] float minTimeToChangeDirection;
    [SerializeField] float maxTimeToChangeDirection;

    Vector3 direction;
    bool changeDirection;


    GroupesBugs groupesBugsAssign;
    int indexTarget;
    int indexGroupesBug;

    [SerializeField] float speedStepUp;
    [SerializeField] Transform rayPositionStepUp;
    float tStepUp;
    float oldY;

    [SerializeField] float gravity;
    Rigidbody rb;
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

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        
        InitBug();
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

    void InitBug ()
    {
        tStepUp = 1;
        changeDirection = true;
        maxFoot = targetFeet.Length;
        actuelFootMove = -1;

        for (int i = 0; i< maxFoot; i++)
        {
            targetFeet[i].InitBug();
            targetFeet[i].SpeedBug = speed;
            
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
        if (changeDirection)
        {
            StopCoroutine(CouroutineChangeTime());
            direction = DirectionToTarget();
            changeDirection = false;
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
        changeDirection = true;
    }

    void MoveTargetsFeets ()
    {
        for(int i = 0; i< maxFoot;i++)
        {
            targetFeet[i].MoveTargetTerrin();
        }
    }

    void StartStep()
    {
        if (!moveFoot)
        {
            int nextFoot = CheckDist();
            if (nextFoot != -1)
            {
                actuelFootMove = nextFoot;
                targetFeet[actuelFootMove].InitMoveStep();
                moveFoot = true;
            }
        }
        else
        {
            moveFoot = targetFeet[actuelFootMove].MoveStep();
        }
    }

    int CheckDist()
    {
        float[] distFoot = new float[maxFoot];
        for (int i = 0; i < maxFoot; i++)
        {

            distFoot[i] = targetFeet[i].DistToOrigine();
        }

        float distRef = 0.4f;
        int actuelFootMove = -1;

        for (int i = 0; i < maxFoot; i++)
        {
            if (targetFeet[i].StepDistance/2 < distFoot[i] && distRef < targetFeet[i].StepDistance / 2)
            {
                distRef = distFoot[i];
                 actuelFootMove = i;
            }
        }
        return actuelFootMove;
    }

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



}
