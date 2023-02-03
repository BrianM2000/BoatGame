using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonHolder : MonoBehaviour
{
    GameObject cannonObj;
    Cannon cannon;
    GameObject modelObj;
    CannonModel model;
    // Start is called before the first frame update
    void Start()
    {
        cannonObj = transform.GetChild(0).gameObject;
        cannon = cannonObj.GetComponent(typeof (Cannon)) as Cannon;
        modelObj = transform.GetChild(1).gameObject;
        model = modelObj.GetComponent(typeof (CannonModel)) as CannonModel;
    }

    // Update is called once per frame
    void Update()
    {
        if(cannonObj.transform.localEulerAngles.y < 135f && cannonObj.transform.localEulerAngles.y > 45f){
            model.Rotate(cannon.cannonDirection, cannon.rotateSpeed);
        }
    }
}
