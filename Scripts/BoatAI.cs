using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatAI : MonoBehaviour
{
    public Rigidbody body;
    public GameObject manager;
    public float baseMaxHealth = 100;
    public float curMaxHealth;
    public float health = 100;
    public float velocity = 10;
    public float rotateSpeed = 0;
    public float angle = 0;
    public float maxRotateSpeed = 1;
    public GameObject target;
    public float minDist = 100;
    public float attackAngle = 45;
    float turn = 0;
    public float maxSailors;
    public float curSailors;
    bool isColliding;
    GameObject[] boats;
    Sailor[] sailors;
    Cannon[] cannons;
    Task[] tasks;
    Task[] boatTasks;
    Task wheel;
    Task bail;
    float bailTimer;
    public float timeToBail = 5;
    public Hole hole;
    Hole[] holes;
    public Sailor sailor;
    [SerializeField] private AnimationCurve bailCurve;
    [SerializeField] private int _numRays = 16;
    // Start is called before the first frame update
    void Start()
    {
        manager = GameObject.FindGameObjectWithTag("BoatManager");
        bailTimer = timeToBail;
        curMaxHealth = baseMaxHealth;
        //boats = GameObject.FindGameObjectsWithTag("Boat");
        cannons = gameObject.GetComponentsInChildren<Cannon>();
        sailors = gameObject.GetComponentsInChildren<Sailor>();
        boatTasks = gameObject.GetComponents<Task>();
        wheel = boatTasks[0];
        bail = boatTasks[1];
        //Debug.Log("Sailors " + sailors.Length + " Cannons " + cannons.Length);
        
    }

    // Update is called once per frame
    void Update()
    {

        if(manager.GetComponent<BoatManager>().pause){
            return;
        }

        if(health < 0f){
            return;
        }
        tasks = gameObject.GetComponentsInChildren<Task>();
        isColliding = false;

        
        //get target to attack
        float tempDist = 100000000f;
        target = null;
        foreach(GameObject boat in BoatManager.boatList){
            //Debug.Log(boat != gameObject && boat.tag != gameObject.tag && boat.GetComponent<BoatAI>().health > 0f);
            if(boat != gameObject && boat.tag != gameObject.tag && boat.GetComponent<BoatAI>().health > 0f){
                float dist = Vector3.Distance(boat.transform.position, transform.position);
                if(dist < tempDist){
                    tempDist = dist;
                    target = boat;
                }
                
            }

        }

        //change speed (needs more work)
        velocity = 10f;
        if(target == null){
            velocity = 0f;
        }
        
        //move
        transform.position += transform.forward * Time.deltaTime * velocity;
        transform.position = new Vector3(transform.position.x, 0 , transform.position.z);

        //rotate boat
        //Vector3 v = GetVectorToFace(target.transform.position);

        List<List<Vector3>> inputs = new List<List<Vector3>>();

        if(wheel.isManned && target != null){
            
            Vector3 direction = (target.transform.position - transform.position).normalized;
            float distance = Vector3.Distance(transform.position, target.transform.position);

            Vector3 perp = Vector3.Cross(transform.forward, direction);
            float dir = Vector3.Dot(perp, transform.up);

            Vector2 temp = Vector2.Perpendicular(new Vector2(direction.x, direction.z));
            Vector3 toFace = new Vector3(temp.x, transform.position.y, temp.y);
            

            inputs.Add(GetVectorToFace(direction, (distance - minDist)/16f));


            if(dir > 0f){
                toFace = new Vector3(temp.x, transform.position.y, temp.y);
            }
            else{
                toFace = new Vector3(-temp.x, transform.position.y, -temp.y);
            }

            inputs.Add(GetVectorToFace(toFace, 1));

            perp = Vector3.Cross(transform.forward, (toFace.normalized + (direction * Mathf.Log10(distance/minDist))));
            dir = Vector3.Dot(perp, transform.up);

            Vector3 bestVec = GetBestVector(CombineVectors(inputs));

            Debug.DrawRay(transform.position, transform.forward * 10f, Color.blue);
            Debug.DrawRay(transform.position, direction * 10f, Color.red);
            Debug.DrawRay(transform.position, bestVec * 10f, Color.yellow);
            
            //float angle = Vector3.Angle(transform.forward, (toFace.normalized + (direction * Mathf.Log10(distance/minDist))));

            float angle = Vector3.Angle(transform.forward, bestVec);

            //doesn't work exactly right, from old version needs update
            turn = Mathf.Clamp((Mathf.Tan(angle/90f))/(Mathf.PI), 0f ,1f) * Mathf.Sign(dir);
            //Debug.Log(turn);

            transform.rotation = Quaternion.Euler(new Vector3(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y + maxRotateSpeed * turn * Time.deltaTime, transform.rotation.eulerAngles.z));

            /*
            if(rotateSpeed < maxRotateSpeed){
                if(dir > 0f){
                    rotateSpeed = Mathf.Lerp(rotateSpeed, maxRotateSpeed, wheelTurnSpeed * Time.deltaTime);
                }
                else{
                    rotateSpeed = Mathf.Lerp(rotateSpeed, -maxRotateSpeed, wheelTurnSpeed * Time.deltaTime);
                }
                
            }
            if(rotateSpeed > 0f){
                toFace = new Vector3(temp.x, transform.position.y, temp.y);
            }
            else{
                toFace = new Vector3(-temp.x, transform.position.y, -temp.y);
            }
            
            //Debug.DrawRay(transform.position, (toFace + (direction * distance/minDist)) * 10, Color.red);
            
            //should keep rotating, transform.forward = tranform.forward + something?
            transform.forward = Vector3.RotateTowards(transform.forward, (toFace.normalized + (direction * Mathf.Log10(distance/minDist))), Mathf.Abs(rotateSpeed) * Time.deltaTime, 0f);
            angle = Vector3.Angle(transform.forward,(toFace.normalized + (direction * Mathf.Log10(distance/minDist))));
            lastYDelta = (transform.rotation.eulerAngles.y - lastY);
            lastY = transform.rotation.eulerAngles.y;
            */
            
            
        }
        else{
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y + maxRotateSpeed * turn * Time.deltaTime, transform.rotation.eulerAngles.z);
        }

        //bail water
        if(bail.isManned){
            if(bailTimer < 0f){
                bailTimer = timeToBail;
                health = Mathf.Clamp(health + 5, 0,curMaxHealth) ;//amount bailed
            }
            else{
                bailTimer = bailTimer - Time.deltaTime;
            }
        }

        //Take damage from unfixed holes
        holes = gameObject.GetComponentsInChildren<Hole>();

        foreach(Hole h in holes){
            health = health - 1f * Time.deltaTime;
        }
        
        //Manage Tasks
        CalcImportance();
        
        foreach(Task t in tasks){
            if(t.importance > 0){
                if(!t.isManned){
                    Sailor s = FindBestSailor(sailors, t);
                    if(s != null){
                        s.manTask(t);
                    }
                    
                }
            }
            else{
                if(t.isManned){ //if task doesn't need sailors and is manned (i.e. someone's doing something that doesn't need being done)
                    t.freeTask();
                }
                
            }
            
        }

    }

    Sailor FindBestSailor(Sailor[] sailors, Task t){
        Sailor toReturn = null;
        float temp = 1f;

        foreach(Sailor s in sailors){
            if(s.availability < temp){
                temp = s.availability;
                toReturn = s;
            }
        }
        //Debug.Log(toReturn.availability + " " + t.importance);
        //Debug.Log(toReturn != null && toReturn.availability < t.importance);
        if(toReturn != null && toReturn.availability + .01 < t.importance){
            if(toReturn.task != null){
                toReturn.freeFromTask();
                //toReturn.task.freeTask();
            }
            return toReturn;
        }
        return null;
    }

    void CalcImportance(){
        //Debug.Log(Mathf.Abs(Vector3.Angle(transform.forward,(target.transform.position - transform.position).normalized) - 90f) > attackAngle);
        if(Mathf.Abs(Vector3.Angle(transform.forward,(target.transform.position - transform.position).normalized) - 90f) > attackAngle){
            wheel.importance = .7f;
        }
        else{
            wheel.importance = Mathf.Clamp(Mathf.Abs(Vector3.Angle(transform.forward,(target.transform.position - transform.position).normalized) - 90f)/90f, 0, 1);
        }

        bail.importance = bailCurve.Evaluate(1f - health/curMaxHealth);

    }

    private void OnTriggerEnter(Collider other)
    {
        if(isColliding) return;
        isColliding = true;

        Cannonball ball;

        if(!other.gameObject.TryGetComponent<Cannonball>(out ball)){
            ball = null;
            return;
        }
        
        if(ball.parent == gameObject){
            return;
        }
        
        health = health - ball.damage;
        curMaxHealth = curMaxHealth - ball.damage * .25f; //Deal a percentage of damage as permanant damage
        Instantiate(hole, transform.position, transform.rotation, transform);
        Destroy(other.gameObject);
    }

    public void HandleInputData(int val){
        BoatManager balls = manager.GetComponent<BoatManager>();
        Cannonball ball = balls.regularBall;

        if(val == 0){
            ball = balls.regularBall;
        }
        if(val == 1){
            ball = balls.lightBall;
        }
        if(val == 2){
            ball = balls.heavyBall;
        }

        foreach(Cannon c in cannons){
            c.ball = ball;
        }

    }

    public void InstantiateSailors(){
        if(curSailors > GetComponentsInChildren<Sailor>().Length){
            int i = 0;
            for(i = 0; i < curSailors; ++i){
                Instantiate(sailor,transform.position, transform.rotation, transform);
            }
        }

        sailors = gameObject.GetComponentsInChildren<Sailor>();
        
    }

    List<Vector3> GetVectorToFace(Vector3 input, float weight){

        List<Vector3> vs = new List<Vector3>();
        
        int i = 0;
        int numRays = _numRays;
        for(i = 0; i < numRays; ++i){
            float theta = 360f/numRays * i;
            //Debug.DrawRay(transform.position, (Quaternion.AngleAxis(theta, Vector3.up) * transform.forward).normalized * 10, Color.red);
            Vector3 ray = (Quaternion.AngleAxis(theta, Vector3.up) * transform.forward).normalized;
            ray = ray * Vector3.Dot(ray, input.normalized) * weight;
            
            vs.Add(ray);
            
        }

        return vs;
    }

    Vector3[] CombineVectors(List<List<Vector3>> vs){
        Vector3[] temp = new Vector3[_numRays];
        int i = 0;

        for(i = 0; i < _numRays; ++i){
            temp[i] = Vector3.zero;
        }

        foreach(List<Vector3> list in vs){
            for(i = 0; i < _numRays; ++i){
                temp[i] = temp[i] + list[i];
            }
        }

        foreach(Vector3 v in temp){
            Debug.DrawRay(transform.position, v * 10, Color.green);
        }

        return temp;
    }

    Vector3 GetBestVector(Vector3[] vs){

        Vector3 temp = transform.forward;
        float bestMag = 0; 
        
        foreach(Vector3 v in vs){
            if(v.magnitude > bestMag){
                bestMag = v.magnitude;
                temp = v;
            }
        }
        return temp;
    }

}
