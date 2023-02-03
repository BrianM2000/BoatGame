using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Sailor : MonoBehaviour
{
    public bool isWorking = false;
    public float workSpeedModifier = 1f;
    public float availability = 0; //0 for free, 1 for absolutely not free at all
    public Task task;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void manTask(Task t){
        task = t;
        isWorking = true;
        task.isManned = true;
        task.sailor = gameObject.GetComponent(typeof (Sailor)) as Sailor;
        availability = task.importance;
        GameObject obj = task.transform.gameObject;
        transform.position = obj.transform.position;
    }

    public virtual void freeFromTask(){
        isWorking = false;
        availability = 0;
        task.isManned = false;
        task.sailor = null;
        task = null;
    }

    
}
