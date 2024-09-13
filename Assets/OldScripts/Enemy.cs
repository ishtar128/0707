using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public abstract class Enemy : Entity
{
    public GameObject bloodeffect;
    public GameObject floatnumber;
    public GameObject enemyWARNING;
    public float enemywarning_x;
    public float enemywarning_y;
    private GameObject enemywarning;
    public float speed;
    public float jumpspeed;
    public int health;
    public int damage;
    public float beAttackedTime = 10f; // 被攻击间隔
    private Vector2 dir;
    private bool isHit;

    protected float flashDuration = 0.1f; // 闪烁持续时间
    protected int flashCount = 3; // 闪烁次数
    protected SpriteRenderer spriteRenderer; // 精灵渲染器
    protected Color originalColor; // 原始颜色

    public float moveSpeed = 2f; // 敌人的移动速度
    public Transform player; // 玩家的Transform


    public enemyHealthBar EnemyHealthBar;


    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        EnemyHealthBar = GetComponentInChildren<enemyHealthBar>();
        EnemyHealthBar.healthMax = health;
        EnemyHealthBar.healthCurrent = health;

        enemywarning = Instantiate(enemyWARNING, transform.position, Quaternion.identity) as GameObject;
        enemywarning.GetComponent<enemyWarning>().enemyWarningScale_x = enemywarning_x;
        enemywarning.GetComponent<enemyWarning>().enemyWarningScale_y = enemywarning_y;

        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
    }

    // Update is called once per frame
    protected override void Update()
    {

        base.Update();
        if (isHit)
        {
            float randomFloat = Random.Range(0.99f, 1.01f);
            myRigidbody.velocity = dir * speed* randomFloat;
            myRigidbody.velocity += jumpspeed*Vector2.up* randomFloat;
            isHit = false;
        }
        EnemyHealthBar.healthCurrent = health;
        if (health <= 0)
        {
            Destroy(gameObject);
        }
        enemywarning.transform.position = transform.position;
        if(enemywarning.GetComponent<enemyWarning>().playerEnter!=null)
        {
            player = enemywarning.GetComponent<enemyWarning>().playerEnter.transform;
        }

    }
    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        AI();
        Flip();
        if(beAttackedTime>=-2)
        beAttackedTime--;
    }

    public void GetHit(Vector2 dirs,float theSpeed,float theJumpspeed)
    {
        transform.localScale = new Vector3(-dirs.x, 1, 1);
        isHit = true;
        this.dir = dirs;
        float oldspeed = speed;
        float oldjumpspeed = jumpspeed;
        if (theSpeed != 0)
            speed = theSpeed;
        else
            speed = oldspeed;
        if (theJumpspeed != 0)
            jumpspeed = theJumpspeed;
        else
            jumpspeed = oldjumpspeed;

    }
    public virtual void TakeDamage(int damage)
    {
        if(beAttackedTime<0)
        {
            GameObject gb = Instantiate(floatnumber, transform.position, Quaternion.identity) as GameObject;
            gb.transform.GetChild(0).GetComponent<TextMeshPro>().text = damage.ToString();
            Instantiate(bloodeffect, transform.position, Quaternion.identity);
            health -= damage;
            EnemyHealthBar.healthCurrent = health;
            beAttackedTime = 10f;
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

            // 恢复原始颜色
            spriteRenderer.color = originalColor;
            yield return new WaitForSeconds(flashDuration);

        }
    }
    public virtual void AI()
    {
        
        // 如果玩家对象存在
        if (player != null)
        {
            // 计算从敌人到玩家的水平方向
            Vector2 direction = new Vector2(player.position.x - transform.position.x, 0).normalized;
            // 移动敌人
            transform.Translate(direction * moveSpeed * Time.deltaTime);

        }

    }
    /*    protected override void Flip()
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
        }*/

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerHealth playerHealth = other.GetComponent<playerHealth>();
            if (playerHealth != null)
            {
                // 调用玩家受伤的方法
                playerHealth.TakeDamage(damage);
                // 计算从敌人到玩家的水平方向
                Vector2 direction = new Vector2(player.position.x - transform.position.x, 0).normalized;
                if (direction.x > 0)
                {
                    playerHealth.GetHit(Vector2.right);

                }
                else if (direction.x < 0)
                {
                    playerHealth.GetHit(Vector2.left);

                }

            }
            else
            {
                Debug.LogError("PlayerHealth component not found on player.");
            }
        }
    }








}
