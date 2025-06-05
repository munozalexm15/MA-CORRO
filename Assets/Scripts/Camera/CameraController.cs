using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player; // Referencia al jugador

    public Vector3 baseOffset = new Vector3(0, 5, -10); // Offset relativo al jugador
    public float laneChangeSpeed = 10f; // Velocidad del movimiento lateral
    public float verticalFollowSpeed = 5f; // Suavidad para seguir en Y

    [Header("Lane Offset Settings")]
    public float leftOffset = -1f;
    public float centerOffset = 0f;
    public float rightOffset = 1f;

    private float targetXOffset = 0f;
    private float currentYOffset = 0f;
    private Vector3 initialRotation;

    public Shader cameraShader;

    void Start()
    {
        GetComponent<Camera>().SetReplacementShader(cameraShader, "RenderType");
        transform.position = player.position + baseOffset;
        currentYOffset = baseOffset.y;
        initialRotation = transform.eulerAngles;
    }

    void LateUpdate()
    {
        if (!player) return;

        // Calcular el nuevo Y deseado en base a la posición del jugador
        float desiredY = player.position.y + baseOffset.y;
        currentYOffset = Mathf.Lerp(currentYOffset, desiredY, verticalFollowSpeed * Time.deltaTime);

        // Calcular la nueva posición de la cámara
        Vector3 desiredPosition = new Vector3(
            player.position.x + targetXOffset,
            currentYOffset,
            player.position.z + baseOffset.z
        );

        transform.position = Vector3.Lerp(transform.position, desiredPosition, laneChangeSpeed * Time.deltaTime);

        // Mantener la rotación original
        transform.eulerAngles = initialRotation;
    }

    public void ChangeLane(int laneIndex)
    {
        if (laneIndex == -1)
            targetXOffset = leftOffset;
        else if (laneIndex == 0)
            targetXOffset = centerOffset;
        else if (laneIndex == 1)
            targetXOffset = rightOffset;
    }
}
