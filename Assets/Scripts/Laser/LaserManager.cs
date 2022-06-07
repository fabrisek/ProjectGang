using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ByPass
{
    public class LaserManager : MonoBehaviour
    {
        [SerializeField] List<LaserController> lasers;
        [SerializeField] float maxDist;
        // Start is called before the first frame update
        void Start()
        {
            SetMaxDisToAllLaser(maxDist);
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        void SetMaxDisToAllLaser (float maxDistToSet)
        {
            for(int i = 0; i < lasers.Count;i++)
            {
                lasers[i].initViewAndDistMax(maxDistToSet);
            }
        }
    }
}
