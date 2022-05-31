using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HudStartScript : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI threeTwoOneSlider;
    [SerializeField] string[] txtThreeTwoOne;

    [SerializeField] public AnimationCurve curveChangeLetter;
   
    [SerializeField] float maxSizeFont;

    float timeToAnimLetter;
   
    float timeToReacToAnimLetterh;
    bool startTimeToAimLetter;
    
    // Start is called before the first frame update
    void Start()
    {
        InitLetterAnim();
        StartThreeTwoOne(2);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateTimerForLetterAnim();
    }

    void InitLetterAnim ()
    {
        timeToAnimLetter = 0;
        startTimeToAimLetter = false;
    }

    void UpdateTimerForLetterAnim ()
    {
        if (startTimeToAimLetter)
        {
            timeToAnimLetter = TimerLetterChange(timeToReacToAnimLetterh, timeToAnimLetter);
            ChangeLettersSizeAndAlpha();
        }
    }

    public void StartThreeTwoOne(float time)
    {
        startTimeToAimLetter = true;
        timeToReacToAnimLetterh = time / 3;
        threeTwoOneSlider.enabled = true;
        threeTwoOneSlider.text = txtThreeTwoOne[0];
        AudioManager.instance.playSoundEffect(1, 1);
        StopCoroutine(CoroutineAffichageImagesStart(time, 1));
        StartCoroutine(CoroutineAffichageImagesStart(time, 1));

    }

    float TimerLetterChange (float timeToReach, float time)
    {
        if(time < timeToReach)
        {
            time += Time.deltaTime;
        }
        else
        {
           
            startTimeToAimLetter = false;
        }
        return time;
    }

    void ChangeLettersSizeAndAlpha ()
    {
        threeTwoOneSlider.color = new Color(threeTwoOneSlider.color.r, threeTwoOneSlider.color.g, threeTwoOneSlider.color.b, curveChangeLetter.Evaluate(timeToAnimLetter/timeToReacToAnimLetterh));
        threeTwoOneSlider.fontSize = maxSizeFont *curveChangeLetter.Evaluate(timeToAnimLetter/timeToReacToAnimLetterh);
    }

    

    IEnumerator CoroutineAffichageImagesStart(float time, int index)
    {
        yield return new WaitForSeconds(time / 3);

        //Reset les lettre au debu de la curve
        this.timeToAnimLetter = 0;
        ChangeLettersSizeAndAlpha();

        //Affichage de la nouvelle lettre
        threeTwoOneSlider.text = txtThreeTwoOne[index];
        AudioManager.instance.playSoundEffect(1, 1);

        //set timer pour anim la lettre
        startTimeToAimLetter = true;
        timeToReacToAnimLetterh = time / 3;

        if (index + 1 < txtThreeTwoOne.Length)
        {

            StartCoroutine(CoroutineAffichageImagesStart(time, index + 1));
        }
        else
        {
            StartCoroutine(CoroutineRemoveTextGo(time));
        }
    }

    IEnumerator CoroutineRemoveTextGo(float time)
    {
        yield return new WaitForSeconds(time / 3);

        threeTwoOneSlider.enabled = false;
        threeTwoOneSlider.text = null;
    }

  

   
}
