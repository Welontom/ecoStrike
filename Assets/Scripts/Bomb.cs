using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public ParticleSystem explosionParticle;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Explosion());
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator Explosion() 
    {
        yield return new WaitForSeconds(5);
        Destroy(gameObject);
        Instantiate(explosionParticle, transform.position, explosionParticle.transform.rotation, transform.parent);
    }
}
