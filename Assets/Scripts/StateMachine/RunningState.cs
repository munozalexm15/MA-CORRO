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
    }

    protected override void OnExit()
    {
        //throw new System.NotImplementedException();
    }

    protected override void OnUpdate()
    {
        CheckForInput();

        float RunBlendProgress = sc.animHandler.GetFloat("RunBlendProgress");
        float SpeedMultiplier = sc.animHandler.GetFloat("SpeedMultiplier");
        if (RunBlendProgress < 1) {
            sc.animHandler.SetFloat("SpeedMultiplier", SpeedMultiplier + 0.00003f);
            sc.animHandler.SetFloat("RunBlendProgress", RunBlendProgress + 0.0001f);
        }
        
        
        //throw new System.NotImplementedException();
    }

    protected void CheckForInput() {

        if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Began) {
            sc.startTouchPos = Input.GetTouch(0).position;
        }

        if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Ended) {
            sc.endTouchPos = Input.GetTouch(0).position;

            Vector2 swipeVector = sc.endTouchPos - sc.startTouchPos;
            
            // Si el desplazamiento en el eje Y es mayor que en el eje X, es un swipe vertical
            if (Mathf.Abs(swipeVector.y) > Mathf.Abs(swipeVector.x)) {
                // Detectar si es un swipe vertical (arriba o abajo)
                if (swipeVector.y < -sc._swipeThreshold) {
                    sc.ChangeState(sc.slidingState);
                }
                if (swipeVector.y > sc._swipeThreshold) {
                    sc.ChangeState(sc.jumpingState);
                }
            }
            // Si el desplazamiento en el eje X es mayor, es un swipe horizontal (cambiar de carril)
            else {
                // Ver si se ha desplazado el dedo en la pantalla en dirección horizontal
                if (swipeVector.x < -sc._swipeThreshold) {
                    if (sc.currentLane.name == "LeftLane") {
                        return;
                    }

                    for (int i = 0; i < sc.lanes.Count; i++) {
                        if (sc.lanes[i].name == sc.currentLane.name) {
                            sc.currentLane = sc.lanes[i - 1];
                            sc.IsChangingLane = true;
                            sc.MoveCameraToLane();
                            return;
                        }
                    }
                }

                if (swipeVector.x > sc._swipeThreshold) {
                    if (sc.currentLane.name == "RightLane") {
                        return;
                    }

                    for (int i = 0; i < sc.lanes.Count; i++) {
                        if (sc.lanes[i].name == sc.currentLane.name) {
                            sc.currentLane = sc.lanes[i + 1];
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
