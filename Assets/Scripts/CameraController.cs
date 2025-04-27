using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Vector3 basePosition = new Vector3(0, 5, -10); // Posición inicial de la cámara
    public float laneChangeSpeed = 10f; // Velocidad del movimiento lateral

    [Header("Lane Offset Settings")]
    public float leftOffset = -1f;    // Offset cuando el jugador está en el carril izquierdo
    public float centerOffset = 0f;   // Offset cuando está en el centro
    public float rightOffset = 1f;    // Offset cuando está en el carril derecho

    private float targetXOffset = 0f;
    private Vector3 initialRotation;

    void Start()
    {
        transform.position = basePosition;
        initialRotation = transform.eulerAngles; // Guardar rotación inicial
    }

    void LateUpdate()
    {
        // Queremos que solo cambie en X
        Vector3 desiredPosition = basePosition + new Vector3(targetXOffset, 0, 0);

        // Suavemente mover la cámara hacia el objetivo
        transform.position = Vector3.Lerp(transform.position, desiredPosition, laneChangeSpeed * Time.deltaTime);

        // Mantener siempre la misma rotación
        transform.eulerAngles = initialRotation;
    }

    // Función para cambiar de carril
    public void ChangeLane(int laneIndex)
    {
        // laneIndex: -1 = izquierda, 0 = centro, 1 = derecha
        if (laneIndex == -1)
            targetXOffset = leftOffset;
        else if (laneIndex == 0)
            targetXOffset = centerOffset;
        else if (laneIndex == 1)
            targetXOffset = rightOffset;
    }
}
