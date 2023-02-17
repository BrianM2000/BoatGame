using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Counter : MonoBehaviour
{
    [SerializeField] TMPro.TMP_Text text;
    [SerializeField] PlayerManager manager;
    // Start is called before the first frame update
    void Start()
    {
        //manager = GameObject.Find("PlayerManager").GetComponent<PlayerManager>();
    }

    // Update is called once per frame
    void Update()
    {
        //text.text = manager.boats.Count.ToString();
    }
}
