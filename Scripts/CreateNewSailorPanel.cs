using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateNewSailorPanel : MonoBehaviour
{
    public GameObject sailorPanel;
    public GameObject boatManager;
    BoatManager bm;
    public GameObject boat;
    // Start is called before the first frame update
    void Awake()
    {
        boatManager = GameObject.Find("BoatManager");
        bm = boatManager.GetComponent<BoatManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void InstantiateSailorPanels(){
        foreach(GameObject s in bm.sailors){
            GameObject p = Instantiate(sailorPanel, transform.position, transform.rotation, transform);
            SailorPanel sp = p.GetComponent<SailorPanel>();
            sp.boat = boat;
            sp.sailor = s;
        }
    }

    public void toggleActive(){
        gameObject.transform.parent.gameObject.SetActive(false);
    }
}
