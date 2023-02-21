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
    GameObject ocean;
    Material oceanMat;
    float frequency1;
    float speed1;
    float height1;
    float frequency2;
    float speed2;
    float height2;
    float matX;
    float matZ;
    Vector4 wave1;
    Vector4 wave2;
    Vector4 wave3;

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        drag = rb.drag;
        angularDrag = rb.angularDrag;

        ocean = GameObject.Find("Ocean");
        oceanMat = ocean.GetComponent<Renderer>().sharedMaterial;
        
        /*
        frequency1 = oceanMat.GetFloat("_Frequency1");
        speed1 = oceanMat.GetFloat("_Speed1");
        height1 = oceanMat.GetFloat("_Height1");
        frequency2 = oceanMat.GetFloat("_Frequency2");
        speed2 = oceanMat.GetFloat("_Speed2");
        height2 = oceanMat.GetFloat("_Height2");
        */

        wave1 = oceanMat.GetVector("_W1_Freq_Amp_Speed_Rot");
        wave2 = oceanMat.GetVector("_W2_Freq_Amp_Speed_Rot");
        wave3 = oceanMat.GetVector("_W3_Freq_Amp_Speed_Rot");

        matX = ocean.transform.position.x;
        matZ = ocean.transform.position.z;

    }

    // Update is called once per frame
    void Update()
    {
        
        foreach(Bouy b in bouys){
            Transform bt = b.transform;

            float time = oceanMat.GetFloat("_UnityTime");
            float realX = bt.position.x - matX;
            float realZ = bt.position.z - matZ;

            waterLine = 
            (Mathf.Sin(((realX * wave1.w) * wave1.x) + (((wave1.w - 1) * realZ) * wave1.x) + (time * wave1.z)) * wave1.y) + 
            (Mathf.Sin(((realX * wave2.w) * wave2.x) + (((wave2.w - 1) * realZ) * wave2.x) + (time * wave2.z)) * wave2.y) + 
            (Mathf.Sin(((realX * wave3.w) * wave3.x) + (((wave3.w - 1) * realZ) * wave3.x) + (time * wave3.z)) * wave3.y);

            Debug.Log(waterLine);
            //waterLine = (Mathf.Sin((time * speed1 + realX) * frequency1) * height1) + (Mathf.Sin((time * speed2 + realX + realZ) * frequency2) * height2);

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
