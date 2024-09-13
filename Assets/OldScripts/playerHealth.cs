using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerHealth : MonoBehaviour
{
    private Rigidbody2D myRigidbody;
    public float beAttackedTime = 50f; // ���������
    public bool isHit;
    public bool canBeHit=true;
    public bool isdied;
    public float speed;//�������ٶ�
    public float jumpspeed;
    private Vector2 dir;
    private float flashDuration = 0.1f; // ��˸����ʱ��
    private int flashCount = 3; // ��˸����
    private SpriteRenderer spriteRenderer; // ������Ⱦ��
    private Color originalColor; // ԭʼ��ɫ
    
    public CameraFollow cameraShake;
    public float shakeDuration = 0.05f;//�����ǿ��
    public float shakeMagnitude = 0.07f;//��ʱ��


    private Animator myAnim;

    //private screenFlash sf;

    public int health = 100;
    // Start is called before the first frame update
    void Start()
    {
        HealthBar.healthMax = health;
        HealthBar.healthCurrent = health;
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
        myRigidbody = GetComponent<Rigidbody2D>();
        myAnim = GetComponent<Animator>();
        //sf= GetComponent<screenFlash>();
        if (cameraShake == null)
        {
            cameraShake = Camera.main.GetComponent<CameraFollow>();
        }
    }

    // Update is called once per frame
    void Update()
    {

        if(canBeHit)
        {
            if (isHit)
            {
                //StartCoroutine(HandleHit());
                myRigidbody.velocity = Vector2.zero;
                myRigidbody.AddForce(jumpspeed * Vector2.up + dir * speed);
                isHit = false;
            }
        }


        HealthBar.healthCurrent = health;
        if (health<=0)
        {
            myAnim.SetBool("Die",true);
            isdied = true;
            GameController.instance.PlayerDied();
        }
        if (Input.GetKeyDown(KeyCode.R) && GameController.instance.isPlayerDead)
        {
            GameController.instance.PlayerRevived();
            isdied = false;
            health = HealthBar.healthMax;
            myAnim.SetBool("Die", false);
        }

    }
    //private IEnumerator HandleHit()
    //{
            // ��������ֱ�ٶ�
      //      myRigidbody.velocity = jumpspeed * Vector2.up;

            // �ȴ�һ��ʱ��
      //      yield return new WaitForSeconds(0.01f);

            // ������ˮƽ�ٶ�
     //       myRigidbody.velocity += dir * speed;
    //        isHit = false;
     //       yield break;
   // }

    private void FixedUpdate()
    {

        if (beAttackedTime >= -2)
            beAttackedTime--;
    }

    public void GetHit(Vector2 dirs)
    {
        if (beAttackedTime < 0 && !isdied&&canBeHit)
        {
            isHit = true;
            dir = dirs;
            beAttackedTime = 50f;
        }

    }
    public void TakeDamage(int damage)
    {
        if (beAttackedTime < 0&&!isdied&&canBeHit)
        {
            //sf.flashScreen();
            health -= damage;
            if (health < 0)
                health = 0;
            HealthBar.healthCurrent = health;

            StartCoroutine(cameraShake.Shake(shakeDuration, shakeMagnitude));
            Debug.Log("Player took damage. Current health: " + health);
            if (health <= 0)
            {
                Die();
            }
            StartCoroutine(FlashEffect());
        }

    }
    IEnumerator FlashEffect()
    {
        for (int i = 0; i < flashCount; i++)
        {

            // shan
            spriteRenderer.color = new Color(1f, 1f, 1f, 0f);
            yield return new WaitForSeconds(flashDuration);

            // �ָ�ԭʼ��ɫ
            spriteRenderer.color = originalColor;
            yield return new WaitForSeconds(flashDuration);

        }
    }


    void Die()
    {
        Debug.Log("Player died.");
        // ��������������������߼�
    }
}
