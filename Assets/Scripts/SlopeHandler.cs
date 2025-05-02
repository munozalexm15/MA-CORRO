using UnityEngine;

public class PlatformFollower : MonoBehaviour
{
    private bool onSlope = false;
    private Vector3 platformDirection = Vector3.zero;
    private float platformSpeed = 0f;
    private float initialZPosition;

    void Start()
    {
        // Guarda la posición Z inicial del jugador al comienzo.
        initialZPosition = transform.position.z;
    }

    void Update()
    {
        if (onSlope || GetComponent<StateController>().isGrounded)
        {
            // Movemos al jugador pero mantenemos la posición Z constante.
            Vector3 newPosition = transform.position + platformDirection * platformSpeed * Time.deltaTime;
            newPosition.z = initialZPosition;  // Fijamos Z a la posición inicial.

            transform.position = newPosition;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Slope"))
        {
            onSlope = true;

            // Dirección contraria al movimiento para simular que sube con la rampa
            platformDirection = -other.transform.forward;

            // Si usas un script para mover la plataforma, puedes leer la velocidad
            MoveTowardsPlayer mp = other.GetComponent<MoveTowardsPlayer>();
            platformSpeed = mp != null ? mp.speed : 5f; // valor por defecto
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Slope"))
        {
            onSlope = false;
            platformDirection = Vector3.zero;
            platformSpeed = 0f;
        }
    }
}
