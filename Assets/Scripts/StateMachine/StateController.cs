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


    [HideInInspector]
    public Vector2 startTouchPos = new();
    [HideInInspector]
    public Vector2 endTouchPos = new();
    [HideInInspector]
    public float _swipeThreshold = 50f;

//GameObject Variables / Components
    public Animator animHandler;

    //Lane control variables
    [HideInInspector]
    public bool IsChangingLane = false;
    [HideInInspector]
    public bool isRotating = false;

    [Header("Lanes Settings")]
    public List<GameObject> lanes;
    public GameObject currentLane;
    public Transform PlayerParentTransform;

    //Camera control variables
    [Header("Camera Settings")]
    public CameraController cameraController;
    public CameraShakeEffect cameraShakeEffect;

    //Object Pool manager variables
    [Header("Object Pooling")]
    public ObjectPool objPoolManager;

    //Collision triggers variables
    [HideInInspector]
    public Vector3 allowedDirection = Vector3.forward;

    //Falling state variables
    [Header("Ground Checking")]
    [HideInInspector]
    public bool isGrounded;
    public LayerMask groundLayer;
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;

    [HideInInspector]
    public BoxCollider playerCollider;

    [HideInInspector]
    public Vector3 originalSize;
    public Vector3 originalCenter;

    private Rigidbody rb;

    public float maxLaneChangeDuration = 2f; // segundos
    private float laneChangeTimer = 0f;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        cameraController.playerRigidbody = rb;

        playerCollider = GetComponent<BoxCollider>();
        originalCenter = playerCollider.center;
        originalSize = playerCollider.size;
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

        if (currentState == defeatedState)
        {
            return;
        }
    }

    void FixedUpdate()
    {
        if (currentState != null)
        {
            if (currentState == defeatedState)
            {
                return;
            }
            currentState.OnStateFixedUpdate();
        }
        if (IsChangingLane)
        {
            isRotating = true;
            MoveToLane();
        }
        if (isRotating) {
            RotatePlayer();
        }
    }

    public void ChangeState(State newState)
    {
        if (currentState != null)
        {
            currentState.OnStateExit();
        };
        previousState = currentState;
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

    public void RotatePlayer()
    {
        Vector3 targetLookAt = new Vector3(currentLane.transform.position.x, transform.position.y, currentLane.transform.position.z + 1f);
        Vector3 direction = targetLookAt - transform.position;
        direction.y = 0;
        if (direction.sqrMagnitude > 1.4f)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.fixedDeltaTime * 20f);
        }
        else
        {
            isRotating = false;
        }
    }

    public void StartLaneChange()
    {
        laneChangeTimer = 0f;
        IsChangingLane = true;
    }

    public void MoveToLane()
    {
        float targetX = currentLane.transform.position.x;
        float deltaX = targetX - rb.position.x;

        if (Mathf.Abs(deltaX) > 0.14f)
        {
            float direction = Mathf.Sign(deltaX);

            Vector3 newVelocity = rb.velocity;
            newVelocity.x = direction * 14f; // brusco y rápido
            rb.velocity = newVelocity;
        }
        else
        {
            // 👇 Forzamos el centrado para eliminar el jitter
            rb.position = new Vector3(targetX, rb.position.y, rb.position.z);

            Vector3 newVelocity = rb.velocity;
            newVelocity.x = 0f;
            rb.velocity = newVelocity;

            IsChangingLane = false;
            isRotating = false;
        }
    }




    public void MoveCameraToLane() {
        if (currentLane.name == "CentralLane")
        {
            cameraController.ChangeLane(0);
        }
        else if (currentLane.name == "LeftLane")
        {
            cameraController.ChangeLane(-1);
        }
        else
        {
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