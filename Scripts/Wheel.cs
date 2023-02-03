using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wheel : MonoBehaviour
{
    public float rotation;
    // Start is called before the first frame update
    void Start()
    {
        rotation = transform.rotation.eulerAngles.z;
    }

    // Update is called once per frame
    void Update()
    {
        //rotate z
    }
}
