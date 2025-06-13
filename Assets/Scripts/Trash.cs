using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Trash : MonoBehaviour
{
    private PlayerController playerController;
    private GameManager gameManager;
    public ParticleSystem explosionParticle;

    // Start is called before the first frame update
    void Start()
    {
        // Inicia a contagem para destruir o objeto automaticamente após 10 segundos
        StartCoroutine(DestroyTrash());

        playerController = GameObject.Find("kaya").GetComponent<PlayerController>();
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter(Collider other)
    {
        // Quando colide com o jogador e ele não está segurando outro objeto
        if (other.gameObject.CompareTag("Player") && playerController.isHolding == false) 
        {

            // Destroi o objeto de lixo
            Destroy(gameObject);

            // Marca que o jogador está segurando algo
            playerController.isHolding = true;

            // Verifica o tipo do lixo e atualiza o estado e o texto
            if (gameObject.CompareTag("vidro"))
            {
                playerController.glass = true;
                gameManager.UpdateHand("Mão: Vidro","verde");
            }
            if (gameObject.CompareTag("metal")) 
            {
                gameManager.UpdateHand("Mão: Metal","amarelo");
                playerController.metal = true;
            }
            if (gameObject.CompareTag("papel"))
            {
                gameManager.UpdateHand("Mão: Papel","azul");
                playerController.paper = true;
            }
            if (gameObject.CompareTag("plastico"))
            {
                gameManager.UpdateHand("Mão: Plástico","vermelho");
                playerController.plastic = true;
            }
            
        }

    }

    // Coroutine que aguarda 10 segundos antes de destruir automaticamente o lixo
    IEnumerator DestroyTrash()
    {
        yield return new WaitForSeconds(10);
        Destroy(gameObject);

        // Instancia a partícula de explosão no lugar do objeto destruído
        Instantiate(explosionParticle, transform.position, explosionParticle.transform.rotation, transform.parent);

    }
}
