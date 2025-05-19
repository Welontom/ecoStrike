using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public ParticleSystem explosionParticle;
    public AudioClip explosionSound;       
    private AudioSource audioSource;         

    // Chama a explosão assim que a bomba é criada
    void Start()
    {
        // Obtém o componente AudioSource no objeto da bomba
        audioSource = GetComponent<AudioSource>();

        audioSource.clip = explosionSound;

        // Inicia a coroutine para a explosão após o tempo determinado
        StartCoroutine(Explosion());
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Aguarda 5 segundos, destrói a bomba e instancia a explosão
    IEnumerator Explosion() 
    {
        yield return new WaitForSeconds(5);

        // Reproduz o som da explosão
        audioSource.PlayOneShot(explosionSound, 1.0f);

        // Deixa o objeto invível para criar efeito de 'destruido', mas sem destruir o gameObejct ainda
        GetComponent<MeshRenderer>().enabled = false;

        // Destroi o objeto só quando termina de tocar a explosão
        Destroy(gameObject, explosionSound.length);

        // Cria a explosão no mesmo local da bomba
        Instantiate(explosionParticle, transform.position, explosionParticle.transform.rotation, transform.parent);
    }
}
