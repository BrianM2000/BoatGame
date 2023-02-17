using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SailorPanel : MonoBehaviour
{
    public GameObject boat;
    public GameObject sailor;
    Sailor s;
    Button button;
    TMPro.TMP_Text bText;
    // Start is called before the first frame update
    void Start()
    {
        button = GetComponentInChildren<Button>();
        bText = button.transform.Find("ButtonText").GetComponent<TMPro.TMP_Text>();
        s = sailor.GetComponent<Sailor>();
        
        TMPro.TMP_Text name = transform.Find("Name").GetComponent<TMPro.TMP_Text>();
        TMPro.TMP_Text health = transform.Find("Health").GetComponent<TMPro.TMP_Text>();
        TMPro.TMP_Text workSpeedMod = transform.Find("WorkSpeedMod").GetComponent<TMPro.TMP_Text>();
        name.text = sailor.name.Substring(0,sailor.name.IndexOf("("));
        health.text = sailor.GetComponent<Sailor>().health.ToString();
        workSpeedMod.text = sailor.GetComponent<Sailor>().workSpeedModifier.ToString();

        if(s.availableForAssignment){
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(OnButtonPress);
            bText.text = "Assign";
            return;
        }
        if(boat == sailor.transform.parent.gameObject){
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(unassignButtonPress);
            bText.text = "Unassign";
        }
        else{
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(switchAssignmentButtonPress);
            bText.text = "Swtich Assignment";
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnButtonPress(){
        BoatAI b = boat.GetComponent<BoatAI>();
        b.InstantiateSailor(sailor);
        s.availableForAssignment = false;
        sailor.SetActive(true);
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(unassignButtonPress);
        bText.text = "Unassign";
    }

    public void unassignButtonPress(){
        s.availableForAssignment = true;
        sailor.SetActive(false);
        sailor.transform.parent = GameObject.Find("PlayerManager").transform;
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(OnButtonPress);
        bText.text = "Assign";
    }

    public void switchAssignmentButtonPress(){
        BoatAI b = boat.GetComponent<BoatAI>();
        b.InstantiateSailor(sailor);
        s.availableForAssignment = false;
        sailor.SetActive(true);
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(unassignButtonPress);
        bText.text = "Unassign";
    }

    void ToggleButton(){
        Button b = GetComponentInChildren<Button>();
        b.interactable = !b.interactable;
    }

}
