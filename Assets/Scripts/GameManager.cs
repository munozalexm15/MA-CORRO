using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public ObjectPool objectPool; // Referencia al Object Pool
    public float spawnRange = 10f; // Rango de aparición de los objetos

    public GameObject LatestSpawnedObject;

    private void Start()
    {
        //nada
    }

    // Método para activar un objeto cuando el jugador pase por el trigger
    public void ActivateObjectInTrigger(Collider other)
    {
        if (other.CompareTag("Player")) // Si el objeto que entra es el jugador
        {
            // Obtener un objeto del pool
            GameObject obj = objectPool.GetObject();
            
            if (obj != null)
            {
                // Posicionar el objeto en el trigger
                obj.transform.position = transform.position;

                // Activar el movimiento hacia el jugador
                obj.GetComponent<MoveTowardsPlayer>().enabled = true;

                // Guardamos el objeto para devolverlo cuando el jugador pase por el trigger
                obj.GetComponentInChildren<TriggerArea>().gameManager = this;
            }
        }
    }
}
