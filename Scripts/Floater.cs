using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floater : MonoBehaviour
{
    Rigidbody rb;
    float waterLine = 0;
    public List<Bouy> bouys;
    bool underwater = false;
    float drag;
    float angularDrag;
    float underwaterDrag = 3f;
    float underwaterAngularDrag = 1f;
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        drag = rb.drag;
        angularDrag = rb.angularDrag;
    }

    // Update is called once per frame
    void Update()
    {
        foreach(Bouy b in bouys){
            Transform bt = b.transform;
            float difference = (bt.position.y - waterLine);
            if(difference < 0){
                rb.AddForceAtPosition(Vector3.up * b.bouancy * Mathf.Abs(difference), bt.position, ForceMode.Force);

                if(!underwater){
                    underwater = true;
                    rb.drag = underwaterDrag;
                    rb.angularDrag = underwaterAngularDrag;
                }
            
            }

            else if(underwater){
                underwater = false;
                rb.drag = drag;
                rb.angularDrag = angularDrag;
            }

        }
    }

}
