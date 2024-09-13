using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class enemyHealthBar : MonoBehaviour
{
    public int healthCurrent;
    public int healthMax;

    private Image healthBar;

    private Enemy enemy;
    // Start is called before the first frame update
    void Start()
    {
        healthBar = GetComponent<Image>();
        healthCurrent = healthMax;
        enemy = GetComponentInParent<Enemy>();
    }

    // Update is called once per frame
    void Update()
    {
        healthBar.fillAmount = (float)healthCurrent / healthMax;

        transform.localScale = new Vector3(enemy.transform.localScale.x, 1, 1);
    }
}
