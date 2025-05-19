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

    // Objeto que ser� o pai dos objetos instanciados (organiza��o na hierarquia)
    public Transform containerDeSpawnados;

    // Start is called before the first frame update
    void Start()
    {
        // O jogo come�a desativado
        isGameActive = false;
    }

    // M�todo p�blico para iniciar o sistema de spawn
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

            // Seleciona um alvo aleat�rio da lista
            int index = Random.Range(0, targets.Count);

            // Instancia o alvo como filho do container
            Instantiate(targets[index], containerDeSpawnados);
        }
    }
}
