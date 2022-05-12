using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateformAutoDestruct : MonoBehaviour
{
    [SerializeField] float _timerAutoDestruct;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 7)
        {
            StartCoroutine(AutoDestruct());
        }
    }

    IEnumerator AutoDestruct()
    {
        yield return new WaitForSeconds(_timerAutoDestruct);
    }
}
