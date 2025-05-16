using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float horizontalInput;
    public float verticalInput;
    public float speed = 10.0f;
    private CharacterController controller;
    private Animator animator;
    public bool isHolding;
    public bool paper;
    public bool metal;
    public bool glass;
    public bool plastic;
    private GameManager gameManager;
    private ParticleSystem explosionParticle;
    public Transform containerDeSpawnados;



    // Start is called before the first frame update
    void Start()
    {
        isHolding = false;
        paper = false;
        metal = false;
        glass = false;
        plastic = false;

        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();


        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        
    }

    // Update is called once per frame
    void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

        Vector3 movimento = new Vector3(horizontalInput, 0,verticalInput);


        controller.Move(movimento * Time.deltaTime * speed);

        controller.Move(new Vector3(0, -9.81f, 0) * Time.deltaTime);

        if (movimento != Vector3.zero) 
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(movimento), Time.deltaTime * 10);
        }

        animator.SetBool("movendo", movimento != Vector3.zero);
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("lixeira_vidro") && glass == true) 
        {
            explosionParticle = Resources.Load<ParticleSystem>("Explosion_green");
            gameManager.Dano(1);
            isHolding = false;
            glass = false;
            gameManager.UpdateHand("Mão vazia");
            Instantiate(explosionParticle, other.transform.position, explosionParticle.transform.rotation);
        }
        if (other.CompareTag("lixeira_metal") && metal == true)
        {
            explosionParticle = Resources.Load<ParticleSystem>("Explosion_yellow");
            gameManager.Dano(1);
            isHolding = false;
            metal = false;
            gameManager.UpdateHand("Mão vazia");
            Instantiate(explosionParticle, other.transform.position, explosionParticle.transform.rotation);
        }
        if (other.CompareTag("lixeira_plastico") && plastic == true)
        {
            explosionParticle = Resources.Load<ParticleSystem>("Explosion_red");
            gameManager.Dano(1);
            isHolding = false;
            plastic = false;
            gameManager.UpdateHand("Mão vazia");
            Instantiate(explosionParticle, other.transform.position, explosionParticle.transform.rotation);
        }
        if (other.CompareTag("lixeira_papel") && paper == true)
        {
            explosionParticle = Resources.Load<ParticleSystem>("Explosion_blue");
            gameManager.Dano(1);
            isHolding = false;
            paper = false;
            gameManager.UpdateHand("Mão vazia");
            Instantiate(explosionParticle, other.transform.position, explosionParticle.transform.rotation);
        }
    }
    public void ResetarPlayer()
    {
        isHolding = false;
        gameManager.UpdateHand("Mão vazia");
        paper = metal = glass = plastic = false;
    }
}
