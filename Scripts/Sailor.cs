using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Sailor : MonoBehaviour
{
    public float baseHealth = 100f;
    public float health;
    public bool isAlive = true;
    public bool isWorking = false;
    public float baseWorkSpeedModifier;
    public float workSpeedModifier;
    public float availability = 0; //0 for free, 1 for absolutely not free at all
    public Task task;
    public bool availableForAssignment = true;
    // Start is called before the first frame update
    void Start()
    {
        health = baseHealth;
        workSpeedModifier = baseWorkSpeedModifier;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void manTask(Task t){
        task = t;
        isWorking = true;
        task.isManned = true;
        task.sailors.Add(gameObject.GetComponent<Sailor>());
        availability = task.importance; //should it be importance or modified importance /:|
        GameObject obj = task.transform.gameObject;
        if(task.spot == null){
            transform.position = obj.transform.position;
        }
        else{
            transform.position = task.spot.position;
        }
    }

    public virtual void freeFromTask(){
        isWorking = false;
        availability = 0;
        //task.isManned = false;
        task.sailors.Remove(gameObject.GetComponent<Sailor>());
        if(task.sailors.Count == 0){
            task.isManned = false;
        }
        task = null;
    }

    public virtual void takeDamage(float damage){
        health = health - damage;
        if(health <= 0){
            die();
        }
    }

    public virtual void die(){
        isAlive = false;
        if(isWorking){
            freeFromTask();
        }
        
    }

    public void reset(){
        health = baseHealth;
        workSpeedModifier = baseWorkSpeedModifier;
        isWorking = false;
        task = null;
        availability = 0f;
        availableForAssignment = true;
    }

}
