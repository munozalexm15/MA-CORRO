using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayerFixedRotation : MonoBehaviour
{
    public Transform player;      // El personaje a seguir
    public Vector3 offset = new Vector3(0, 5, -10); // Offset respecto al jugador
    public float smoothSpeed = 10f; // Velocidad de movimiento suave

    void LateUpdate()
    {
        if (player != null)
        {
            // Calculamos la nueva posición de la cámara
            Vector3 desiredPosition = player.position + offset;
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
            transform.position = smoothedPosition;

            // NO rotamos la cámara, mantenemos su rotación original
        }
    }
}