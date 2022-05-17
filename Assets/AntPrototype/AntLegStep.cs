using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntLegStep : MonoBehaviour
{
    [SerializeField] LayerMask groundLayer = default;
    [SerializeField] float speed = 1;
    [SerializeField] float stepDistance = 4;
    [SerializeField] float stepLength = 4;
    [SerializeField] float stepHeight = 1;
    [SerializeField] GameObject originePoint;

    [SerializeField] ScriptLef scriptLef;
    
    float speedAnt;

    Vector3 currentPoint;

    Vector3 nextPoint;
    Vector3 arcPoint;
    Vector3 oldPoint;

    float lerp;

    bool move;
    bool tombe;

    public bool Move
    {
        set
        {
            move = value;
        }
        get
        {
            return move;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        tombe = true;
        speedAnt = scriptLef.Speed;
        lerp = 1;
        currentPoint = transform.position;
       
    }

    // Update is called once per frame
    void Update()
    {
        if (!tombe)
        {

            transform.position = currentPoint;
            
        }
        else
        {
            
            if (IsGrounded())
            {
                Debug.Log("Je suis au sol mtn");
                    tombe = false;
                lerp = 1;
                currentPoint = transform.position;
            }
        }

        if (DistToOrigine() > stepDistance && !move)
        {
            Debug.Log("yooooooo" + gameObject.name);
            move = true;
            lerp = 1;
        }

        if (move)
        {
            if(lerp >=1)
            {
                InitMoveStep();
            }
            else
            {
                move = MoveStep();
            }
        }
        


        
    }

    void InitMoveStep()
    {
        nextPoint = CalculNextPoint2(currentPoint, stepDistance, stepHeight, originePoint.transform.position);
        if (nextPoint != Vector3.zero)
        {
            lerp = 0;

            arcPoint = CalculPointArc(stepDistance, currentPoint, currentPoint, stepHeight, originePoint.transform.position);
            oldPoint = currentPoint;
        }
        else
        {
            lerp = 1;
            move = false;
            
        }
    }

    bool MoveStep ()
    {
        
        currentPoint = CalculPointStep(oldPoint, nextPoint, arcPoint, lerp);
        lerp += Time.deltaTime * (speed * speedAnt);
        if(lerp >=1 || IsGrounded() && lerp >0.2)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    Vector3 CalculNextPoint2(Vector3 currentPosition, float stepDistance,float stepheight, Vector3 originePos)
    {
        Vector3 direction = new Vector3(originePos.x - currentPosition.x, currentPosition.y, originePos.z - currentPosition.z);
        Vector3 nextPoint = new Vector3(currentPosition.x + (direction.x * stepDistance), currentPosition.y, currentPosition.z + (direction.z * stepDistance));
        Vector3 checkNextPoint = new Vector3(nextPoint.x, nextPoint.y + (stepheight - 0.1f), nextPoint.z);
        RaycastHit[] hits = Physics.RaycastAll(checkNextPoint, Vector3.down, stepheight, groundLayer);
        if (hits.Length >= 1)
        {
            return hits[0].point;
        }
        hits = Physics.RaycastAll(nextPoint, Vector3.down, stepheight, groundLayer);
        if (hits.Length >= 1)
        {
            return hits[0].point;
        }

        return Vector3.zero;
    }

    Vector3 CalculPointArc(float stepDist, Vector3 currentPosition, Vector3 oldPoint, float stepHeight,Vector3 originePos)
    {
        Vector3 direction = new Vector3(originePos.x - currentPosition.x, currentPosition.y, originePos.z - currentPosition.z);
        return new Vector3(((stepDist / 2 )*direction.x) + oldPoint.x, stepHeight + oldPoint.y, (((stepDist / 2)*direction.z )+ oldPoint.z));
    }

    Vector3 CalculPointStep(Vector3 oldPoint, Vector3 nextPoint, Vector3 pointArc, float lerp)
    {
        Vector3 oldToArc = Vector3.Lerp(oldPoint, pointArc, lerp);
        Vector3 ArcToNext = Vector3.Lerp(pointArc, nextPoint, lerp);
        return Vector3.Lerp(oldToArc, ArcToNext, lerp);

    }

    public float DistToOrigine ()
    {
        return Vector3.Distance(originePoint.transform.position, transform.position);
    }

    public bool IsGrounded ()
    {
       RaycastHit[] hits = Physics.RaycastAll(new Vector3(transform.position.x, transform.position.y + stepHeight, transform.position.z), Vector3.down,stepHeight +0.1f, groundLayer);
       if(hits.Length >=1)
        {
            return true;
        }
       
        return false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawSphere(CalculNextPoint2(currentPoint, stepDistance, stepHeight, originePoint.transform.position), 0.1f);
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(CalculPointArc(stepDistance, currentPoint, currentPoint, stepHeight, originePoint.transform.position), 0.1f);

    }
}
