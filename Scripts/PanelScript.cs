using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelScript : MonoBehaviour
{
    public GameObject boat;
    public GameObject MainSailorPanel;
    static GameObject sp;
    CreateNewSailorPanel p;
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

    public void OnButtonPress(){
        if(sp == null){
            sp = Instantiate(MainSailorPanel, transform.root.position, transform.rotation, transform.root);
            p = sp.GetComponentInChildren<CreateNewSailorPanel>();
            p.boat = boat;
            p.InstantiateSailorPanels();
        }
        else{
            Destroy(sp);
            sp = Instantiate(MainSailorPanel, transform.root.position, transform.rotation, transform.root);
            p = sp.GetComponentInChildren<CreateNewSailorPanel>();
            p.boat = boat;
            p.InstantiateSailorPanels();
        }
    }
}
