using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class StateController : MonoBehaviour
{
    State currentState;

/// <summary>
/// The states that the player will move between. Running (Idle), Sliding and Jumping. More to come
/// </summary>
    public RunningState runningState = new RunningState();
    public SlidingState slidingState = new SlidingState();
    public JumpingState jumpingState = new JumpingState();
    public DefeatedState defeatedState = new DefeatedState();

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
    

    private void Start()
    {
        animHandler = GetComponent<Animator>();
        ChangeState(runningState);
    }

    void Update()
    {
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

    public void ChangeState(State newState)
    {
        if (currentState != null)
        {
            currentState.OnStateExit();
        }
        currentState = newState;
        currentState.OnStateEnter(this);
    }

    public void OnJumpEnded() {
        ChangeState(runningState);
        
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
        Vector3 targetPosition = new Vector3(currentLane.GetComponent<Transform>().position.x, transform.position.y, currentLane.GetComponent<Transform>().position.z);

        // Calcular la distancia ignorando diferencias en Y
        float distance = Vector3.Distance(transform.position, targetPosition);

        if (distance > 0.1f)
        {
            // Moverse hacia el objetivo sin cambiar la altura (Y)
            Vector3 direction = (targetPosition - transform.position).normalized;
            transform.position += direction * 10 * Time.deltaTime;
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Obstacle") && currentState != defeatedState) {
            ChangeState(defeatedState);
        }
    }
}