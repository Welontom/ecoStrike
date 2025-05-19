using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

public class Target : MonoBehaviour
{
    private Rigidbody targetRb;

    // Objeto que define o ponto de spawn (posi��o inicial)
    public GameObject spawn;

    private float minSpeed = 12f;
    private float maxSpeed = 13f;
    private float maxTorque = 180;
    // Start is called before the first frame update
    void Start()
    {
        targetRb = GetComponent<Rigidbody>();

        // Aplica for�a inicial em dire��o aleat�ria para frente
        targetRb.AddForce(RandomForce(), ForceMode.Impulse);

        // Aplica torque (rota��o) aleat�ria nos 3 eixos
        targetRb.AddTorque(RandomTorque(), RandomTorque(), RandomTorque(), ForceMode.Impulse);

        // Define a posi��o inicial do objeto como o ponto de spawn
        transform.position = spawn.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Retorna uma for�a com dire��o levemente aleat�ria para frente
    Vector3 RandomForce()
    {
        float angle = Random.Range(-1f, 1.1f);
        return new Vector3(angle,0.1f,-1) * Random.Range(minSpeed, maxSpeed);
    }

    // Retorna um valor de torque aleat�rio entre -maxTorque e +maxTorque
    float RandomTorque()
    {
        return Random.Range(-maxTorque, maxTorque);
    }

}
