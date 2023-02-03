using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonModel : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Rotate(Vector3 cannonDirection, float rotateSpeed){
        //Debug.DrawRay(transform.position, cannonDirection, Color.red);
        transform.up = Vector3.RotateTowards(transform.up, cannonDirection, rotateSpeed * Time.deltaTime, 100);;
    }
}
