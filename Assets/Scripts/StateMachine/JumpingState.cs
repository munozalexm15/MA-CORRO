using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpingState : State
{
    protected override void OnEnter()
    {
        sc.animHandler.CrossFade("Jump", 0.4f,0);
        sc.GetComponent<Rigidbody>().velocity = Vector3.up * 9;
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
        base.OnFixedUpdate();
        if (sc.isGrounded)
        {
            sc.ChangeState(sc.runningState);
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

            // Si el desplazamiento en el eje Y es mayor que en el eje X, es un swipe vertical
            if (Mathf.Abs(swipeVector.y) > Mathf.Abs(swipeVector.x))
            {
                // Detectar si es un swipe vertical (arriba o abajo)
                if (swipeVector.y < -sc._swipeThreshold)
                {
                    //Physics.gravity = new Vector3(0, -30.0F, 0);
                    sc.GetComponent<Rigidbody>().velocity = Vector3.up * -9;
                    sc.ChangeState(sc.fallingState);
                }
            }
            // Si el desplazamiento en el eje X es mayor, es un swipe horizontal (cambiar de carril)
            else
            {
                // Ver si se ha desplazado el dedo en la pantalla en dirección horizontal
                if (swipeVector.x < -sc._swipeThreshold)
                {
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

                if (swipeVector.x > sc._swipeThreshold)
                {
                    if (sc.currentLane.name == "RightLane")
                    {
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
