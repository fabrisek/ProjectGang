using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathTuto : MonoBehaviour
{
    [SerializeField] RobotTutoController robotTutoController;
    public int startIndex = 0;
    Transform currentCPPos;
    [SerializeField] ParticleSystem deathParticule;
    // Start is called before the first frame update
    void Start()
    {
        startIndex = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == 15)
        {
            transform.position = robotTutoController.RespawnPosition(startIndex).position;
            AudioManager.instance.playSoundEffect(8, 0.5f);
            deathParticule.Play();
            
        }
    }
}
