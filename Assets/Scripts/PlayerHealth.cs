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

}
