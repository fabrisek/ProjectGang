using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Outil : MonoBehaviour
{

    public GameObject player;
    public GameObject endSpawn;

    public Transform Spawn;

    public LayerMask layer;

    public Canvas canvasTimer;
    public float timer;
    public static bool _isDead;
    public TextMeshProUGUI text;
    // Start is called before the first frame update
    void Start()
    {
        Instantiate(player, Spawn.position, Quaternion.identity);

        //Instantiate(canvasTimer);
       

        _isDead = false;

        
    }

    // Update is called once per frame
    void Update()
    {
        text.text = timer.ToString();
        if (!_isDead)
        {
            timer += Time.deltaTime;
        }
        else
        { 

        }
            
       
    }

   
}
