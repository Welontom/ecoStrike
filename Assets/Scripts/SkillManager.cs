using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    [System.Serializable]
    public class SkillData
    {
        public string nome;
        public KeyCode tecla;
        public GameObject assetVisual;
        public float duracao;         // Quanto tempo a skill fica ativa
        public float tempoDeRecarga;  // Quanto tempo até poder usar de novo
    }

    public SkillData[] skills; // 5 habilidades (E, R, T, Y, U)
    private bool[] emUso;
    private bool[] emRecarga;

    private GameManager gameManager;
    private PlayerController playerController;


    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        playerController = FindObjectOfType<PlayerController>();

        emUso = new bool[skills.Length];
        emRecarga = new bool[skills.Length];
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < skills.Length; i++)
        {
            if (Input.GetKeyDown(skills[i].tecla) && !emUso[i] && !emRecarga[i])
            {
                StartCoroutine(AtivarSkill(i));
            }
        }
    }

    IEnumerator AtivarSkill(int index)
    {
        emUso[index] = true;
        SkillData skill = skills[index];

        // Piscar asset visual
        StartCoroutine(Piscar(skill.assetVisual, skill.duracao));

        // Ativar efeito específico
        switch (skill.tecla)
        {
            case KeyCode.E: // Escudo
                //playerController.SetInvencivel(true);
                yield return new WaitForSeconds(skill.duracao);
                //playerController.SetInvencivel(false);
                break;
            case KeyCode.R: // Dano ao boss
                gameManager.Dano(1); // Pode escalar por nível depois
                yield return new WaitForSeconds(skill.duracao);
                break;
            case KeyCode.T: // Pausar tempo
                Time.timeScale = 0f;
                yield return new WaitForSecondsRealtime(skill.duracao);
                Time.timeScale = 1f;
                break;
            case KeyCode.Y: // Aumentar velocidade
                //playerController.ModificarVelocidade(2f); // multiplicador
                yield return new WaitForSeconds(skill.duracao);
                //playerController.ModificarVelocidade(1f); // volta ao normal
                break;
            case KeyCode.U: // Diminuir raio da bomba
                //gameManager.ReduzirRaioBombas(skill.duracao); // implementar esse método
                yield return new WaitForSeconds(skill.duracao);
                break;
        }

        // Fica invisível durante recarga
        skill.assetVisual.SetActive(false);
        emUso[index] = false;
        emRecarga[index] = true;

        yield return new WaitForSeconds(skill.tempoDeRecarga);

        skill.assetVisual.SetActive(true);
        emRecarga[index] = false;
    }

    IEnumerator Piscar(GameObject obj, float tempo)
    {
        float intervalo = 0.2f;
        float tempoTotal = 0f;

        while (tempoTotal < tempo)
        {
            obj.SetActive(!obj.activeSelf);
            yield return new WaitForSeconds(intervalo);
            tempoTotal += intervalo;
        }

        obj.SetActive(false); // invisível até fim da recarga
    }


}
