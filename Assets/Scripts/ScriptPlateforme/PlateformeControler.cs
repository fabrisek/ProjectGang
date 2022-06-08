using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace ByPass
{
    public class PlateformeControler : MonoBehaviour
    {
        [SerializeField] NavMeshAgent navAgent;

        [SerializeField] Transform target;
        [SerializeField] float latence;

        [SerializeField] float speed;
        [SerializeField] float speedRotate;
        [SerializeField] float minTimeToChangeDirection;
        [SerializeField] float maxTimeToChangeDirection;

        Vector3 direction;
        bool changeDirection1;

        GroupesPlateforme groupePlateformeAssign;
        int indexTarget;
        int indexGroupesBug;

        public GroupesPlateforme GroupePlateformeAssign
        {
            set
            {
                groupePlateformeAssign = value;
            }
        }

        public Transform Target
        {
            set
            {
                target = value;
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
        // Start is called before the first frame update
        void Start()
        {
            
              changeDirection1 = true;
        }

        // Update is called once per frame
        void Update()
        {
            if(target != null)
            {
                navAgent.SetDestination(target.position);
            }
            
            
             Movebug();
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
                    if (groupePlateformeAssign != null)
                    {

                        groupePlateformeAssign.ChangeTarget(indexTarget, indexGroupesBug);
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

        public void DestroyMe()
        {
            Destroy(gameObject);
        }
    }
}
