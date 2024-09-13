using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyWarning : MonoBehaviour
{
    public GameObject playerEnter;
    public float enemyWarningScale_x=1;
    public float enemyWarningScale_y = 1;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(transform.localScale.x!= enemyWarningScale_x|| transform.localScale.y != enemyWarningScale_y)
        transform.localScale =  new Vector3(enemyWarningScale_x, enemyWarningScale_y, 1);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerEnter = other.gameObject;
        }
    }
}
