using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyHealthCanBeSeen : MonoBehaviour
{
    private Enemy enemy;
    private int healthMax;
    public GameObject bar;
    // Start is called before the first frame update
    void Start()
    {
        enemy = GetComponent<Enemy>();
        healthMax = enemy.health;
    }

    // Update is called once per frame
    void Update()
    {
        if(enemy.health==healthMax)
        {
            bar.SetActive(false);
        }
        else
        {
            bar.SetActive(true);
        }
    }
}
