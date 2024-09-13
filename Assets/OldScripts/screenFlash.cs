using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class screenFlash : MonoBehaviour
{
    public Image img;
    public float time;
    public Color flashcolor;
    public Color defaultColor;
    // Start is called before the first frame update
    void Start()
    {
        defaultColor = img.color;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void flashScreen()
    {
        StartCoroutine(Flash());
    }
    IEnumerator Flash()
    {
        img.color = flashcolor;
        yield return new WaitForSeconds(time);
        img.color = defaultColor;
    }
}
