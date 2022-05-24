using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BugTargetFoot
{
    [SerializeField] LayerMask groundLayer = default;
    [SerializeField] Transform targetToMove;
    [SerializeField] Transform originePoint;
    [SerializeField] Transform shoulder;
    [SerializeField] float speed = 1;
    [SerializeField] float stepDistance = 4;
    [SerializeField] float stepLength = 4;
    [SerializeField] float stepHeight = 1;

    float speedBug;

    Vector3 currentPoint;
    Vector3 nextPoint;
    Vector3 arcPoint;
    Vector3 oldPoint;

    float lerp;

    bool move;

    public float SpeedBug
    {
        set
        {
            speedBug = value;
        }
    }

    public float StepDistance
    {
        get
        {
           return stepDistance;
        }
    }


    public void InitBug()
    {
       
            currentPoint = targetToMove.position;
        
    }

    public void MoveTargetTerrin()
    {
    
        if (CheckIfOrigineIsGrounded()|| move)
        {
            targetToMove.position = currentPoint;
        }
        else
        {
            targetToMove.position = originePoint.position;
            currentPoint = originePoint.position;
        }
        
       
    }

    public void InitMoveStep()
    {
        move = true;
        nextPoint = CalculNextPoint2(currentPoint, stepDistance, stepHeight, stepLength, originePoint.position, groundLayer,shoulder);
       
        arcPoint = CalculPointArc(stepDistance, currentPoint, currentPoint, stepHeight, originePoint.transform.position);
        oldPoint = currentPoint;
        lerp = 0;
    }

    public bool MoveStep()
    {
        lerp += Time.deltaTime * (speed * speedBug);
        currentPoint = CalculPointStep(oldPoint, nextPoint, arcPoint, lerp);
        if(speedBug == 0)
        {
            speedBug = 1;
        }
       
        if (lerp >= 1 || IsGrounded(targetToMove, groundLayer,stepHeight) && lerp > 0.2)
        {
            move = false;
            return false;
        }
        else
        {
            return true;
        }
    }

    Vector3 CalculNextPoint2(Vector3 currentPosition, float stepDistance, float stepheight,float stepLenght, Vector3 originePos, LayerMask groundLayer, Transform shoulder)
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

        return originePoint.position;
    }

    Vector3 CalculPointArc(float stepDist, Vector3 currentPosition, Vector3 oldPoint, float stepHeight, Vector3 originePos)
    {
        Vector3 direction = new Vector3(originePos.x - currentPosition.x, currentPosition.y, originePos.z - currentPosition.z);
        return new Vector3(((stepDist / 2) * direction.x) + oldPoint.x, stepHeight + oldPoint.y, (((stepDist / 2) * direction.z) + oldPoint.z));
    }

    Vector3 CalculPointStep(Vector3 oldPoint, Vector3 nextPoint, Vector3 pointArc, float lerp)
    {
        Vector3 oldToArc = Vector3.Lerp(oldPoint, pointArc, lerp);
        Vector3 ArcToNext = Vector3.Lerp(pointArc, nextPoint, lerp);
        return Vector3.Lerp(oldToArc, ArcToNext, lerp);
    }

    public float DistToOrigine()
    {
        return Vector3.Distance(originePoint.position, currentPoint);
    }

    public float DistToShoulder(Transform shoulder, Vector3 currentPoint)
    {
        return Vector3.Distance(shoulder.position, currentPoint);
    }

    public bool IsGrounded(Transform target, LayerMask groundLayer, float stepHeight)
    {
        RaycastHit[] hits = Physics.RaycastAll(new Vector3(target.position.x, target.position.y + stepHeight, target.position.z), Vector3.down, 0.0001f, groundLayer);
        if (hits.Length >= 1)
        {
           
            return true;
        }

        return false;
    }

    bool CheckIfOrigineIsGrounded ()
    {
        RaycastHit[] hits = Physics.RaycastAll(new Vector3(originePoint.transform.position.x, originePoint.transform.position.y + stepHeight, originePoint.transform.position.z), Vector3.down,stepHeight+(stepHeight), groundLayer);
        if (hits.Length >= 1)
        {
            return true;
        }

        return false;
    }
}
