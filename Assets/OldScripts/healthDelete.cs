using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class healthDelete : MonoBehaviour
{
    private Image healthBar;
    private float oldHealth;
    private float newHealth;
    // Start is called before the first frame update
    void Start()
    {
        oldHealth = HealthBar.healthCurrent;
        newHealth = HealthBar.healthCurrent;
        healthBar = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {

        healthBar.fillAmount = (float)oldHealth / HealthBar.healthMax;
    }
    private void FixedUpdate()
    {
        if (HealthBar.healthCurrent != newHealth)
        {
            newHealth = HealthBar.healthCurrent;
        }
        if (oldHealth > newHealth)
        {
            oldHealth-=0.1f;
        }
        if (oldHealth < newHealth)
        {
            oldHealth=newHealth;
        }
    }
}
