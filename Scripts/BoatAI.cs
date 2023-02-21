using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatAI : MonoBehaviour
{
    public Rigidbody body;
    public GameObject manager;
    BoatManager bm;
    public float baseMaxHealth = 100;
    public float curMaxHealth;
    public float health = 100;
    public float velocity = 10;
    public float rotateSpeed = 0;
    public float angle = 0;
    public float maxRotateSpeed = 1;
    [SerializeField] Quaternion rot;
    public GameObject target;
    public float minDist = 100;
    public float attackAngle = 45;
    public float maxSailors;
    public float curSailors;
    public float curSailsPer = 0;
    public float maxSpeed = 10f;
    bool isColliding;
    GameObject[] boats;
    public Sailor[] sailors;
    public List<GameObject> sailorsToAdd;
    Cannon[] cannons;
    Task[] tasks;
    Task[] boatTasks;
    Task wheel;
    Task bail;
    Task sails;
    public float bailTimer;
    public float timeToBail = 5;
    public Hole hole;
    Hole[] holes;
    public Sailor sailor;
    [SerializeField] private AnimationCurve bailCurve;
    [SerializeField] private int _numRays = 16;
    List<List<Vector3>> inputs = new List<List<Vector3>>();
    float lastDeltaY;
    float _fear = 8f;
    public bool sunk = false;
    Bouy[] bouys;
    [SerializeField] List<UpgradeBaseSO> upgrades = new List<UpgradeBaseSO>();
    public float cannonLoadSpeedBonus = 0;
    public float cannonLoadSpeedMultipler = 1;
    public float cannonAccuracyBonus = 0;
    public float cannonAccuracyMultipler = 1;
    public float shipHealthBonus = 0;
    public float shipHealthMultipler = 1;
    public float shipSpeedBonus = 0;
    public float shipSpeedMultipler = 1;
    public float shipSailsBonus = 0;
    public float shipSailsMultipler = 1;
    public float shipBailSpeedBonus = 0;
    public float shipBailSpeedMultipler = 1;
    public float shipRepairSpeedBonus = 0;
    public float shipRepairSpeedMultipler = 1;
    public float sailorHealthBonus = 0;
    public float sailorHealthMultipler = 1;
    public float sailorWorkSpeedBonus = 0;
    public float sailorWorkSpeedMultipler = 1;
    // Start is called before the first frame update
    void Start()
    {

        lastDeltaY = 0;

        rot = Quaternion.identity;
        manager = GameObject.FindGameObjectWithTag("BoatManager");
        bm = manager.GetComponent<BoatManager>();

        bailTimer = timeToBail  - shipBailSpeedBonus;
        baseMaxHealth = ((baseMaxHealth + shipHealthBonus) * shipHealthMultipler);
        maxSpeed = ((maxSpeed + shipSpeedBonus) * shipSpeedMultipler);
        curMaxHealth = baseMaxHealth;

        cannons = gameObject.GetComponentsInChildren<Cannon>();
        sailors = gameObject.GetComponentsInChildren<Sailor>();
        boatTasks = gameObject.GetComponents<Task>();
        wheel = boatTasks[0];
        bail = boatTasks[1];
        sails = boatTasks[2];
        
    }

    void Initialize(){
        
    }

    // Update is called once per frame
    void Update()
    {

        if(bm.pause){
            return;
        }

        if(health <= 0f){
            if(!sunk){
                sink();
            }
            else{
                foreach(Bouy b in bouys){
                    if(b.bouancy >  b.targetBouancy){
                        b.bouancy = Mathf.MoveTowards(b.bouancy, b.targetBouancy, (.01f + Random.Range(0f, .1f)) * Time.deltaTime);
                    }
                    if(b.targetBouancy > 0){
                        b.targetBouancy = Mathf.MoveTowards(b.targetBouancy, 0, .005f * Time.deltaTime);
                    }
                    
                }
            }
            if(velocity > 0){
                velocity = Mathf.Clamp(velocity - (1f * Time.deltaTime), 0 , maxSpeed);
                transform.position += transform.forward * Time.deltaTime * velocity;
            }
            
            return;
        }
        
        isColliding = false;
        
        tasks = gameObject.GetComponentsInChildren<Task>();
        
        //get target to attack
        float tempDist = 100000000f;
        target = null;
        foreach(GameObject boat in bm.boatList){
            //Debug.Log(boat != gameObject && boat.tag != gameObject.tag && boat.GetComponent<BoatAI>().health > 0f);
            if(boat != gameObject && boat.tag != gameObject.tag && boat.GetComponent<BoatAI>().health > 0f){
                float dist = Vector3.Distance(boat.transform.position, transform.position);
                if(dist < tempDist){
                    tempDist = dist;
                    target = boat;
                }
                
            }

        }

        //rotate boat
        inputs.Clear();
        CheckForCollisions();

        Vector3 direction = (target.transform.position - transform.position).normalized;
        float distance = Vector3.Distance(transform.position, target.transform.position);

        Vector3 perp = Vector3.Cross(transform.forward, direction);
        float dir = Vector3.Dot(perp, transform.up);

        Vector2 temp = Vector2.Perpendicular(new Vector2(direction.x, direction.z));
        Vector3 toFace = new Vector3(temp.x, transform.position.y, temp.y);
        
        inputs.Add(GetVectorToFace(direction, (distance - minDist)/16f));
        inputs.Add(GetVectorToFace(direction, -1f * (1f - health/curMaxHealth) * _fear));
        inputs.Add(GetVectorToFace(Vector3.zero, Vector3.Distance(Vector3.zero, transform.position)));

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

        /*
        Debug.DrawRay(transform.position, transform.forward * 10f, Color.blue);
        Debug.DrawRay(transform.position, direction * 10f, Color.red);
        if(gameObject.CompareTag("PlayerBoat")){
            Debug.DrawRay(transform.position, bestVec * 10f, Color.yellow);
        }
        */

        angle = Vector3.Angle(transform.forward, bestVec);

        if(wheel.isManned && target != null){
            
            float y = transform.rotation.y;
            rot = Quaternion.LookRotation(bestVec);
            transform.rotation = Quaternion.Lerp(transform.rotation, rot, maxRotateSpeed * Time.deltaTime);
            lastDeltaY = y - transform.rotation.y;

        }
        else{
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y + lastDeltaY, transform.rotation.eulerAngles.z);
        }

        //change speed (needs more work)
        float desiredSailPer = angle/180f;
        
        if(sails.isManned){
            curSailsPer = curSailsPer + (desiredSailPer - curSailsPer) * (Time.deltaTime * (sails.sumWorkSpeedMod() + shipSailsBonus) * shipSailsMultipler);
        }
        
        velocity = maxSpeed * (1f - curSailsPer);
        
        //move
        transform.position += transform.forward * Time.deltaTime * velocity;
        //transform.position = new Vector3(transform.position.x, 0, transform.position.z);

        //bail water
        if(bail.isManned){
            if(bailTimer < 0f){
                bailTimer = timeToBail - shipBailSpeedBonus;
                health = Mathf.Clamp(health + 5, 0,curMaxHealth) ;//amount bailed
            }
            else{
                bailTimer = bailTimer - (((Time.deltaTime * bail.sumWorkSpeedMod())) * shipBailSpeedMultipler) ;
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
                if(t.maxWorkers > t.sailors.Count){
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
            if(s.isAlive && s.availability < temp){
                temp = s.availability;
                toReturn = s;
            }
        }
        //Debug.Log(toReturn.availability + " " + t.importance);
        //Debug.Log(toReturn != null && toReturn.availability < t.importance);
        if(toReturn != null && toReturn.availability + .15f < t.modifiedImportance){
            if(toReturn.task != null){
                toReturn.freeFromTask();
                //toReturn.task.freeTask();
            }
            return toReturn;
        }
        return null;
    }

    void CalcImportance(){
        
        wheel.importance = (angle/180f);
        if(angle < 0.2){
            wheel.importance = .15f;
        }

        bail.importance = bailCurve.Evaluate(1f - health/curMaxHealth);

        sails.importance = Mathf.Abs(angle/180f - curSailsPer);

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

    public void InstantiateSailor(GameObject sailor){
        sailor.transform.parent = gameObject.transform;

        Sailor s = sailor.GetComponent<Sailor>();
        s.health = s.baseHealth;
        s.workSpeedModifier = s.baseWorkSpeedModifier;
        s.health = (s.health + sailorHealthBonus) * sailorHealthMultipler;
        s.workSpeedModifier = (s.workSpeedModifier + sailorWorkSpeedBonus) * sailorWorkSpeedMultipler;

        sailors = gameObject.GetComponentsInChildren<Sailor>();
    }

    public void InstantiateSailors(){

        foreach(GameObject s in sailorsToAdd){
            s.transform.parent = gameObject.transform;
            //Instantiate(s,transform.position, transform.rotation, transform);
        }

        sailors = gameObject.GetComponentsInChildren<Sailor>();
        
    }

    public void InstantiateBasicSailors(){
        if(curSailors > GetComponentsInChildren<Sailor>().Length){
            int i = 0;
            for(i = 0; i < curSailors; ++i){
                Instantiate(sailor,transform.position, transform.rotation, transform);
            }
        }

        sailors = gameObject.GetComponentsInChildren<Sailor>();
    }

    void CheckForCollisions(){
        List<Vector3> vs = new List<Vector3>();
        
        int i = 0;
        int numRays = _numRays;
        for(i = 0; i < numRays; ++i){
            float theta = 360f/numRays * i;
            Vector3 ray = (Quaternion.AngleAxis(theta, Vector3.up) * transform.forward).normalized;
            
            vs.Add(ray);
            
        }

        foreach(Vector3 v in vs){
            RaycastHit hit;
            if(Physics.Raycast(transform.position, v, out hit, minDist/2f)){
                //Debug.Log(hit.distance);
                if(hit.collider.gameObject.transform.root != gameObject.transform.root){
                    inputs.Add(GetVectorToFace(v, -1 * (minDist/2f - hit.distance)));
                }
                
            }
        }

    }

    List<Vector3> GetVectorToFace(Vector3 input, float weight){

        List<Vector3> vs = new List<Vector3>();
        
        int i = 0;
        int numRays = _numRays;
        for(i = 0; i < numRays; ++i){
            float theta = 360f/numRays * i;
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

        /*
        foreach(Vector3 v in temp){
            Debug.DrawRay(transform.position, v * 10, Color.green);
        }
        */

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

    public int aliveSailorCount(){
        int count = 0;
        foreach(Sailor s in sailors){
            if(s.isAlive){
                ++count;
            }
        }
        return count;
    }

    public void startVoyage(){

    }

    public void reset(){
        transform.position = Vector3.zero;
        transform.rotation = Quaternion.identity;

        /*
        cannonLoadSpeedBonus = 0;
        cannonLoadSpeedMultipler = 1;
        cannonAccuracyBonus = 0;
        cannonAccuracyMultipler = 1;
        
        shipHealthBonus = 0;
        shipHealthMultipler = 1;
        shipSpeedBonus = 0;
        shipSpeedMultipler = 1;
        shipSailsBonus = 0;
        shipSailsMultipler = 1;
        shipBailSpeedBonus = 0;
        shipBailSpeedMultipler = 1;
        shipRepairSpeedBonus = 0;
        shipRepairSpeedMultipler = 1;

        sailorHealthBonus = 0;
        sailorHealthMultipler = 1;
        sailorWorkSpeedBonus = 0;
        sailorWorkSpeedMultipler = 1;
        */

        return;
    }

    void resetStats(){
        cannonLoadSpeedBonus = 0;
        cannonLoadSpeedMultipler = 1;
        cannonAccuracyBonus = 0;
        cannonAccuracyMultipler = 1;
        
        shipHealthBonus = 0;
        shipHealthMultipler = 1;
        shipSpeedBonus = 0;
        shipSpeedMultipler = 1;
        shipSailsBonus = 0;
        shipSailsMultipler = 1;
        shipBailSpeedBonus = 0;
        shipBailSpeedMultipler = 1;
        shipRepairSpeedBonus = 0;
        shipRepairSpeedMultipler = 1;

        sailorHealthBonus = 0;
        sailorHealthMultipler = 1;
        sailorWorkSpeedBonus = 0;
        sailorWorkSpeedMultipler = 1;
    }

    public void upgradeStats(){
        resetStats();
        foreach(UpgradeBaseSO up in upgrades){
            cannonLoadSpeedBonus += up.cannonLoadSpeedBonus;
            cannonLoadSpeedMultipler *= up.cannonLoadSpeedMultipler;
            cannonAccuracyBonus += up.cannonAccuracyBonus;
            cannonAccuracyMultipler *= up.cannonAccuracyMultipler;

            shipHealthBonus += up.shipHealthBonus;
            shipHealthMultipler *= up.shipHealthMultipler;
            shipSpeedBonus += up.shipSpeedBonus;
            shipSpeedMultipler *= up.shipSpeedMultipler;
            shipSailsBonus += up.shipSailsBonus;
            shipSailsMultipler *= up.shipSailsMultipler;
            shipBailSpeedBonus += up.shipBailSpeedBonus;
            shipBailSpeedMultipler *= up.shipBailSpeedMultipler;
            shipRepairSpeedBonus += up.shipRepairSpeedBonus;
            shipRepairSpeedMultipler *= up.shipRepairSpeedMultipler;

            sailorHealthBonus += up.sailorHealthBonus;
            sailorHealthMultipler *= up.sailorHealthMultipler;
            sailorWorkSpeedBonus += up.sailorWorkSpeedBonus;
            sailorWorkSpeedMultipler *= up.sailorWorkSpeedMultipler;
        }

    }

    public void upgradeStats(UpgradeBaseSO upgrade, int addOrSubtract){//addOrSubtract should be 1 or -1
        
        cannonLoadSpeedBonus += addOrSubtract * upgrade.cannonLoadSpeedBonus;
        cannonLoadSpeedMultipler *= addOrSubtract * upgrade.cannonLoadSpeedMultipler;
        cannonAccuracyBonus += addOrSubtract * upgrade.cannonAccuracyBonus;
        cannonAccuracyMultipler *= addOrSubtract * upgrade.cannonAccuracyMultipler;

        shipHealthBonus += addOrSubtract * upgrade.shipHealthBonus;
        shipHealthMultipler *= addOrSubtract * upgrade.shipHealthMultipler;
        shipSpeedBonus += addOrSubtract * upgrade.shipSpeedBonus;
        shipSpeedMultipler *= addOrSubtract * upgrade.shipSpeedMultipler;
        shipSailsBonus += addOrSubtract * upgrade.shipSailsBonus;
        shipSailsMultipler *= addOrSubtract * upgrade.shipSailsMultipler;
        shipBailSpeedBonus += addOrSubtract * upgrade.shipBailSpeedBonus;
        shipBailSpeedMultipler *= addOrSubtract * upgrade.shipBailSpeedMultipler;
        shipRepairSpeedBonus += addOrSubtract * upgrade.shipRepairSpeedBonus;
        shipRepairSpeedMultipler *= addOrSubtract * upgrade.shipRepairSpeedMultipler;

        sailorHealthBonus += addOrSubtract * upgrade.sailorHealthBonus;
        sailorHealthMultipler *= addOrSubtract * upgrade.sailorHealthMultipler;
        sailorWorkSpeedBonus += addOrSubtract * upgrade.sailorWorkSpeedBonus;
        sailorWorkSpeedMultipler *= addOrSubtract * upgrade.sailorWorkSpeedMultipler;
    }

    void sink(){
        sunk = true;
        body.constraints = RigidbodyConstraints.None;
        bouys = gameObject.GetComponentsInChildren<Bouy>();
        foreach(Bouy b in bouys){
            b.targetBouancy = Random.Range(-.1f, .2f);
        }
    }

}
