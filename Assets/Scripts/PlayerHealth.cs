using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public int maxLives = 3;              // Quantidade máxima de vidas
    private int currentLives;             // Vidas atuais do jogador
    public Image[] hearts;                // Imagens dos corações na UI
    public bool invencible; 
    public float blinkDuration = 1f;
    public float blinkInterval = 0.1f;
    private Renderer characterRenderer;
    private GameManager gameManager;

    public Image skillimage;
    public float cooldown = 10f;
    public float lastUsedTime = -Mathf.Infinity;

    public float distanciaTeleporte = 10f;

    public float raioDeAtracao = 30f;
    public float forcaDeAtracao = 30f;
    public float duracaoDoIma = 1f;
  
    public ParticleSystem explosionParticle;
    // Start is called before the first frame update

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        characterRenderer = GetComponentInChildren<Renderer>();
        currentLives = maxLives;       // Inicia com todas as vidas
        UpdateHearts();                // Atualiza a UI dos corações
        invencible = false;

    }

    public void TakeDamage()
    {
        // Se já está sem vidas, não faz nada
        if (currentLives <= 0) return;

        if(invencible == false)
        {
            currentLives--;
            UpdateHearts();
        }
        
        // Se zerou as vidas, chama Game Over
        if (currentLives <= 0)
        {
            Debug.Log("Game Over!");
            gameManager.GameOver();
        }
    }

    // Ativa ou desativa as imagens de coração com base nas vidas
    void UpdateHearts()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            hearts[i].enabled = i < currentLives;
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && SceneManager.GetActiveScene().name == "Level2" && Time.time >= lastUsedTime + cooldown)
        {
            StartCoroutine(Invencibilidade(2f));
            lastUsedTime = Time.time;
        }
        if (Input.GetKeyDown(KeyCode.E) && SceneManager.GetActiveScene().name == "Level3" && Time.time >= lastUsedTime + cooldown)
        {
            TeleportarParaFrente();
            lastUsedTime = Time.time;
        }
        if (Input.GetKeyDown(KeyCode.E) && SceneManager.GetActiveScene().name == "Level5" && Time.time >= lastUsedTime + cooldown)
        {
            StartCoroutine(AtivarImã());
            lastUsedTime = Time.time;
        }



        AtualizarCooldown();
    }
    private void OnParticleCollision(GameObject other)
    {
        // Se colidiu com uma explosão e não está invencível, perde vida
        if (other.CompareTag("explosion") && invencible == false)
        {
            TakeDamage();
            StartCoroutine(Invencibilidade(blinkDuration)); // Inicia o efeito de invencibilidade temporária
        }
        
    }
    IEnumerator Invencibilidade(float duration) 
    {
        invencible = true;
        float timer = 0f;

        // Enquanto estiver no tempo de invencibilidade, pisca o personagem
        while (timer < duration)
        {
            characterRenderer.enabled = false;
            yield return new WaitForSecondsRealtime(blinkInterval / 2f);

            characterRenderer.enabled = true;
            yield return new WaitForSecondsRealtime(blinkInterval / 2f);

            timer += blinkInterval;
        }
        invencible = false;
    }

    // Restaura todas as vidas e atualiza a UI
    public void ResetarVidas()
    {
        currentLives = maxLives;
        UpdateHearts();
    }
    public void AtualizarCooldown() 
    {
        float tempoRestante = (lastUsedTime + cooldown) - Time.time;
        if (tempoRestante <= 0)
        {
            skillimage.enabled = true;
        }
        else
        {
            skillimage.enabled = false;
        }
    }
    void TeleportarParaFrente()
    {
        Vector3 direcao = transform.forward;
        Vector3 destino = transform.position + direcao * distanciaTeleporte;


        GetComponent<CharacterController>().enabled = false;
        explosionParticle = Resources.Load<ParticleSystem>("Explosion_blue");
        Instantiate(explosionParticle, transform.position, explosionParticle.transform.rotation);
        transform.position = destino;
        GetComponent<CharacterController>().enabled = true;
        Debug.Log("Teleporte realizado!");
    }
    IEnumerator AtivarImã()
    {
        float tempo = 0f;
        
        while (tempo < duracaoDoIma)
        {
            AtrairLixos();
            Debug.Log(tempo);
            tempo += Time.deltaTime;
            yield return null;
        }

    }
    void AtrairLixos()
    {
        Collider[] lixos = Physics.OverlapSphere(transform.position, raioDeAtracao);

        foreach (Collider col in lixos)
        {
            if (col.CompareTag("vidro") || col.CompareTag("plastico") || col.CompareTag("metal") || col.CompareTag("papel"))
            {
                Rigidbody rb = col.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    Vector3 direcao = (transform.position - col.transform.position).normalized;
                    rb.AddForce(direcao * forcaDeAtracao, ForceMode.Acceleration);
                }
                else
                {
                    // alternativa: mover diretamente se não tiver Rigidbody
                    col.transform.position = Vector3.MoveTowards(
                        col.transform.position,
                        transform.position,
                        forcaDeAtracao * Time.deltaTime
                    );
                }
            }
        }
    }

}
