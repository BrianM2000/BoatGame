using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoatManager : MonoBehaviour
{
    public static List<GameObject> boatList = new List<GameObject>();
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
    void Start() {
        pause = true;
        timer = 0;

        playerManager = GameObject.Find("PlayerManager");
        pm = playerManager.GetComponent<PlayerManager>();

        foreach(GameObject s in pm.sailors){
            GameObject sailor = Instantiate(s, Vector3.zero, Quaternion.identity);
            sailor.SetActive(false);
            sailors.Add(sailor);
        }
    }

    void Update(){
        if(pause){
            return;
        }

        if(tokens <= 0){
            return;
        }

        if(timer <= 0f){
            timer = Random.Range(minTime, maxTime);
            float randX = Mathf.Sign(Random.Range(-1,1))*Random.Range(0, width);
            float randY = Mathf.Sign(Random.Range(-1,1))*Random.Range(height, height + 10);
            GameObject b = Instantiate(boat,new Vector3(randX, 0, randY), Quaternion.Euler(Vector3.zero));
            b.tag = "EnemyBoat";
            BoatAI ba = b.GetComponent<BoatAI>();
            float randSailors = Random.Range(1, ba.maxSailors);
            ba.curSailors = randSailors;
            ba.InstantiateBasicSailors();
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
            rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        }

    }

}
