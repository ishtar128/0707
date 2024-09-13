using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Text healthNumber;
    public static int healthCurrent;
    public static int healthMax;

    private Image healthBar;
    // Start is called before the first frame update
    void Start()
    {
        healthBar = GetComponent<Image>();
        healthCurrent = healthMax;
    }

    // Update is called once per frame
    void Update()
    {
        healthBar.fillAmount = (float)healthCurrent / healthMax;
        healthNumber.text = healthCurrent.ToString() + "/" + healthMax.ToString();
    }
}
