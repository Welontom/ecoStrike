using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public ParticleSystem explosionParticle;
    public AudioClip explosionSound;       
    private AudioSource audioSource;         

    // Chama a explos�o assim que a bomba � criada
    void Start()
    {
        // Obt�m o componente AudioSource no objeto da bomba
        audioSource = GetComponent<AudioSource>();

        audioSource.clip = explosionSound;

        // Inicia a coroutine para a explos�o ap�s o tempo determinado
        StartCoroutine(Explosion());
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Aguarda 5 segundos, destr�i a bomba e instancia a explos�o
    IEnumerator Explosion() 
    {
        yield return new WaitForSeconds(5);

        // Reproduz o som da explos�o
        audioSource.PlayOneShot(explosionSound, 1.0f);

        // Deixa o objeto inv�vel para criar efeito de 'destruido', mas sem destruir o gameObejct ainda
        GetComponent<MeshRenderer>().enabled = false;

        // Destroi o objeto s� quando termina de tocar a explos�o
        Destroy(gameObject, explosionSound.length);

        // Cria a explos�o no mesmo local da bomba
        Instantiate(explosionParticle, transform.position, explosionParticle.transform.rotation, transform.parent);
    }
}
