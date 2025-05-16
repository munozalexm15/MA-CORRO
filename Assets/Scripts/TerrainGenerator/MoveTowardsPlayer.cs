using System;
using UnityEngine;

public class MoveTowardsPlayer : MonoBehaviour
{
    private float initialSpeed = 8f;
    public float speed; // Velocidad de movimiento

    public bool canMove = true;

    private void Start()
    {
        // Asumimos que el jugador tiene la etiqueta "Player"
        speed = initialSpeed;
    }

    private void FixedUpdate()
    {
        if (canMove)
        {
            //GetComponent<Rigidbody>().MovePosition(Vector3.back * speed * Time.deltaTime);
            transform.position += Vector3.back * speed * Time.deltaTime; // Moverse hacia el jugador
        }
    }
}
