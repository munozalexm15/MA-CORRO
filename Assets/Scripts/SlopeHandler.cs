using UnityEngine;

public class SlopeHandler : MonoBehaviour
{
    private Vector3 platformDirection = Vector3.zero;
    private float platformSpeed = 0f;
    private float initialZPosition;
    private Quaternion originalRotation;
    private Quaternion targetRotation;
    private Transform currentSlope;

    private int framesWithoutSlope = 0;
    public int maxFramesWithoutSlope = 5;

    public float raycastDistance = 1.5f;
    public float rotationSmoothSpeed = 5f; // Velocidad de suavizado

    void Start()
    {
        initialZPosition = transform.position.z;
        originalRotation = transform.rotation;
        targetRotation = originalRotation;
    }

    void FixedUpdate()
    {
        Ray ray = new Ray(transform.position + Vector3.up * 0.1f, Vector3.down);
        RaycastHit hit;
        
        if (Physics.Raycast(ray, out hit, raycastDistance))
        {
            if (hit.collider.CompareTag("Slope"))
            {
                framesWithoutSlope = 0;

                if (currentSlope != hit.collider.transform)
                {
                    currentSlope = hit.collider.transform;

                   Vector3 slopeNormal = hit.normal;

                    // Proyectar la normal sobre el plano ZY (para aislar inclinación hacia adelante/atrás)
                    Vector3 forwardProjection = Vector3.ProjectOnPlane(slopeNormal, transform.right);

                    // Obtener el ángulo entre el 'up' y esa proyección
                    float angle = Vector3.SignedAngle(Vector3.up, forwardProjection, transform.right);

                    // Amplificar el ángulo si se desea
                    angle *= 2.5f; // Cambia esto a gusto: 1.0 = igual que la rampa, >1.0 = más inclinación

                    // Crear una rotación solo en el eje X
                    targetRotation = Quaternion.Euler(angle, originalRotation.eulerAngles.y, originalRotation.eulerAngles.z);


                    platformDirection = -currentSlope.forward;

                    MoveTowardsPlayer mp = currentSlope.GetComponent<MoveTowardsPlayer>();
                    platformSpeed = mp != null ? mp.speed : 5f;
                }
            }
            else
            {
                framesWithoutSlope++;
            }
        }
        else
        {
            framesWithoutSlope++;
        }

        if (framesWithoutSlope >= maxFramesWithoutSlope)
        {
            ResetSlope();
        }

        // Aplicar rotación suavemente
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.fixedDeltaTime * rotationSmoothSpeed);

        if (currentSlope != null || GetComponent<StateController>().isGrounded)
        {
            Vector3 newPosition = transform.position + platformDirection * platformSpeed * Time.fixedDeltaTime;
            newPosition.z = initialZPosition;
            transform.position = newPosition;
        }
    }

    private void ResetSlope()
    {
        if (currentSlope != null)
        {
            currentSlope = null;
            platformDirection = Vector3.zero;
            platformSpeed = 0f;
            targetRotation = originalRotation;
        }
    }
}
