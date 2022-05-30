using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager gameManager;
    
    public GameObject Player;
    public GameObject camera;
    public GameObject GetPlayer()
    {
        return Player;
    }
    public GameObject GetCamera()
    {
        return camera;
    }
    void Awake()
    {
        if (gameManager != null && gameManager != this)
            Destroy(gameObject);    // Suppression d'une instance précédente (sécurité...sécurité...)

        gameManager = this;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
