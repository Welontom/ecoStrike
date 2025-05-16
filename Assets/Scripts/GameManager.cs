using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Timeline.TimelinePlaybackControls;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public Spawner spawner; 
    public PlayerHealth playerHealth; 
    public PlayerController playerController;

    public TextMeshProUGUI handText;
    public Image barraDeVida;
    public TextMeshProUGUI gameOverText;
    public TextMeshProUGUI startText;
    public TextMeshProUGUI winText;
    public Button startButton;
    public Button restartButton;
    public TextMeshProUGUI tempoText;

    public bool jogoComecou = false;
    public float tempoLimite = 45f;
    private float tempoRestante;
    private int vidaBossAtual;
    private int vidaBossTotal = 7;


    public AudioSource audioManager;
    public AudioClip winMusic;
    public AudioClip FundoMusic;
    public AudioClip gameOverMusic;


    // Start is called before the first frame update
    void Start()
    {
        gameOverText.gameObject.SetActive(false);
        winText.gameObject.SetActive(false);
        restartButton.gameObject.SetActive(false);

        vidaBossAtual = vidaBossTotal;
        tempoRestante = tempoLimite;
        AlterarBarraDeVida(vidaBossAtual, vidaBossTotal);
        AtualizarTempoUI();

        audioManager.clip = FundoMusic;
        audioManager.Play();
        audioManager.loop = true;
    }
    public void IniciarJogo()
    {
        jogoComecou = true;
        startText.gameObject.SetActive(false);
        startButton.gameObject.SetActive(false);
        spawner.IniciarSpawn(); // ativa o spawn
    }

    // Update is called once per frame
    void Update()
    {
        if (jogoComecou)
        {
            tempoRestante -= Time.deltaTime;
            AtualizarTempoUI();

            if (tempoRestante <= 0)
            {
                GameOver(); // Chama o GameOver se o tempo acabar
            }
        }
    }
    public void UpdateHand(string s) 
    { 
        handText.text = s;
    }
    public void Dano(int dano) 
    {
        vidaBossAtual -= dano;
        AlterarBarraDeVida(vidaBossAtual, vidaBossTotal);
        if (vidaBossAtual <= 0)
        {
            VencerJogo();
        }
    }
    public void AlterarBarraDeVida(int atual, int max) 
    {
        barraDeVida.fillAmount = (float) atual / max;
    }

    public void LimparObjetosSpawnados()
    {
        foreach (Transform filho in spawner.containerDeSpawnados)
        {
            Destroy(filho.gameObject);
        }
    }

    public void RestartGame()
    {
        playerHealth.ResetarVidas();
        playerController.ResetarPlayer();
        ResetarBoss();
        tempoRestante = 45f;
        Time.timeScale = 1f;

        // Esconde textos e botão de reinício
        gameOverText.gameObject.SetActive(false);
        winText.gameObject.SetActive(false);
        restartButton.gameObject.SetActive(false);

        audioManager.clip = FundoMusic;
        audioManager.Play();
        audioManager.loop = true;
    }

    void ResetarBoss()
    {
        vidaBossAtual = vidaBossTotal;
        AlterarBarraDeVida(vidaBossAtual, vidaBossTotal);
    }
    public void GameOver()
    {
        gameOverText.gameObject.SetActive(true);
        restartButton.gameObject.SetActive(true);
        LimparObjetosSpawnados();
        audioManager.Stop();
        audioManager.clip = gameOverMusic;
        audioManager.Play();
        audioManager.loop = false;
        Time.timeScale = 0f; // Pausa o jogo

    }

    public void VencerJogo()
    {
        winText.gameObject.SetActive(true);
        restartButton.gameObject.SetActive(false);
        startText.gameObject.SetActive(false);
        LimparObjetosSpawnados();
        audioManager.Stop();
        audioManager.clip = winMusic;
        audioManager.Play();
        audioManager.loop = false;
        Time.timeScale = 0f; // Pausa o jogo
        
    }

    void AtualizarTempoUI()
    {
        int minutos = Mathf.FloorToInt(tempoRestante / 60);
        int segundos = Mathf.FloorToInt(tempoRestante % 60);
        tempoText.text = string.Format("Tempo: {0:0}:{1:00}", minutos, segundos);
    }
}
