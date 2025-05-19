using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    // Lista de prefabs que podem ser instanciados
    public List<GameObject> targets;

    // Intervalo de tempo entre cada spawn
    public float intervalo = 2f;
    

    public bool isGameActive;

    // Objeto que será o pai dos objetos instanciados (organização na hierarquia)
    public Transform containerDeSpawnados;

    // Start is called before the first frame update
    void Start()
    {
        // O jogo começa desativado
        isGameActive = false;
    }

    // Método público para iniciar o sistema de spawn
    public void IniciarSpawn()
    {
        isGameActive = true;
        StartCoroutine(SpawnTarget());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Coroutine que instancia alvos enquanto o jogo estiver ativo
    IEnumerator SpawnTarget()
    {
        while (isGameActive)
        {
            // Espera pelo intervalo definido
            yield return new WaitForSeconds(intervalo);

            // Seleciona um alvo aleatório da lista
            int index = Random.Range(0, targets.Count);

            // Instancia o alvo como filho do container
            Instantiate(targets[index], containerDeSpawnados);
        }
    }
}
