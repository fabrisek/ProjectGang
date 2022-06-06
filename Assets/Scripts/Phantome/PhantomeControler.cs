using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ByPass
{
    public class PhantomeControler : MonoBehaviour
    {
        [SerializeField] float timeToSave;
        [SerializeField] Transform playerRef;
        [SerializeField] GameObject objectView;

        PhantomeSave PhantomeSave;
        PhantomeSave reproduce;

        bool setTime;

        float speed;
        bool speedSet;
        int indexOfPath;
        float t;


        bool phantomFinish;
        bool tst;
        // Start is called before the first frame update
        void Start()
        {
            initPhantome();
        }

        // Update is called once per frame
        void Update()
        {
            if (Timer.Instance != null && Timer.Instance.GetTimer() != 0)
            {
                if (Timer.Instance != null && Timer.Instance.GetTimer() < 20)
                {
                    if (!setTime)
                    {
                        StartCoroutine(CoroutineSaveTransformeTime());
                    }
                }
                else if (!tst)
                {
                    reproduce = PhantomeSave;
                    tst = true;
                }
                    if (Timer.Instance != null && Timer.Instance.GetTimer() >= 30)
                {
                    if (reproduce != null && !phantomFinish)
                    {
                        StartPath();
                    }
                }
            }
        
        }

        void initPhantome ()
        {
            PhantomeSave = new PhantomeSave();
            PhantomeSave.initPhantome();
            indexOfPath = 0;

            // Chopper la sauvegarde si il y a;
        }

        IEnumerator CoroutineSaveTransformeTime ()
        {
            setTime = true;
            yield return new WaitForSeconds(timeToSave);
            SeTTimeTransform();
            setTime = false;
        }

        void SeTTimeTransform()
        {
            PhantomeSave.AddTransfomTime(playerRef.position, Timer.Instance.GetTimer());
        }

        void StartPath ()
        {
            if(!speedSet)
            {
                if (indexOfPath + 1 < PhantomeSave.TransformPlayer.Count)
                {
                    speed = CalCulDeSpeed(reproduce.TransformPlayer[indexOfPath], reproduce.TransformPlayer[indexOfPath + 1], CalCulDeTemp(reproduce.TimeTransforme[indexOfPath], reproduce.TimeTransforme[indexOfPath + 1]));
                    Debug.Log(speed);
                    speedSet = true;
                }
                else
                {

                }
            }
            else
            {
                speed = 1;
                if (speed != 0)
                {
                    t += Time.deltaTime * 100;
                }
                else
                {
                    t = 1;
                }

                objectView.transform.position = Vector3.Lerp(reproduce.TransformPlayer[indexOfPath], reproduce.TransformPlayer[indexOfPath + 1], t);
                if(t >= 1)
                {
                    t = 0;
                    indexOfPath ++;
                    speedSet = false;
                    
                }
            }
        }


        float CalCulDeSpeed (Vector3 depart, Vector3 arriver, float time)
        {
            float dist = Vector3.Distance(depart, arriver);
            if (dist != 0)
            {
                int intTime = (int)time;
                float fraction = time * 1000;
                fraction = (fraction % 1000);
                return dist / fraction;
            }
            else
            {
                return 0;
            }
        }

        float CalCulDeTemp (float timeDepart, float timeArriver)
        {
            return timeArriver - timeDepart;
        }


    }
}
