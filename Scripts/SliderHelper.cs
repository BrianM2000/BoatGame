using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderHelper : MonoBehaviour
{
    PanelScript script;
    BoatAI boat;
    public Slider slider;
    // Start is called before the first frame update
    void Start()
    {
        script = GetComponentInParent<PanelScript>();
        boat = script.boat.GetComponent<BoatAI>();
        slider.maxValue = boat.maxSailors;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
