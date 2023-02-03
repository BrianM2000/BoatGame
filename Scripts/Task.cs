using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Task : MonoBehaviour
{
    // Start is called before the first frame update
    public bool isManned = false;
    //public bool needsSailor = false;
    public float importance = 0;
    public Sailor sailor;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(isManned){
            sailor.availability = importance;
        }
    }

    public void freeTask(){
        //sailor.freeFromTask(gameObject.GetComponent(typeof(Task)) as Task);
    }

}
