using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlidingState : State
{
    protected override void OnEnter()
    {
        sc.animHandler.CrossFade("Slide", 0.2f,0);
    }

    protected override void OnExit()
    {
        sc.animHandler.CrossFade("Blend Tree", 0.2f,0);
        //throw new System.NotImplementedException();
    }

    protected override void OnUpdate()
    {
        CheckForInput();
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
                            return;
                        }
                    }
                }
            }
        }
    }
}
