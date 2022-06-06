using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ByPass
{
    [System.Serializable]
    public class PhantomeSave
    {
        List<Vector3> transfomPlayer;
        List<float> timeTransforme;

        public List<Vector3> TransformPlayer
        {
            get
            {
                return transfomPlayer;
            }
        }

        public List<float> TimeTransforme
        {
            get
            {
                return timeTransforme;
            }
        }
        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        public void initPhantome()
        {
            transfomPlayer = new List<Vector3>();
            timeTransforme = new List<float>();
        }

        public void AddTransfomTime (Vector3 transform, float time)
        {
            transfomPlayer.Add(transform);
            timeTransforme.Add(time);
        }
    }
}
