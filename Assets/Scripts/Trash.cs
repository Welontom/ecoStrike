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
        if (other.gameObject.CompareTag("Player") && playerController.isHolding == false) 
        {
            Destroy(gameObject);
            playerController.isHolding = true;
            if (gameObject.CompareTag("vidro"))
            {
                playerController.glass = true;
                gameManager.UpdateHand("Mão: Vidro");
            }
            if (gameObject.CompareTag("metal")) 
            {
                gameManager.UpdateHand("Mão: Metal");
                playerController.metal = true;
            }
            if (gameObject.CompareTag("papel"))
            {
                gameManager.UpdateHand("Mão: Papel");
                playerController.paper = true;
            }
            if (gameObject.CompareTag("plastico"))
            {
                gameManager.UpdateHand("Mão: Plástico");
                playerController.plastic = true;
            }
            
        }

    }
    IEnumerator DestroyTrash()
    {
           yield return new WaitForSeconds(10);
           Destroy(gameObject);
           Instantiate(explosionParticle, transform.position, explosionParticle.transform.rotation, transform.parent);

    }
}
