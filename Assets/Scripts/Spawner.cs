using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public List<GameObject> targets; 
    public float intervalo = 2f;        // intervalo entre lançamentos
    public bool isGameActive;
    public Transform containerDeSpawnados;

    // Start is called before the first frame update
    void Start()
    {
        isGameActive = false; // começa como falso
    }

    public void IniciarSpawn()
    {
        isGameActive = true;
        StartCoroutine(SpawnTarget());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator SpawnTarget()
    {
        while (isGameActive)
        {
            yield return new WaitForSeconds(intervalo);
            int index = Random.Range(0, targets.Count);
            Instantiate(targets[index], containerDeSpawnados);
        }
    }
}
