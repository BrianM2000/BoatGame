using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour
{
    public float rotateSpeed = 40;
    public Cannonball ball;
    private Vector3 target;
    private Camera _cam;
    float lastTime = 1;
    Plane plane = new Plane(Vector3.up,0);
    GameObject[] boats;
    public Vector3 cannonDirection;
    public bool canFire = false;
    public float reloadSpeed = 100f;
    public float timer;
    public bool isLoaded;
    public float cannonError;
    Task task;
    GameObject manager;
    // Start is called before the first frame update
    void Start()
    {
        timer = reloadSpeed;
        task = gameObject.GetComponent(typeof (Task)) as Task;
        manager = GameObject.FindGameObjectWithTag("BoatManager");
    }

    // Update is called once per frame
    void Update()
    {
        if(manager.GetComponent<BoatManager>().pause){
            return;
        }
        
        if(transform.localEulerAngles.y < 135f && transform.localEulerAngles.y > 45f){
            canFire = true;
        }
        else{
            canFire = false;
        }

        BoatAI boatAI = gameObject.transform.parent.GetComponentInParent(typeof (BoatAI)) as BoatAI;
        GameObject target = boatAI.target;
        BoatAI targetAI = target.gameObject.GetComponent<BoatAI>();

        /*
        float distance;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (plane.Raycast(ray, out distance)){
            target = ray.GetPoint(distance);
            //Debug.Log(ray.GetPoint(distance));
        }
        */

        Vector3 targetPositionLeading = target.transform.position + target.transform.forward * targetAI.velocity * lastTime;

        Vector3 diff = new Vector3(targetPositionLeading.x, transform.position.y, targetPositionLeading.z) - transform.position;

        Vector3 direction = (targetPositionLeading - transform.position).normalized;
        float distance = Vector3.Distance(transform.position, targetPositionLeading);
        float angle;
        Vector3 VxzDirection;
        
        if((-Physics.gravity.y * distance / (ball.initVelo * ball.initVelo)) >= 1){
            angle = Mathf.Asin(1) / 2;;
        }
        else{
            angle = Mathf.Asin(-Physics.gravity.y * distance / (ball.initVelo * ball.initVelo)) / 2;
        }

        lastTime = -(2f*ball.initVelo*Mathf.Sin(angle))/Physics.gravity.y;
        //Debug.Log(lastTime);
        
        float Vy = ball.initVelo * Mathf.Sin(angle);
        float Vxz = ball.initVelo * Mathf.Cos(angle);

        VxzDirection = direction * Vxz;
        VxzDirection.y = Vy;

        cannonDirection = VxzDirection;
        //Debug.Log(cannonDirection);

        Vector3 newDir = Vector3.RotateTowards(transform.up, cannonDirection, rotateSpeed * Time.deltaTime, 100);

        transform.rotation = Quaternion.LookRotation((transform.forward + diff), newDir);

        if(timer > 0f){
            if(task.isManned && !isLoaded){
                timer = timer - (((Time.deltaTime  * task.sumWorkSpeedMod())) * boatAI.cannonLoadSpeedMultipler);
            }
            else{
                timer = reloadSpeed - boatAI.cannonLoadSpeedBonus;
            }
        }
        else{
            timer = reloadSpeed;
            isLoaded = true;
        }
        
        if(canFire && task.isManned && isLoaded){
            Instantiate(ball, transform.position + transform.forward, transform.rotation, transform);
            isLoaded = false;
        }

        /*
        if(Input.GetMouseButtonDown(0)){
            if(canFire){
                Instantiate(ball, transform.position, transform.rotation, transform);
            }
        }
        */
        
        CalcImportance();

    }

    void CalcImportance(){
        if(canFire){
            task.importance = .65f;
            return;
        }
        if(!isLoaded){
            task.importance = .2f;
            return;
        }
        task.importance = 0f;
        return;
    }

}
