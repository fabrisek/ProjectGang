using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeColorWall : MonoBehaviour
{
    public Color StartColor;
    public Color EndColor;
    public float time;
    bool goingForward;
    public bool isCycling;
    Material myMaterial;

    private void Awake()
    {
        goingForward = true;
        myMaterial = GetComponent<Renderer>().material;
    }

    private void Update()
    {
        if (!isCycling)
        {
            if (goingForward)
                StartCoroutine(CycleMaterial(StartColor, EndColor, time, myMaterial));
            else
                StartCoroutine(CycleMaterial(EndColor, StartColor, time, myMaterial));
        }
    }

    IEnumerator CycleMaterial(Color startColor, Color endColor, float cycleTime, Material mat)
    {
        isCycling = true;
        float currentTime = 0;
        while (currentTime < cycleTime)
        {
            currentTime += Time.deltaTime;
            float t = currentTime / cycleTime;
            Color currentColor = Color.Lerp(startColor, endColor, t);
            mat.color = currentColor;
            mat.SetColor("_EmissionColor", currentColor);
            yield return null;
        }
        isCycling = false;
        goingForward = !goingForward;

    }

}
