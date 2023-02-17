using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoatManager : MonoBehaviour
{
    public List<GameObject> boatList = new List<GameObject>();
    public List<GameObject> sailors = new List<GameObject>();
    public GameObject boat;
    public bool pause;
    public Canvas canvas;
    public Cannonball regularBall;
    public Cannonball lightBall;
    public Cannonball heavyBall;
    public bool active = false;
    public GameObject sideUI;
    public GameObject sailorUI;
    public float width = 600;
    public float height = 300;
    public float tokens = 1;
    public float maxTime = 30;
    public float minTime = 10;
    public float timer;
    GameObject playerManager;
    PlayerManager pm;
    public GameObject victoryUI;
    public GameObject defeatUI;
    void Start() {
        pause = true;
        timer = 0;

        playerManager = GameObject.Find("PlayerManager");
        pm = playerManager.GetComponent<PlayerManager>();

        foreach(Sailor s in pm.GetComponentsInChildren<Sailor>(true)){
            //GameObject sailor = Instantiate(s, Vector3.zero, Quaternion.identity);
            GameObject sailor = s.gameObject;
            sailor.SetActive(false);
            sailors.Add(sailor);
        }
    }

    void Update(){
        if(pause){
            return;
        }

        bool noEnemies = checkForAllSunkBoats(GameObject.FindGameObjectsWithTag("EnemyBoat"));

        if(!pause && tokens <= 0 && noEnemies){
            pause = true;
            GameObject[] playerBoats = GameObject.FindGameObjectsWithTag("PlayerBoat");
            
            foreach(GameObject boat in playerBoats){
                Sailor[] sailor = boat.GetComponentsInChildren<Sailor>();
                foreach(Sailor s in sailor){
                    s.transform.parent = pm.transform;
                    s.reset();
                    s.gameObject.SetActive(false);
                }
                boat.transform.parent = pm.transform;
                boat.GetComponent<BoatAI>().reset();
                boat.SetActive(false);
            }

            victoryUI.SetActive(true);
            return;

        }

        bool noFriends = checkForAllSunkBoats(GameObject.FindGameObjectsWithTag("PlayerBoat"));

        if(noFriends){
            pause = true;
            defeatUI.SetActive(true);
        }

        if(tokens <= 0){
            return;
        }

        if(timer <= 0f){
            timer = Random.Range(minTime, maxTime);
            float randX = Mathf.Sign(Random.Range(-1,1))*Random.Range(0, width);
            float randY = Mathf.Sign(Random.Range(-1,1))*Random.Range(height, height + 10);
            GameObject b = Instantiate(boat,new Vector3(randX, 0, randY), Quaternion.Euler(Vector3.zero));
            b.SetActive(true);
            b.tag = "EnemyBoat";
            BoatAI ba = b.GetComponent<BoatAI>();
            float randSailors = Random.Range(1, ba.maxSailors);
            ba.curSailors = randSailors;
            ba.InstantiateBasicSailors();
            Renderer[] r = b.GetComponentsInChildren<Renderer>();
            foreach(Renderer render in r){
                render.material.SetColor("_Color", Color.red);
            }
            boatList.Add(b);
            tokens--;
        }
        else{
            timer = timer - Time.deltaTime;
        }

        

    }
    public void StartUp(){
        sideUI.SetActive(false);
        sailorUI = GameObject.Find("SailorScrollObject(Clone)");
        if(sailorUI != null){
            sailorUI.SetActive(false);
        }
        

        pause = false;

        TMPro.TMP_Dropdown[] dp = canvas.gameObject.GetComponentsInChildren<TMPro.TMP_Dropdown>();

        foreach(TMPro.TMP_Dropdown d in dp){
            d.interactable = !d.interactable;
        }
        
        foreach(GameObject boat in GameObject.FindGameObjectsWithTag("PlayerBoat")){
            boat.GetComponent<BoatAI>().InstantiateSailors();
            Rigidbody rb = boat.GetComponent<Rigidbody>();
            //rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
            rb.constraints = RigidbodyConstraints.None;
        }

    }

    bool checkForAllSunkBoats(GameObject[] boats){
        bool temp = true;

        foreach(GameObject b in boats){
            temp = temp && b.GetComponent<BoatAI>().sunk;
        }
        return temp;
    }

}
