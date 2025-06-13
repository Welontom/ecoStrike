using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float horizontalInput;
    public float verticalInput;
    public float speed = 10.0f;

    public CharacterController controller;
    public Animator animator;

    public bool isHolding;  // Se o jogador está segurando um lixo
    public bool paper;
    public bool metal;
    public bool glass;
    public bool plastic;

    public GameManager gameManager; // Referência ao GameManager
    public ParticleSystem explosionParticle;

    public AudioClip hitSound;
    public AudioSource audioSource;



    // Start is called before the first frame update
    void Start()
    {
        // Inicializa os estados
        isHolding = false;
        paper = false;
        metal = false;
        glass = false;
        plastic = false;

        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();

        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        // Lê entrada do jogador
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

        // Cria vetor de movimento
        Vector3 movimento = new Vector3(horizontalInput, 0,verticalInput);

        // Move o jogador
        controller.Move(movimento * Time.deltaTime * speed);
        controller.Move(new Vector3(0, -9.81f, 0) * Time.deltaTime);

        // Rotaciona o personagem para a direção do movimento
        if (movimento != Vector3.zero) 
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(movimento), Time.deltaTime * 10);
        }

        // Atualiza animação de movimento
        animator.SetBool("movendo", movimento != Vector3.zero);
    }

    // Verifica se encostou na lixeira correta com o lixo correto, dá dano no chefe,  reseta estados, ativa explosão e toca efeito
    void OnTriggerEnter(Collider other)
    {
        
        if (other.CompareTag("lixeira_vidro") && glass == true) 
        {
            explosionParticle = Resources.Load<ParticleSystem>("Explosion_green");
            gameManager.Dano(1); // Dano ao chefe
            ResetarPlayer();
            Instantiate(explosionParticle, other.transform.position, explosionParticle.transform.rotation);
            audioSource.PlayOneShot(hitSound, 1.0f);
        }
        if (other.CompareTag("lixeira_metal") && metal == true)
        {
            explosionParticle = Resources.Load<ParticleSystem>("Explosion_yellow");
            gameManager.Dano(1);  // Dano ao chefe
            ResetarPlayer();
            Instantiate(explosionParticle, other.transform.position, explosionParticle.transform.rotation);
            audioSource.PlayOneShot(hitSound, 1.0f);
        }
        if (other.CompareTag("lixeira_plastico") && plastic == true)
        {
            explosionParticle = Resources.Load<ParticleSystem>("Explosion_red");
            gameManager.Dano(1);  // Dano ao chefe
            ResetarPlayer();
            Instantiate(explosionParticle, other.transform.position, explosionParticle.transform.rotation);
            audioSource.PlayOneShot(hitSound, 1.0f);
        }
        if (other.CompareTag("lixeira_papel") && paper == true)
        {
            explosionParticle = Resources.Load<ParticleSystem>("Explosion_blue");
            gameManager.Dano(1);  // Dano ao chefe
            ResetarPlayer();
            Instantiate(explosionParticle, other.transform.position, explosionParticle.transform.rotation);
            audioSource.PlayOneShot(hitSound, 1.0f);
        }
    }

    // Limpa o estado de coleta do jogador
    public void ResetarPlayer()
    {
        isHolding = false;
        gameManager.UpdateHand("Mão vazia","branco");
        paper = metal = glass = plastic = false;
    }
}
