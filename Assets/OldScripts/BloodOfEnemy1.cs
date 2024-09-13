using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodOfEnemy1 : MonoBehaviour
{
    public float timeToDestory=1;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, timeToDestory);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
