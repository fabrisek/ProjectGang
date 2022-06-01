using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager gameManager;
    
    public GameObject Player;
    public GameObject cam;
    public GameObject GetPlayer()
    {
        return Player;
    }
    public GameObject GetCamera()
    {
        return cam;
    }
    void Awake()
    {
        if (gameManager != null && gameManager != this)
            Destroy(gameObject);    // Suppression d'une instance précédente (sécurité...sécurité...)

        gameManager = this;
    }
}
