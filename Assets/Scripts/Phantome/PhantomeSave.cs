using System.Collections;
using System.Collections.Generic;
using UnityEngine;


    [System.Serializable]
    public class PhantomeSave
    {
        public List<Vector3> transfomPlayer;
        public List<float> timeTransforme;

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

