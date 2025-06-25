using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

public class RunningState : State
{
    protected override void OnEnter()
    {
        //throw new System.NotImplementedException();
        if (sc.previousState == sc.fallingState && sc.GetComponent<Rigidbody>().velocity.y <= -8)
        {
            sc.transform.GetComponent<CharacterVFXManager>().GroundPoundVFX.Play();
        }
    }

    protected override void OnExit()
    {
        //throw new System.NotImplementedException();
    }

    protected override void OnUpdate()
    {
        CheckForInput();
    }

    protected override void OnFixedUpdate()
    {

        Rigidbody rb = sc.GetComponent<Rigidbody>();

        if (!sc.isGrounded)
        {
            if (rb.velocity.y > 0)
            {
                sc.ChangeState(sc.jumpingState);
            }
            else if (rb.velocity.y < 0)
            {
                sc.ChangeState(sc.fallingState);
            }
}
        //throw new System.NotImplementedException();

        float RunBlendProgress = sc.animHandler.GetFloat("RunBlendProgress");
        float SpeedMultiplier = sc.animHandler.GetFloat("SpeedMultiplier");
        if (RunBlendProgress < 1)
        {
            sc.animHandler.SetFloat("SpeedMultiplier", SpeedMultiplier + 0.00003f);
            sc.animHandler.SetFloat("RunBlendProgress", RunBlendProgress + 0.0001f);
            sc.objPoolManager.UpdatePlatformSpeed(0.002f);
        }

    }

    protected void CheckForInput()
    {

        if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            sc.startTouchPos = Input.GetTouch(0).position;
        }

        if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Ended)
        {
            sc.endTouchPos = Input.GetTouch(0).position;

            Vector2 swipeVector = sc.endTouchPos - sc.startTouchPos;

            // Swipe vertical
            if (Mathf.Abs(swipeVector.y) > Mathf.Abs(swipeVector.x))
            {
                if (swipeVector.y < -sc._swipeThreshold)
                {
                    sc.ChangeState(sc.slidingState);
                }
                if (swipeVector.y > sc._swipeThreshold)
                {
                    sc.ChangeState(sc.jumpingState);
                }
            }
            // Swipe horizontal
            else
            {
                Vector3 rayOrigin = sc.transform.position;
                float rayDistance = 2f; // Ajusta la distancia si es necesario
                Vector3 direction = Vector3.zero;

                if (swipeVector.x < -sc._swipeThreshold)
                {
                    direction = Vector3.left;

                    // Verificar si hay una pared a la izquierda
                    if (Physics.Raycast(rayOrigin, direction, rayDistance, LayerMask.GetMask("Wall")))
                    {
                        return;
                    }

                    if (sc.currentLane.name == "LeftLane") return;

                    for (int i = 0; i < sc.lanes.Count; i++)
                    {
                        if (sc.lanes[i].name == sc.currentLane.name)
                        {
                            sc.currentLane = sc.lanes[i - 1];
                            sc.StartLaneChange();
                            sc.IsChangingLane = true;
                            sc.StartLaneChange();
                            sc.MoveCameraToLane();
                            return;
                        }
                    }
                }

                if (swipeVector.x > sc._swipeThreshold)
                {
                    direction = Vector3.right;

                    // Verificar si hay una pared a la derecha
                    if (Physics.Raycast(rayOrigin, direction, rayDistance, LayerMask.GetMask("Wall")))
                    {
                        return;
                    }

                    if (sc.currentLane.name == "RightLane") return;

                    for (int i = 0; i < sc.lanes.Count; i++)
                    {
                        if (sc.lanes[i].name == sc.currentLane.name)
                        {
                            sc.currentLane = sc.lanes[i + 1];
                            sc.StartLaneChange();
                            sc.IsChangingLane = true;
                            
                            sc.MoveCameraToLane();
                            return;
                        }
                    }
                }
            }
        }
    }
}
