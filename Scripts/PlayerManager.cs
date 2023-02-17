using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public List<GameObject> boats = new List<GameObject>();
    public List<GameObject> sailors = new List<GameObject>();
    public GameObject baseSailor;
    [SerializeField] TextAsset nameList;
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

    public GameObject generateNewSailor(){
        GameObject sailor = Instantiate(baseSailor, Vector3.zero, Quaternion.identity);
        sailor.name = ReadRandomLineFromFile();
        Sailor s = sailor.GetComponent<Sailor>();
        s.health = Round(Random.Range(75,125), 2);
        s.workSpeedModifier = Round(Random.Range(.75f, 1.25f), 2);
        return sailor;
    }

    float Round(float input, int numDeci){
        return Mathf.Round(input * (10f * numDeci))/(10f * numDeci);
    }

    string ReadRandomLineFromFile(){
        var endLine = new string[] {"\r\n", "\r", "\n"};
        var lines = nameList.text.Split(endLine, System.StringSplitOptions.RemoveEmptyEntries);

        int randLine = Random.Range(0, lines.Length);
        return lines[randLine];
    }

}
