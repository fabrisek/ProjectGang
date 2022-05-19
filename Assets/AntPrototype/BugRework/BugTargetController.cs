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
    public Transform Target
    {
        set
        {
            target = value;
        }
    }

    private void Start()
    {
        InitBug();
    }

    private void Update()
    {
        Movebug();

        MoveTargetsFeets();
        StartStep();
    }

    void InitBug ()
    {
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

        float distRef = 0.5f;
        int actuelFootMove = -1;

        for (int i = 0; i < maxFoot; i++)
        {
            if (distRef < distFoot[i])
            {
                distRef = distFoot[i];
                actuelFootMove = i;
            }
        }
        return actuelFootMove;
    }



}
