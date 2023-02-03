using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelScript : MonoBehaviour
{
    public GameObject boat;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void HandleInputData(int val){
        boat.GetComponent<BoatAI>().HandleInputData(val);
    }

    public void OnSliderValueChange(float val){
        boat.GetComponent<BoatAI>().curSailors = val;
    }
}
