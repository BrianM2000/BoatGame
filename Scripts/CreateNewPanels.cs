using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateNewPanels : MonoBehaviour
{
    public GameObject panel;
    public GameObject playerManager;
    public BoatManager manager;
    // Start is called before the first frame update
    void Awake()
    {
        playerManager = GameObject.Find("PlayerManager");
        PlayerManager pm = playerManager.GetComponent<PlayerManager>();

        foreach(GameObject boat in pm.boats){
            GameObject p = Instantiate(panel, transform.position, transform.rotation, transform);
            PanelScript ps = p.GetComponent<PanelScript>();
            ps.boat = Instantiate(boat, Vector3.zero, Quaternion.Euler(Vector3.zero));
            ps.boat.tag = "PlayerBoat";
            ps.boat.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            BoatManager.boatList.Add(ps.boat);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
