using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestruct : MonoBehaviour
{
    [SerializeField] float _timeToAutoDestruct;
    bool alreadyLaunch;
    IEnumerator LaunchAutoDestruct()
    {
        yield return new WaitForSeconds(.2f);
        _timeToAutoDestruct -= .2f;
        if (_timeToAutoDestruct < 0)
        {
            StartCoroutine(LaunchEffect());
        }
        else
        {
            StartCoroutine(LaunchAutoDestruct());
        }
    }
    IEnumerator LaunchEffect()
    {
        yield return new WaitForSeconds(1f);
        Destroy(this.gameObject);
    }

        private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 7)
        {
            if (!alreadyLaunch)
            {
                StartCoroutine(LaunchAutoDestruct());
                alreadyLaunch = true;
            }
        }
    }
}
