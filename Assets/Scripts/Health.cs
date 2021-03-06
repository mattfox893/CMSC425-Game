using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public float health;
    public float maxHealth;

    public GameObject healthUI;
    public Slider healthSlider;
    public Unit unit;

    // Start is called before the first frame update
    void Start()
    {
        maxHealth = unit.unitStats.Health;
        health = maxHealth;
        healthSlider.value = SliderHealth();
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(maxHealth);
        //Debug.Log(health);

        health = unit.GetHealth();
        //Debug.Log(healthSlider);
        healthSlider.value = SliderHealth();
        
        //No overflow health
        health = Mathf.Min(health, maxHealth);
    }

    float SliderHealth()
    {
        return health / maxHealth;
    }
}
