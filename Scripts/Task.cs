using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Task : MonoBehaviour
{
    // Start is called before the first frame update
    public bool isManned = false;
    //public bool needsSailor = false;
    public float importance = 0;
    public float modifiedImportance;
    public int maxWorkers = 1;
    public List<Sailor> sailors;
    public Transform spot;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        modifiedImportance = importance * ((1f - ((float)sailors.Count/maxWorkers)));

        if(isManned){
            foreach(Sailor sailor in sailors){
                sailor.availability = importance; //* (1 - sailors.Count/maxWorkers);
            }
        }
    }

    public void freeTask(){
        //sailor.freeFromTask(gameObject.GetComponent(typeof(Task)) as Task);
    }

    public float sumWorkSpeedMod(){
        float sum = 0;
        foreach(Sailor s in sailors){
            sum = sum + s.workSpeedModifier;
        }
        return sum;
    }

}
