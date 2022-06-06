using System.Collections;
using System.Collections.Generic;
using UnityEngine;


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

