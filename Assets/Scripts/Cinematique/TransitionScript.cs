using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TransitionScript : MonoBehaviour
{
    public static TransitionScript Instance;
    [SerializeField] Image imageToFade;
    [SerializeField] Color color;
    [SerializeField] float speed;
    [SerializeField] GameObject canvas;

    float t;

   public  bool startFadeIn;
   public  bool startfadeOut;

    bool startFade;
    bool done;
    bool startPause;
    bool coroutineStart;
    private void Awake()
    {

        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);    // Suppression d'une instance précédente (sécurité...sécurité...)
            
        }
        else
        {
            DontDestroyOnLoad(this.gameObject);
            Instance = this;
        }
 
    }

   

    public void Fade(float speed)
    {
      
        if(speed == 0)
        {
            this.speed = 2;
        }
        else
        {
            this.speed = speed;
        }

        done = false;
        startFade = true;
        t = 0;
        canvas.SetActive(true);
        startPause = false;
        coroutineStart = false;
        
    }

    private void Start()
    {
       
    }

    private void Update()
    {
        
       
        if (startFade)
        {
            if (startPause)
            {
                if(!coroutineStart)
                {
                   StopCoroutine(CoroutineFadeV2());
                    StartCoroutine(CoroutineFadeV2());
                }

                if (done)
                {
                    startFade = StartFadeOut();

                }
            }
            else
            {
                startPause = StartFadeIn();
            }
           
           
        }
    }
     bool StartFadeOut ()
    {
        
        t += Time.deltaTime * speed;
        imageToFade.color = Color.Lerp(color, new Color(color.r, color.g, color.b, 0), t);
        
        if (t >= 1)
        {
            t = 1;
            
            canvas.SetActive(false); 
            startFade = false;
            startPause = false;
            coroutineStart = false;
            return false;
        }

        return true;

    }

     bool StartFadeIn()
    {
        t -= Time.deltaTime * speed;
        imageToFade.color = Color.Lerp(color, new Color(color.r, color.g, color.b, 0), t);
        
        if (t <= 0)
        {
            t = 0;
           
            
            return true;
        }

        return false;

    }

    public IEnumerator CoroutineFadeV2 ()
    {
        coroutineStart = true;
        yield return new WaitForSeconds(0.1f);
       // if()
        done = true;
        
    }

    public IEnumerator CoroutineFade(bool outOrIn)
    {
        /*  Debug.Log("Je commence Transition");
          canvas.SetActive(true);
          if (outOrIn)
          {
              startFadeIn = true;
              if(t != 1)
              {
                  t = 1;
              }
          }
          else
          {
              startfadeOut = true;
              if (t != 0)
              {
                  t = 0;
              }
          }
       // yield return new  CustomYieldInstruction.keepWaiting()
          yield return new WaitForSeconds(1/speed);
          Debug.Log("Je finie Transition");
          canvas.SetActive(false);
          if (outOrIn)
          {
              startFadeIn = false;
          }
          else
          {
              startfadeOut = false;
          }*/
        yield return new WaitForSeconds(1 / speed);

    }
}
