using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class CameraFollow : MonoBehaviour
{
    public GameObject player;
    public Transform target; // 玩家的Transform组件
    public float smoothSpeed = 0.125f; // 相机跟随的平滑度
    public float dy;
    private void Start()
    {
        player=GameObject.FindGameObjectWithTag("Player");
        target = player.transform;
    }

    void LateUpdate()
    {
        if(!target.gameObject.activeSelf)
        player = GameObject.FindGameObjectWithTag("Player");
        target = player.transform  ;
        if (target != null)
        {
            Vector3 desiredPosition = target.position+ new Vector3(0, dy, 0);
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            transform.position = new Vector3(smoothedPosition.x, smoothedPosition.y, transform.position.z);
        }
    }
    public IEnumerator Shake(float duration, float magnitude)
    {


        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude+ transform.position.x;
            float y = Random.Range(-1f, 1f) * magnitude+ transform.position.y;

            transform.position = new Vector3(x, y, transform.position.z);

            elapsed += Time.deltaTime;

            yield return null;
        }


    }
}
