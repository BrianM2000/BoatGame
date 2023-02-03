using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightBall : Cannonball
{
    public float velocity = 75;
    // Start is called before the first frame update
    void Start()
    {
        initVelo = velocity;
        OnStart();
    }

    // Update is called once per frame
    void Update()
    {
        OnUpdate();
    }
}
