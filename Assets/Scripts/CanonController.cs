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
        //    Shoot();
        }
    }

    public void Shoot()
    {
        GameObject fourmis1 = Instantiate(fourmis,ShootDirection.position, Quaternion.identity);
        fourmis.transform.position = ShootDirection.transform.position;
        fourmis1.GetComponent<Rigidbody>().AddForce(Random.Range(explosionForce/2f,explosionForce*2f) * ShootDirection.forward, ForceMode.Impulse);

        fourmis1.GetComponent<Rigidbody>().freezeRotation = false;
        Debug.Log(fourmis1.transform.position);
        fourmis1.GetComponent<BugTargetController>().gravity = 20;

        //feedBack
        AudioManager.instance.playSoundEffect3D(16,ShootDirection.position , 10f);
        explosionEffect.Play();
    }

}
