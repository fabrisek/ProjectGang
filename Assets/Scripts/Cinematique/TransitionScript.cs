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

    bool startFadeIn;
    bool startfadeOut;
    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);    // Suppression d'une instance précédente (sécurité...sécurité...)
        DontDestroyOnLoad(this.gameObject);
        Instance = this;


       

    }

    private void Start()
    {
        StartCoroutine(CoroutineFade(false));
    }

    private void Update()
    {
        if(startFadeIn)
        {
            StartFadeIn();
        }
        if(startfadeOut)
        {
            StartFadeOut();
        }
    }
    public bool StartFadeOut ()
    {
        
        t += Time.deltaTime * speed;
        imageToFade.color = Color.Lerp(color, new Color(color.r, color.g, color.b, 0), t);
        if(t >= 1)
        {
            t = 1;
            return true;
        }

        return false;

    }

    public bool StartFadeIn()
    {
        t -= Time.deltaTime * speed;
        imageToFade.color = Color.Lerp(color, new Color(color.r, color.g, color.b, 0), t);
        Debug.Log(t);
        if (t <= 0)
        {
            t = 0;
            return true;
        }

        return false;

    }

    public IEnumerator CoroutineFade(bool outOrIn)
    {
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

        yield return new WaitForSeconds(1/speed);

        canvas.SetActive(false);
        if (outOrIn)
        {
            startFadeIn = false;
        }
        else
        {
            startfadeOut = false;
        }

    }
}
