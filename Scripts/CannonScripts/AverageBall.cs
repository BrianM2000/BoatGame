using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AverageBall : Cannonball
{
    // Start is called before the first frame update
    void Start()
    {
        initVelo = 10;
        damage = 10;
        OnStart();
    }

    // Update is called once per frame
    void Update()
    {
        OnUpdate();
    }
}
