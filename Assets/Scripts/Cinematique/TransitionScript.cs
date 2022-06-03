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
    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);    // Suppression d'une instance précédente (sécurité...sécurité...)
        DontDestroyOnLoad(this.gameObject);
        Instance = this;


       

    }

    public void Fade()
    {
        StartCoroutine(CoroutineFadeV2());
    }

    private void Start()
    {
        //StartCoroutine(CoroutineFade(false));
    }

    private void Update()
    {
      /*  if(startFadeIn)
        {
            StartFadeIn();
        }
        if(startfadeOut)
        {
            StartFadeOut();
        }*/

        if(startFade)
        {
           
            if(done)
            {
                startFade = StartFadeOut();
                
            }
            else
            {
                done = StartFadeIn();
            }
        }
    }
     bool StartFadeOut ()
    {
        
        t += Time.deltaTime * speed;
        imageToFade.color = Color.Lerp(color, new Color(color.r, color.g, color.b, 0), t);
        if(t >= 1)
        {
            t = 1;
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
        Debug.Log("On mactive");
        done = false;
        startFade = true;
        t = 0;
        canvas.SetActive(true);
        yield return new WaitForSeconds((1 / speed)*2);
        startFade = false;
        canvas.SetActive(false);
        Debug.Log("Terminer");
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
