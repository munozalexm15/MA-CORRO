using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicConfigController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Time.fixedDeltaTime = 1f / 60f;
        Application.targetFrameRate = 60;
        //QualitySettings.vSyncCount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
