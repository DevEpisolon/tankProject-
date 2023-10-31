using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCollisionHandler : MonoBehaviour
{
    public GameObject explosionPrefab;
    private ParticleSystem explosionEffect;


    private void Start()
    {
        explosionEffect = explosionPrefab.GetComponent<ParticleSystem>();
    }
    private void OnCollisionEnter (Collision collision)
    {
        if(collision.gameObject.tag == "Tank"){
            return;
        }
        
        GameObject explosionObject = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
        ParticleSystem instantiatedEffect = explosionObject.GetComponent<ParticleSystem>();
        instantiatedEffect.Play();
        Destroy(explosionObject, instantiatedEffect.main.duration);
    }
}
