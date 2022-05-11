using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelectorMenu : MonoBehaviour
{
    [SerializeField] GameObject _canvas;
    [SerializeField] int _sceneNumber;

    public int GetSceneNumber() { return _sceneNumber; }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 7)
        {
            _canvas.SetActive(true);

            other.GetComponent<MenuAntCrontroller>().SetLevelRef(this);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 7)
        {
            _canvas.SetActive(false);
        }
    }
}
