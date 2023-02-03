using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hole : MonoBehaviour
{
    GameObject boat;
    Task task;
    float timer;
    float timeToFix = 3;
    // Start is called before the first frame update
    void Start()
    {
        timer = timeToFix;
        boat = transform.parent.gameObject;
        task = gameObject.GetComponent<Task>();
    }

    // Update is called once per frame
    void Update()
    {
        if(task.isManned){
            if(timer > 0f){
                timer = timer - Time.deltaTime;
            }
            else{
                task.sailor.freeFromTask();
                Destroy(gameObject);
            }
        }
        else{
            timer = timeToFix;
        }

        CalcImportance();
    }

    void CalcImportance(){
        BoatAI boatStats = boat.GetComponent<BoatAI>();

        task.importance = 1f - boatStats.health/boatStats.baseMaxHealth;

    }

}
