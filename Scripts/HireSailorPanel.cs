using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HireSailorPanel : MonoBehaviour
{
    public GameObject sailor;
    ShopManager sm;
    
    // Start is called before the first frame update
    void Start()
    {
        sm = GameObject.Find("ShopManager").GetComponent<ShopManager>();

        TMPro.TMP_Text name = transform.Find("Name").GetComponent<TMPro.TMP_Text>();
        TMPro.TMP_Text health = transform.Find("Health").GetComponent<TMPro.TMP_Text>();
        TMPro.TMP_Text workSpeedMod = transform.Find("WorkSpeedMod").GetComponent<TMPro.TMP_Text>();
        name.text = sailor.name.Substring(0,sailor.name.IndexOf("("));
        health.text = sailor.GetComponent<Sailor>().health.ToString();
        workSpeedMod.text = sailor.GetComponent<Sailor>().workSpeedModifier.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnButtonPress(){
        //sm.AddSailor(sailor);
        sailor.transform.parent = sm.playerManager.transform;
        Button b = GetComponentInChildren<Button>();
        b.interactable = !b.interactable;
    }
}
