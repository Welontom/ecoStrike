using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CarNavegation : MonoBehaviour
{
    public Transform destino; // ponto final no caminho
    private NavMeshAgent agent;
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        if (destino != null)
        {
            agent.SetDestination(destino.position);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
