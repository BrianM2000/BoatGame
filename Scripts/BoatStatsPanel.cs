using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoatStatsPanel : MonoBehaviour
{
    public GameObject boat;
    BoatAI b;
    public Slider healthSlider;
    public Slider MaxHealthSlider;
    TMPro.TMP_Text sailors;

    void Start()
    {
        Slider[] sliders = gameObject.GetComponentsInChildren<Slider>();
        sliders[0] = MaxHealthSlider;
        sliders[1] = healthSlider;
        b = boat.GetComponent<BoatAI>();
        TMPro.TMP_Text name = transform.Find("Name").GetComponent<TMPro.TMP_Text>();
        name.text = boat.name;
        sailors = transform.Find("Sailors").GetComponent<TMPro.TMP_Text>();

    }

    // Update is called once per frame
    void Update()
    {
        healthSlider.value = Mathf.Clamp(b.health, 0, b.baseMaxHealth)/b.baseMaxHealth;
        MaxHealthSlider.value = Mathf.Clamp(b.curMaxHealth, 0, b.baseMaxHealth)/b.baseMaxHealth;
        sailors.text = b.aliveSailorCount().ToString() + "/" + b.sailors.Length;
    }

    public void OnButtonPress(){
        Destroy(gameObject);
    }

}
