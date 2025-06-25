using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    public ObjectPool objPool;
    public StateController stateController;

    public Button UIButton;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

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
    }
}
