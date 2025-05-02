using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class StateController : MonoBehaviour
{
    public State currentState;
    public State previousState;

/// <summary>
/// The states that the player will move between. Running (Idle), Sliding, jumping, falling and defeated
/// </summary>
    public RunningState runningState = new RunningState();
    public SlidingState slidingState = new SlidingState();
    public JumpingState jumpingState = new JumpingState();
    public DefeatedState defeatedState = new DefeatedState();
    public FallingState fallingState = new FallingState();

/// <summary>
/// Swipe Variables
/// </summary>
    public Vector2 startTouchPos = new();
    public Vector2 endTouchPos = new();
    public float _swipeThreshold  = 50f;

//GameObject Variables / Components
    public Animator animHandler;

//Lane control variables
    public bool IsChangingLane = false;
    private bool isRotating = false;

    public List<GameObject> lanes;
    public GameObject currentLane;
    public Transform PlayerParentTransform;

//Camera control variables
    public CameraController cameraController;
    public CameraShakeEffect cameraShakeEffect;

//Object Pool manager variables
    public ObjectPool objPoolManager;

//Collision triggers variables
    public Vector3 allowedDirection = Vector3.forward;
    
//Falling state variables
    public bool isGrounded;
    public LayerMask groundLayer;
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;

    private void Start()
    {
        Physics.gravity = new Vector3(0, -15.0F, 0);
        animHandler = GetComponent<Animator>();
        ChangeState(runningState);
    }

    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundLayer);
        if (currentState != null)
        {
            currentState.OnStateUpdate();
        }

         if (IsChangingLane) {
            isRotating = true;
            MoveToLane();
        }
        if (isRotating) {
            RotatePlayer();
        }
    }

    void FixedUpdate()
    {
        if (currentState != null) {
            currentState.OnStateFixedUpdate();
        }
    }

    public void ChangeState(State newState)
    {
        if (currentState != null)
        {
            currentState.OnStateExit();
        };
        currentState = newState;
        currentState.OnStateEnter(this);
    }

    public void OnJumpEnded() {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundLayer);
        if (!isGrounded)
        {
            ChangeState(fallingState);
        }
        else {
            ChangeState(runningState);
        }
    }

    public void RotatePlayer() {

        Vector3 targetLookAt = new(currentLane.GetComponent<Transform>().position.x, 1, currentLane.GetComponent<Transform>().position.z + 1);

    // Mientras mira al objetivo
        Vector3 direction = targetLookAt - transform.position;
        direction.y = 0; // Solo rotación horizontal

        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 20);
        }
    }

   public void MoveToLane() {
        // Clonar la posición del target pero con el mismo Y que el personaje
        Vector3 targetPosition = new Vector3(currentLane.GetComponent<Transform>().position.x, transform.position.y, transform.position.z);

        // Calcular la distancia ignorando diferencias en Y
        float distance = Vector3.Distance(new Vector3(transform.position.x, 0f, transform.position.z), new Vector3(targetPosition.x, 0f, transform.position.z));

        if (distance > 0.2f) // Aumenta el umbral para evitar movimientos innecesarios
        {
            // Moverse solo en el eje X sin cambiar la altura (Y) ni Z
            Vector3 direction = (targetPosition - transform.position).normalized;
            direction.y = 0;  // Evitar cualquier movimiento vertical
            direction.z = 0;  // Evitar cualquier movimiento en Z
            transform.position += direction * 15 * Time.deltaTime;
        }
        else {
            // Asegurarse de que la posición esté exactamente en la del objetivo
            transform.position = new Vector3(targetPosition.x, transform.position.y, transform.position.z);
            IsChangingLane = false;
        }
    }



    public void MoveCameraToLane() {
        if (currentLane.name == "CentralLane") {
            cameraController.ChangeLane(0);
        }
        else if (currentLane.name == "LeftLane") {
            cameraController.ChangeLane(-1);
        }
        else {
            cameraController.ChangeLane(1);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        
        // Calcula la dirección desde la que viene el objeto (desde el centro del colisionador hacia el objeto que colisiona)
        Vector3 direction = (collision.transform.position - transform.position).normalized;

        // Convierte la dirección permitida a espacio global
        Vector3 worldAllowedDirection = transform.TransformDirection(allowedDirection).normalized;

        // Calcula el ángulo con un dot product
        float dot = Vector3.Dot(direction, worldAllowedDirection);

        if (dot > 0.7f) // Ajusta el umbral según tu necesidad (0.7f ≈ 45 grados)
        {
            // Aquí va tu lógica para colisiones válidas
            if (collision.gameObject.CompareTag("Obstacle") && currentState != defeatedState) {
                ChangeState(defeatedState);
            }
        }
        else
        {
            // Opcional: anular efectos, rebote, etc.
        }
    }


    
    
}