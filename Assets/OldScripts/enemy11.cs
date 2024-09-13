using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class enemy11 : Enemy
{

    public float jumpForce = 5f; // 跳跃力
    public float jumpInterval = 100f; // 跳跃间隔
    private BoxCollider2D GroundTrigger;


    private PlayerController playerController;
    protected override void Start()
    {
        base.Start();
        GroundTrigger = GetComponent<BoxCollider2D>();
        damage = 5;
        faceForce = new Vector2(transform.localScale.x, 0);
    }

    private Vector2 beforeJumpPos;
    private bool canRememberAfterJumpPos;
    private Vector2 afterJumpPos;
    private Vector2 canrememberbeforepos = Vector2.zero;
    private Vector2 faceForce;
    private int groundTime=0;
    public float faceforce;
    private Vector2 direction = Vector2.zero;
    public override void AI()
     {

        float Force=1;


        // 如果玩家对象存在
        if (player != null)
        {
            // 计算从敌人到玩家的水平方向
            direction = new Vector2(player.position.x - transform.position.x, 0).normalized;

            if(Mathf.Abs(player.position.x - transform.position.x)<=7.4f)
            {
                Force = Mathf.Abs(player.position.x - transform.position.x) / 7.4f;
                //rb.velocity = new Vector2(rb.velocity.x* Mathf.Abs(player.position.x - transform.position.x)*1f, rb.velocity.y);
            }
        }
        else
        {
            if(direction==Vector2.zero)
            {
                float randomFloat = Random.Range(-1f, 1f);
                if (randomFloat >= 0)
                    direction = new Vector2(1, 0);
                else
                    direction = new Vector2(-1, 0);
                transform.localScale = new Vector3(direction.x, 1, 1);
            }


            Force = 1;
        }




        if (GroundTrigger.IsTouchingLayers(LayerMask.GetMask("Ground")))
        {
            groundTime++;
            if(jumpInterval>-2)
            jumpInterval--;




            if (canRememberAfterJumpPos && groundTime > 30)
            {
                direction = Vector2.zero;
                canrememberbeforepos = transform.position;
                afterJumpPos = transform.position;
                canRememberAfterJumpPos = false;
                if (Mathf.Abs(beforeJumpPos.x-afterJumpPos.x) < 0.5f && beforeJumpPos != Vector2.zero && afterJumpPos != Vector2.zero)
                {
                    faceForce.x = 5* transform.localScale.x;
                }
                else
                {
                    faceForce.x = transform.localScale.x;
                }
                faceforce = faceForce.x;

            }
        }
        else
        {
            groundTime = 0;
            canRememberAfterJumpPos = true;
            afterJumpPos = Vector2.zero;
            beforeJumpPos = canrememberbeforepos;

            myRigidbody.velocity = new Vector2(myRigidbody.velocity.x, myRigidbody.velocity.y) +0.01f*faceForce;


            Debug.Log(myRigidbody.velocity);
        }



        if (jumpInterval<=0&& GroundTrigger.IsTouchingLayers(LayerMask.GetMask("Ground")))
        {
            myRigidbody.velocity = new Vector2(myRigidbody.velocity.x, 0);
            float randomFloat = Random.Range(0.95f, 1.05f);
            myRigidbody.AddForce(Vector2.up * jumpForce+ direction*moveSpeed*Force* randomFloat + faceForce* randomFloat, ForceMode2D.Impulse);
            jumpInterval = 100f * randomFloat  ;

        }






    }
    public override void TakeDamage(int damage)
    {
        if (beAttackedTime < 0)
        {   
            GameObject gb = Instantiate(floatnumber, transform.position, Quaternion.identity) as GameObject;
            gb.transform.GetChild(0).GetComponent<TextMeshPro>().text = damage.ToString();

            Instantiate(bloodeffect, transform.position, Quaternion.identity);
            health -= damage;
            float randomFloat = Random.Range(0.95f, 1.05f);
            beAttackedTime = 10f;
            if(jumpInterval<=20f)
            jumpInterval = 20f* randomFloat;
            StartCoroutine(FlashEffect());
        }

    }
    IEnumerator FlashEffect()
    {
        for (int i = 0; i < flashCount; i++)
        {

            // shan
            spriteRenderer.color = new Color(1f,1f,1f,0f);
            yield return new WaitForSeconds(flashDuration);

            // 恢复原始颜色
            spriteRenderer.color = originalColor;
            yield return new WaitForSeconds(flashDuration);

        }
    }

    
    protected override void Flip()
    {

        // 如果玩家对象存在
        if (player != null)
        {
            if (Mathf.Abs(player.position.x - transform.position.x) >= 0.1f)
            {
                // 计算从敌人到玩家的水平方向
                Vector2 direction = new Vector2(player.position.x - transform.position.x, 0).normalized;
                transform.localScale = new Vector3(player.position.x > transform.position.x ? 1 : -1, 1, 1);
            }
        }
    }
    }
