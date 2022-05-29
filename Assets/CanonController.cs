using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanonController : MonoBehaviour
{
    [SerializeField] float timerShoot;
    float timerShootReset;
    [SerializeField] GameObject fourmis;
    [SerializeField] Transform ShootDirection;
    [SerializeField] float explosionForce;
    [SerializeField] ParticleSystem explosionEffect;
    // Start is called before the first frame update
    void Start()
    {
        timerShootReset = timerShoot;
    }

    // Update is called once per frame
    void Update()
    {
        timerShoot -= Time.deltaTime;
        if(timerShoot<0)
        {
            timerShoot = timerShootReset;
            Shoot();
        }
    }

    public void Shoot()
    {
        Instantiate(fourmis,ShootDirection, true);
        fourmis.GetComponent<Rigidbody>().AddForce(explosionForce * ShootDirection.forward, ForceMode.Impulse);

        //feedBack
        AudioManager.instance.playSoundEffect3D(16,ShootDirection.position , 3f);
    }

}
