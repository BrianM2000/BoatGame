using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        BoatAI[] boats = pm.GetComponentsInChildren<BoatAI>(true);

        foreach(BoatAI boat in boats){
            GameObject p = Instantiate(panel, transform.position, transform.rotation, transform);
            PanelScript ps = p.GetComponent<PanelScript>();
            boat.transform.parent = null;
            SceneManager.MoveGameObjectToScene(boat.gameObject, SceneManager.GetActiveScene());
            boat.gameObject.SetActive(true);
            ps.boat = boat.gameObject;
            ps.boat.tag = "PlayerBoat";
            ps.boat.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionX | 
            RigidbodyConstraints.FreezePositionZ | 
            RigidbodyConstraints.FreezeRotationX |
            RigidbodyConstraints.FreezeRotationY |
            RigidbodyConstraints.FreezeRotationZ;
            manager.boatList.Add(ps.boat);
        }
        /*
        foreach(GameObject boat in pm.boats){
            GameObject p = Instantiate(panel, transform.position, transform.rotation, transform);
            PanelScript ps = p.GetComponent<PanelScript>();
            ps.boat = Instantiate(boat, Vector3.zero, Quaternion.Euler(Vector3.zero));
            ps.boat.tag = "PlayerBoat";
            ps.boat.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            manager.boatList.Add(ps.boat);
        }
        */
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
