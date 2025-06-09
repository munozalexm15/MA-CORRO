using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SlidingState : State
{

    protected override void OnEnter()
    {
        sc.animHandler.CrossFade("Slide", 0.2f, 0);
        sc.playerCollider.size -= new Vector3(0, sc.playerCollider.size.y / 1.5f, 0);
        sc.playerCollider.center -= new Vector3(0, sc.playerCollider.center.y / 1.5f, 0);
    }

    protected override void OnExit()
    {
        sc.animHandler.CrossFade("Blend Tree", 0.2f, 0);
        Physics.gravity = new Vector3(0, -15.0F, 0);
        sc.playerCollider.size = sc.originalSize;
        sc.playerCollider.center = sc.originalCenter;
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
                // Detectar si es un swipe hacia arriba y el personaje no está saltando 
                if (swipeVector.y > sc._swipeThreshold && sc.isGrounded)
                {
                    sc.ChangeState(sc.jumpingState);
                }
            }
            // Si el desplazamiento en el eje X es mayor, es un swipe horizontal (cambiar de carril)
            else {
                // Ver si se ha desplazado el dedo en la pantalla en dirección horizontal
                
                 Vector3 rayOrigin = sc.transform.position; 
                float rayDistance = 2f;                     
                Vector3 direction = Vector3.zero;           
                if (swipeVector.x < -sc._swipeThreshold)
                {
                    direction = Vector3.left;
                    if (Physics.Raycast(rayOrigin, direction, rayDistance, LayerMask.GetMask("Wall"))) {    
                        return;                                                                          
                    }

                    if (sc.currentLane.name == "LeftLane")
                    {
                        return;
                    }

                    for (int i = 0; i < sc.lanes.Count; i++)
                    {
                        if (sc.lanes[i].name == sc.currentLane.name)
                        {
                            sc.currentLane = sc.lanes[i - 1];
                            sc.StartLaneChange();
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

                    direction = Vector3.right;           
                    // Verificar si hay una pared a la derecha
                    if (Physics.Raycast(rayOrigin, direction, rayDistance, LayerMask.GetMask("Wall"))) { 
                        return;                                                                         
                    }

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
