using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    public ObjectPool objPool;
    public StateController stateController;

    public GameObject MainMenuUI;
    private Button UIButton;


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
        UIButton = MainMenuUI.transform.Find("RunButton").GetComponent<Button>();
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

        //isLerping = true;
        elapsed = 0f;
        startPos = currentPos.position;
        startRot = currentPos.rotation;
        cam.GetComponent<CameraOrbit>().BeginOrbit();
        stateController.ChangeState(stateController.runningState);
        MainMenuUI.transform.Find("Top").GetComponent<Animator>().Play("FadeOutTopAnim");
        MainMenuUI.transform.Find("Bottom").GetComponent<Animator>().Play("FadeOutBottomAnim");
    }
}
