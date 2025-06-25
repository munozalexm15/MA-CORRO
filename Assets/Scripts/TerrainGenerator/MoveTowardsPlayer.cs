using System;
using UnityEngine;

public class MoveTowardsPlayer : MonoBehaviour
{
    private float initialSpeed = 8f;
    public float speed; // Velocidad de movimiento

    public bool canMove;

    private void Awake()
    {
        speed = initialSpeed;
        canMove = false;
    }

    private void FixedUpdate()
    {
        if (canMove)
        {
            transform.position += Vector3.back * speed * Time.fixedDeltaTime;
        }
    }
}
