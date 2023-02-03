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
        var parentComp = GetComponentInParent<Cannon>();
        Vector3 dir = parentComp.cannonDirection;
        body.AddForce(dir, ForceMode.VelocityChange);
        transform.parent = null;
    }

    public virtual void OnUpdate(){
        if(transform.position.y < 0){
            Instantiate(splash, transform.position, Quaternion.Euler(0,0,0));
            //Debug.Log(transform.position);
            Destroy(gameObject);
        }
    }

}
