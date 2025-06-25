using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Rigidbody playerRigidbody; // ← Ahora seguimos el Rigidbody, no el Transform

    public Vector3 baseOffset = new Vector3(0, 5, -10);
    public float verticalFollowSpeed = 5f;

    [Header("Lane Offset Settings")]
    public float leftOffset = -1f;
    public float centerOffset = 0f;
    public float rightOffset = 1f;

    private float targetXOffset = 0f;
    private float currentYOffset = 0f;
    private Vector3 initialRotation;
    private Vector3 velocity = Vector3.zero;

    public bool canMove;

    void Start()
    {
        //transform.position = playerRigidbody.position + baseOffset;
        currentYOffset = baseOffset.y;
        initialRotation = transform.eulerAngles;
    }

    void LateUpdate() // ← ¡Movemos la cámara en LateUpdate ahora!
    {
        if (canMove)
        {
            UpdateCamera();
        }
        
    }

    public void ChangeLane(int laneIndex)
    {
        if (laneIndex == -1)
            targetXOffset = leftOffset;
        else if (laneIndex == 0)
            targetXOffset = centerOffset;
        else if (laneIndex == 1)
            targetXOffset = rightOffset;

        velocity = Vector3.zero; // Resetea la velocidad para evitar tirones bruscos
    }

    void UpdateCamera()
    {
        if (!playerRigidbody) return;

        // Y sigue suavemente
        float desiredY = playerRigidbody.position.y + baseOffset.y;
        currentYOffset = Mathf.Lerp(currentYOffset, desiredY, verticalFollowSpeed * Time.deltaTime);

        // Calcula la nueva posición deseada de la cámara
        Vector3 desiredPosition = new Vector3(
            playerRigidbody.position.x + targetXOffset,
            currentYOffset,
            playerRigidbody.position.z + baseOffset.z
        );

        // Movimiento suave con SmoothDamp
        transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, 0.1f);

        // Fijar rotación si lo deseas
        //transform.eulerAngles = initialRotation;
    }
}
