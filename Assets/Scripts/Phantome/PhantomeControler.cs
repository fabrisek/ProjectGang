using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PhantomeControler : MonoBehaviour
{
    [SerializeField] public static PhantomeControler Instance;
    [SerializeField] float timeToSave;
    [SerializeField] Transform playerRef;
    [SerializeField] GameObject objectView;
    [SerializeField] FinishLine finishLine;
    [SerializeField] Material fantomeMat;

    PhantomeSave PhantomeSave;
    PhantomeSave reproduce;

    bool setTime;


    int indexOfPath;
    float t;


    bool phantomFinish;

    bool stopSave;
    public PhantomeSave phantomeSave
    {
        get
        {
            return PhantomeSave;
        }
    }

    private void Awake()
    {
        
        Instance = this;
    }
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

               if(!setTime)
            {
                StartCoroutine(CoroutineSaveTransformeTime());
            }

                if (reproduce != null && !phantomFinish)
                {
                    StartPath();
                }

            }
        

    }

    void initPhantome()
    {
        if (Data_Manager.Instance != null)
        {
            if (finishLine != null)
            {
                reproduce = Data_Manager.Instance.GetData()._worldData[finishLine.WorldIndex]._mapData[finishLine.LevelIndex].GetPhantomSave();
            }
        }
        PhantomeSave = new PhantomeSave();
        PhantomeSave.initPhantome();
        indexOfPath = 0;

        // Chopper la sauvegarde si il y a;
    }

    IEnumerator CoroutineSaveTransformeTime()
    {
        setTime = true;
        do
        {
            yield return new WaitForSeconds(timeToSave);
            SeTTimeTransform();

        }
        while (!stopSave);
    }

    void SeTTimeTransform()
    {
        PhantomeSave.AddTransfomTime(playerRef.position, Timer.Instance.GetTimer());
    }

    void StartPath()
    {

        if (indexOfPath + 1 < reproduce.transfomPlayer.Count)
        {
            t += Time.unscaledDeltaTime * (1 / timeToSave);
            objectView.transform.position = Vector3.Lerp(reproduce.transfomPlayer[indexOfPath], reproduce.transfomPlayer[indexOfPath + 1], t);
            if (t >= 1)
            {
                t = 0;
                indexOfPath++;

            }
        }
        else
        {
            phantomFinish = true;
        }
       
    }

  




}

