using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Cannonball : MonoBehaviour
{
    public Rigidbody body;
    public float initVelo = 10;
    public float damage = 10;
    public GameObject parent;
    [SerializeField] ParticleSystem splash;
    bool isColliding;
    float splashDamageRange = 10f;
    float sailorDamage = 50;
    // Start is called before the first frame update
    void Start()
    {
        OnStart();
    }

    // Update is called once per frame
    void Update()
    {
        OnUpdate();
    }

    public virtual void OnStart(){
        body.angularVelocity = Vector3.zero;
        parent = transform.parent.transform.parent.transform.parent.gameObject;
        Cannon parentComp = GetComponentInParent<Cannon>();
        BoatAI boatAI = transform.root.GetComponent<BoatAI>();
        Vector3 dir = parentComp.cannonDirection;
        float range = parentComp.cannonError * (1f/((parentComp.GetComponent<Task>().sumWorkSpeedMod() + boatAI.cannonAccuracyBonus) * boatAI.cannonAccuracyMultipler));
        body.AddForce(dir + 
            new Vector3(Random.Range(-range,range),
            Random.Range(-range,range),
            Random.Range(-range,range)), 
            ForceMode.VelocityChange);
        transform.parent = null;
    }

    public virtual void OnUpdate(){
        isColliding = false;
        if(transform.position.y < -10){
            Instantiate(splash, transform.position, Quaternion.Euler(0,0,0));
            //Debug.Log(transform.position);
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision){
        GameObject obj = collision.gameObject;

        if(isColliding) return;
        isColliding = true;

        Vector3 impactPoint = collision.GetContact(0).point;

        //Debug.Log(parent + " " + obj.transform.root.gameObject);

        if(parent == obj.transform.root.gameObject){
            return;
        }

        if((obj.tag == "PlayerBoat" || obj.tag == "EnemyBoat")){
            BoatAI boat = obj.GetComponent<BoatAI>();
            boat.health = boat.health - damage;
            boat.curMaxHealth = boat.curMaxHealth - damage * .25f; //Deal a percentage of damage as permanant damage
            Instantiate(boat.hole, collision.GetContact(0).point, boat.transform.rotation, boat.transform);

            foreach(Sailor s in boat.sailors){
                float dist = Vector3.Distance(s.transform.position, impactPoint);
                if(dist <= splashDamageRange){
                    s.takeDamage(sailorDamage * (dist/splashDamageRange));
                }
            }

        }

        if(obj.tag == "Sailor"){
            Sailor s = obj.GetComponent<Sailor>();
            s.takeDamage(s.health + 1f);
        }

        Destroy(gameObject);

    }

}
