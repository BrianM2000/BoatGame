using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SailorPanel : MonoBehaviour
{
    public GameObject boat;
    public GameObject sailor;
    Sailor s;
    // Start is called before the first frame update
    void Start()
    {
        s = sailor.GetComponent<Sailor>();
        if(!s.availableForAssignment){
            ToggleButton();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnButtonPress(){
        BoatAI b = boat.GetComponent<BoatAI>();
        b.sailorsToAdd.Add(sailor);
        s.availableForAssignment = false;
        sailor.SetActive(true);
        ToggleButton();
    }

    void ToggleButton(){
        Button b = GetComponentInChildren<Button>();
        b.interactable = !b.interactable;
    }

}
