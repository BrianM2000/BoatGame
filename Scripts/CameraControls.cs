using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControls : MonoBehaviour
{
    public float maxZoom = 700;
    public float minZoom = 500;
    public float zoom;
    public float zoomCap;
    public float scrollSens = 1;
    public float moveSpeed;
    float pos;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //zoom in and out
        zoom += Input.mouseScrollDelta.y * scrollSens;
        zoom =  Mathf.Clamp(zoom, -zoomCap, zoomCap);

        float pos = Mathf.Clamp(transform.position.y - zoom * scrollSens * Time.deltaTime, minZoom, maxZoom);
        if(pos <= minZoom || pos >= maxZoom){
            zoom = 0;
        }
        transform.position = new Vector3(transform.position.x, pos, transform.position.z);

        zoom = Mathf.MoveTowards(zoom, 0, .01f);

        //move in x and z
        if(Input.GetKey("w")){
            transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + moveSpeed * Time.deltaTime);
        }
        if(Input.GetKey("s")){
            transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - moveSpeed * Time.deltaTime);
        }

        if(Input.GetKey("a")){
            transform.position = new Vector3(transform.position.x - moveSpeed * Time.deltaTime, transform.position.y, transform.position.z);
        }
        if(Input.GetKey("d")){
            transform.position = new Vector3(transform.position.x + moveSpeed * Time.deltaTime, transform.position.y, transform.position.z);
        }
    }
}
