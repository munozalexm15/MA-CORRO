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

    void FixedUpdate()
    {
        UpdateCamera();
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

    private Vector3 velocity = Vector3.zero;

    void UpdateCamera()
    {
        if (!player) return;

        float desiredY = player.position.y + baseOffset.y;
        currentYOffset = Mathf.Lerp(currentYOffset, desiredY, verticalFollowSpeed * Time.fixedDeltaTime);

        Vector3 desiredPosition = new Vector3(
            player.position.x + targetXOffset,
            currentYOffset,
            player.position.z + baseOffset.z
        );

        // Movimiento más suave con aceleración
        transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, 0.1f);

        transform.eulerAngles = initialRotation;
    }


}
