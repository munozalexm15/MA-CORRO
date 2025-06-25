using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    public ObjectPool objPool;
    public StateController stateController;
    public Button UIButton;

    [Header("Positions")]
    public Transform targetPos;
    public Transform currentPos;
    public Transform PlayerPos;

    public Camera cam;

    [Header("Settings")]
    public float duration = 1.25f;
    [Range(1f, 20f)]
    public float rotSmooth = 10f;
   

    bool isLerping;
    float elapsed;
    Vector3 startPos;
    Quaternion startRot;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (isLerping)
        {
           elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, 57f, t);

            // Lerp de posición
            cam.transform.position = Vector3.Lerp(startPos, targetPos.position, t);

            // Calcular rotación para mirar al jugador
            if (PlayerPos != null)
            {
                Quaternion lookRot = Quaternion.LookRotation(PlayerPos.position -  cam.transform.position);
                // Suavizamos la rotación hacia el jugador
                cam.transform.rotation = Quaternion.Slerp( cam.transform.rotation, lookRot, Time.deltaTime * rotSmooth);
            }

            if (t >= 1f)
            {
                isLerping = false;
                cam.GetComponent<CameraController>().canMove = true;
            }
        }
    }

    public void StartRace()
    {
        foreach (GameObject pooled in objPool.objectPool)
        {
            MoveTowardsPlayer mover = pooled.GetComponent<MoveTowardsPlayer>();
            //Debug.Log(mover);
            if (mover != null)
            {
                mover.canMove = true;
            }
        }

        foreach (GameObject obj in objPool.activeObjects)
        {
            if (!obj) continue;

            MoveTowardsPlayer mover = obj.GetComponent<MoveTowardsPlayer>();
            if (mover != null)
            {
                mover.canMove = true;
            }
        }
        UIButton.gameObject.SetActive(false);

        stateController.animHandler.CrossFade("Blend Tree", 0.05f, 0);

        isLerping = true;
        elapsed = 0f;
        startPos = currentPos.position;
        startRot = currentPos.rotation;
        stateController.ChangeState(stateController.runningState);
    }
}
