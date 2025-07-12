using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Referencias")]
    public Rigidbody playerRigidbody;                    // Seguimos al Rigidbody

    [Header("Offsets de Posición")]
    public Vector3 baseOffset = new(0, 5, -10);
    public float verticalFollowSpeed = 5f;

    [Header("Lane Offset Settings")]
    public float leftOffset   = -1f;
    public float centerOffset = 0f;
    public float rightOffset  = 1f;

    private float  targetXOffset = 0f;
    private float  currentYOffset;
    private Vector3 velocity = Vector3.zero;

    [Header("Transición de rotación al iniciar")]        // ★ NUEVO
    [SerializeField] float lookUpInit      = 1.5f;       // cuánto más arriba mira
    [SerializeField] public float startupLerpTime = 0.35f;      // duración de la suavización

    Quaternion rotIni;          // rotación que tiene la cámara en el editor   ★ NUEVO
    float      tweenT;          // progreso 0‑1 de la interpolación            ★ NUEVO
    bool       tweening;        // ¿seguimos interpolando?                     ★ NUEVO

    public bool canMove;
    [Header("Ajustes de rotación")]
    public bool rotationEnabled = true;
    

    /* ───────────────────────────────────────────────────────── */

    void Start()
    {
        currentYOffset = baseOffset.y;

        // guardamos la rotación de arranque tal cual la ves en la escena  ★ NUEVO
        rotIni   = transform.rotation;
        tweenT   = 0f;
        tweening = true;
    }

    void LateUpdate()
    {
        /* ── ROTACIÓN ─────────────────────────────────────────────── */
        if (playerRigidbody && rotationEnabled)   // ← ¡sólo si está activada!
        {
            Vector3 lookTarget = playerRigidbody.position + Vector3.up * lookUpInit;
            Quaternion rotTarget =
                Quaternion.LookRotation(lookTarget - transform.position, Vector3.up);

            if (tweening)
            {
                tweenT += Time.deltaTime / startupLerpTime;
                transform.rotation = Quaternion.Slerp(rotIni, rotTarget, tweenT);

                if (tweenT >= 1f)
                {
                    tweening = false;
                    transform.rotation = rotTarget;
                }
            }
            else
            {
                transform.rotation = rotTarget;
            }
        }
        /* Si rotationEnabled == false no tocamos la rotación actual */

        /* ── POSICIÓN ─────────────────────────────────────────────── */
        if (canMove && playerRigidbody)
            UpdateCamera();
    }

    /* ───────────────────────────────────────────────────────── */

    public void ChangeLane(int laneIndex)
    {
        targetXOffset = laneIndex switch          // sintaxis compacta C# 8+
        {
            -1 => leftOffset,
            0  => centerOffset,
            1  => rightOffset,
            _  => targetXOffset
        };

        velocity = Vector3.zero; // evita tirones bruscos
    }

    void UpdateCamera()
    {
        // Y (altura) sigue suavemente
        float desiredY = playerRigidbody.position.y + baseOffset.y;
        currentYOffset = Mathf.Lerp(currentYOffset, desiredY, verticalFollowSpeed * Time.deltaTime);

        // Posición deseada completa
        Vector3 desiredPosition = new(
            playerRigidbody.position.x + targetXOffset,
            currentYOffset,
            playerRigidbody.position.z + baseOffset.z);

        // Movimiento suave con SmoothDamp
        transform.position = Vector3.SmoothDamp(transform.position, desiredPosition,
                                                ref velocity, 0.1f);
    }
}
