using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public List<GameObject> boats = new List<GameObject>();
    public List<GameObject> sailors = new List<GameObject>();
    // Start is called before the first frame update
    
    void Awake() {
        DontDestroyOnLoad(this.gameObject);
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddBoat(GameObject boat){
        boats.Add(boat);
    }

    public void AddSailor(GameObject sailor){
        sailors.Add(sailor);
    }

}
