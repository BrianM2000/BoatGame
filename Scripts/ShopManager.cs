using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public PlayerManager playerManager;
    public bool Tavern = false;
    public bool Docks = false;
    public GameObject baseSailor;
    public GameObject sailorPanel;
    public GameObject shopPanel;
    // Start is called before the first frame update
    void Start()
    {
        playerManager = GameObject.Find("PlayerManager").GetComponent<PlayerManager>();

        if(Tavern){
            generateSailorsToHire();
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void generateSailorsToHire(){
        float randNum = Mathf.RoundToInt(Random.Range(20, 30));
        int i;
        for(i = 0; i < randNum; ++i){
            GameObject temp = playerManager.generateNewSailor();
            GameObject s = Instantiate(temp, Vector3.zero, Quaternion.identity);
            Destroy(temp);
            s.SetActive(false);
            GameObject p = Instantiate(sailorPanel, shopPanel.transform.position, shopPanel.transform.rotation, shopPanel.transform);
            HireSailorPanel sp = p.GetComponent<HireSailorPanel>();
            //sp.boat = boat;
            sp.sailor = s;
        }
    }

    public void AddBoat(GameObject boat){
        playerManager.boats.Add(boat);
        boat.SetActive(false);
        GameObject tb = Instantiate(boat, Vector3.zero, Quaternion.identity, playerManager.transform);
        //boat.transform.parent = playerManager.transform;
    }

    public void AddSailor(GameObject sailor){
        playerManager.sailors.Add(sailor);
    }

}
