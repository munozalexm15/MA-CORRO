using UnityEngine;

public class MoveTowardsPlayer : MonoBehaviour
{
    public float speed = 5f; // Velocidad de movimiento
    private Transform playerTransform;

    private void Start()
    {
        // Asumimos que el jugador tiene la etiqueta "Player"
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        if (playerTransform != null)
        {
            transform.position += Vector3.back * speed * Time.deltaTime; // Moverse hacia el jugador
        }
    }
}
