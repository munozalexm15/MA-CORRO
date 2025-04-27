using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefeatedState : State
{
    protected override void OnEnter()
    {

        sc.animHandler.CrossFade("Knocked Down", 0.4f,0);
        sc.objPoolManager.StopMap();
        sc.cameraShakeEffect.Shake(0.2f, 0.3f);
        sc.cameraShakeEffect.originalPos = sc.cameraShakeEffect.transform.localPosition;
        sc.cameraShakeEffect.canShake = true;
    }

    protected override void OnExit()
    {
        //throw new System.NotImplementedException();
    }

    protected override void OnUpdate()
    {
        //throw new System.NotImplementedException();
    }
}
