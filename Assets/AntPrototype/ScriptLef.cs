using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptLef : MonoBehaviour
{
    [SerializeField] AntLegStep[] foots;
    int actuelFootMove;
    int maxFoot;
    [SerializeField] Transform target;
    [SerializeField] Vector3 direction;
    [SerializeField] float speed;
    [SerializeField] float speedRotate;
    [SerializeField] float minTimeToChangeDirection;
    [SerializeField] float maxTimeToChangeDirection;


    bool changeDirection;
    public Transform Target
    {
        set
        {
            target = value;
        }
    }

    public float Speed
    {
        get
        {
            return speed;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        actuelFootMove = 0;
        maxFoot = foots.Length;
        changeDirection = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (target != null)
        {
            if (Vector3.Distance(new Vector3(target.position.x, 0, target.position.z), new Vector3(transform.position.x, 0, transform.position.z)) > 3)
            {
                
                MoveToTarget();
                RotateToTarget();
            }
        }
      /* if (Input.GetKey(KeyCode.Z))
        {
            direction += Vector3.forward;
        }
        if (Input.GetKey(KeyCode.S))
        {
            direction += Vector3.back;
        }
        if (Input.GetKey(KeyCode.D))
        {
            direction += Vector3.right;
        }
        if (Input.GetKey(KeyCode.Q))
        {
            direction += Vector3.left;
        }

        transform.Translate(direction.normalized * speed * Time.deltaTime);
        direction = Vector3.zero;*/
       StartStep();


    }

    void RotateToTarget ()
    {
        Quaternion rotationRef = Quaternion.LookRotation(new Vector3(target.position.x - transform.position.x, 0, target.position.z - transform.position.z).normalized);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotationRef, speedRotate * Time.deltaTime);
    }

    void MoveToTarget ()
    {
        if (changeDirection)
        {
            StopCoroutine(CouroutineChangeTime());
            direction = DirectionToTarget();
            changeDirection = false;
            StartCoroutine(CouroutineChangeTime());
        }
       // gameObject.GetComponent<Rigidbody>().velocity = new Vector3(GetComponent<Rigidbody>().velocity.x + (direction.x * speed*Time.deltaTime), GetComponent<Rigidbody>().velocity.y, GetComponent<Rigidbody>().velocity.z + (direction.z * speed* Time.deltaTime));
        transform.Translate(direction * speed * Time.deltaTime);
    }

    Vector3 DirectionToTarget ()
    {
        float X;
        X = Random.Range(-0.7f, 0.7f);

        /* if (target != null)
         {
             return new Vector3(target.position.x - transform.position.x, 0, target.position.z - transform.position.z).normalized;
         }
         else
         {
             return Vector3.zero;
         }*/
        return new Vector3(X, 0, 1).normalized ;
    }

    void StartStep ()
    {
        
       
            if (!foots[actuelFootMove].Move)
            {
            int nextFoot = CheckDist();
            if (nextFoot != -1)
                {
                actuelFootMove = nextFoot;
                foots[actuelFootMove].Move = true;
                }
              
            }
        
           
        
    }

    int CheckDist ()
    {
        float[] distFoot = new float[maxFoot];
        for(int i = 0;i<maxFoot;i++)
        {

            distFoot[i] = foots[i].DistToOrigine();
        }

        float distRef = 0.5f;
        int actuelFootMove = -1;

        for (int i = 0; i < maxFoot; i++)
        {
           if(distRef < distFoot[i])
            {
                distRef = distFoot[i];
                actuelFootMove = i;
            }
        }
        return actuelFootMove;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, new Vector3((transform.position.x * direction.x)*20, transform.position.y,( transform.position.z * direction.z)*20));
    }

    IEnumerator CouroutineChangeTime ()
    {
        float time = Random.Range(minTimeToChangeDirection, maxTimeToChangeDirection);
        yield return new WaitForSeconds(time);
        changeDirection = true;
    }
}
