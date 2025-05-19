using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Timeline.TimelinePlaybackControls;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Referências aos scripts
    public Spawner spawner; 
    public PlayerHealth playerHealth; 
    public PlayerController playerController;

    // Refência aos elementos da interface
    public TextMeshProUGUI handText;
    public Image barraDeVida;
    public TextMeshProUGUI gameOverText;
    public TextMeshProUGUI startText;
    public TextMeshProUGUI winText;
    public Button startButton;
    public Button restartButton;
    public TextMeshProUGUI tempoText;

    // Controle do jogo
    public bool jogoComecou = false;
    public float tempoLimite = 45f;
    private float tempoRestante;
    private int vidaBossAtual;
    private int vidaBossTotal = 7;

    // Áudio
    public AudioSource audioManager;
    public AudioClip winMusic;
    public AudioClip FundoMusic;
    public AudioClip gameOverMusic;


    // Start is called before the first frame update
    void Start()
    {
        // Inicializa a interface
        gameOverText.gameObject.SetActive(false);
        winText.gameObject.SetActive(false);
        restartButton.gameObject.SetActive(false);

        vidaBossAtual = vidaBossTotal;
        tempoRestante = tempoLimite;

        AlterarBarraDeVida(vidaBossAtual, vidaBossTotal);
        AtualizarTempoUI();

        // Toca a música de fundo
        audioManager.clip = FundoMusic;
        audioManager.Play();
        audioManager.loop = true;
    }
    public void IniciarJogo()
    {
        // Começa o jogo ao clicar no botão
        jogoComecou = true;
        startText.gameObject.SetActive(false);
        startButton.gameObject.SetActive(false);
        spawner.IniciarSpawn(); // Começa a spawnar objetos
    }

    // Update is called once per frame
    void Update()
    {
        // Atualiza o tempo do jogo enquanto ele está rolando
        if (jogoComecou)
        {
            tempoRestante -= Time.deltaTime;
            AtualizarTempoUI();

            if (tempoRestante <= 0)
            {
                GameOver(); // Fim do jogo se o tempo acabar
            }
        }
    }

    // Atualiza o tempo formatado na tela
    void AtualizarTempoUI()
    {
        int minutos = Mathf.FloorToInt(tempoRestante / 60);
        int segundos = Mathf.FloorToInt(tempoRestante % 60);
        tempoText.text = string.Format("Tempo: {0:0}:{1:00}", minutos, segundos);
    }

    // Atualiza o texto da mão
    public void UpdateHand(string s) 
    { 
        handText.text = s;
    }

    // Reduz a vida do chefão
    public void Dano(int dano) 
    {
        vidaBossAtual -= dano;
        AlterarBarraDeVida(vidaBossAtual, vidaBossTotal);

        // Se vida chega a 0, ganha o jogo
        if (vidaBossAtual <= 0)
        {
            VencerJogo();
        }
    }

    // Atualiza o preenchimento da barra de vida
    public void AlterarBarraDeVida(int atual, int max) 
    {
        barraDeVida.fillAmount = (float) atual / max;
    }

    // Remove todos os objetos spawnados em cena
    public void LimparObjetosSpawnados()
    {
        foreach (Transform filho in spawner.containerDeSpawnados)
        {
            Destroy(filho.gameObject);
        }
    }

    public void RestartGame()
    {
        // Reinicia o jogo
        playerHealth.ResetarVidas();
        playerController.ResetarPlayer();
        ResetarBoss();
        tempoRestante = tempoLimite;
        AtualizarTempoUI();

        // 'Despausa' o jogo
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
        // Reinicia a vida do boss
        vidaBossAtual = vidaBossTotal;
        AlterarBarraDeVida(vidaBossAtual, vidaBossTotal);
    }
    public void GameOver()
    {
        // Mostra tela de game over
        gameOverText.gameObject.SetActive(true);
        restartButton.gameObject.SetActive(true);

        LimparObjetosSpawnados();

        // Troca a música para a de game over
        audioManager.Stop();
        audioManager.clip = gameOverMusic;
        audioManager.Play();
        audioManager.loop = false;
        Time.timeScale = 0f; // Pausa o jogo

    }

    public void VencerJogo()
    {
        // Mostra tela de vitória
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
}
