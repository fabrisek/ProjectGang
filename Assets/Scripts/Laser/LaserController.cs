using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ByPass
{
    public class LaserController : MonoBehaviour
    {
        [SerializeField] LayerMask layer;
        [SerializeField] float distMax;
        [SerializeField] Transform view;

        public float DistMax
        {
            set
            {
                distMax = value;

            }
        }
        private void Awake()
        {
            initViewAndDistMax(distMax);
        }
        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
              transform.localScale = new Vector3(1,1, CheckDistLaser());
        }

       public void initViewAndDistMax (float dist)
        {
           
                distMax = dist;
            
            view.localScale = new Vector3(view.localScale.x, distMax / 2, view.localScale.z);
            view.parent.localPosition = new Vector3(0, 0, distMax / 2);
        }

        float CheckDistLaser ()
        {
            float distLaser = 1;

            RaycastHit[] hits = Physics.RaycastAll(transform.position, transform.forward, distMax, layer);
            if(hits.Length >0)
            {
                distLaser = (Vector3.Distance(transform.position, hits[0].point))/distMax;
            }


            return distLaser;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(transform.position + transform.forward * distMax, 2);
        }
    }
}
