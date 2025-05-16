using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public int maxLives = 3;
    private int currentLives;
    public Image[] hearts;
    bool invencible;
    public float blinkDuration = 1f;
    public float blinkInterval = 0.1f;
    private Renderer characterRenderer;
    private GameManager gameManager;

    // arraste os corações no Inspector
    // Start is called before the first frame update

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        characterRenderer = GetComponentInChildren<Renderer>();
        currentLives = maxLives;
        UpdateHearts();
        invencible = false;
    }

    public void TakeDamage()
    {
        if (currentLives <= 0) return;
        currentLives--;
        UpdateHearts();

        if (currentLives <= 0)
        {
            Debug.Log("Game Over!");
            gameManager.GameOver(); // Chama tela de game over
        }
    }

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
        
    }
    private void OnParticleCollision(GameObject other)
    {
        if (other.CompareTag("explosion") && invencible == false)
        {
            TakeDamage();
            StartCoroutine(Invencibilidade());
        }
        
    }
    IEnumerator Invencibilidade() 
    {
        invencible = true;
        float timer = 0f;

        while (timer < blinkDuration)
        {
            characterRenderer.enabled = false;
            yield return new WaitForSecondsRealtime(blinkInterval / 2f);

            characterRenderer.enabled = true;
            yield return new WaitForSecondsRealtime(blinkInterval / 2f);

            timer += blinkInterval;
        }
        invencible = false;
    }

    public void ResetarVidas()
    {
        currentLives = maxLives;
        UpdateHearts();
    }

}
