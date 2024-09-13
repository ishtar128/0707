using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Entity
{
    [Header("Jump info")]
    [SerializeField] private float fallMultiplier; // 下落乘数
    [SerializeField] private float lowJumpMultiplier; // 低跳乘数
    [SerializeField] private float normalgravity; // 重力
    [SerializeField] private float jumpSpeed;
    [SerializeField] private int canjumpcount;
    private int jumpCount;
    private bool jumpPressed;
    private int jumpPress;
    private bool ifjumpWhenisJump = true;

    [Header("Move info")]
    [SerializeField] private bool canRun=true;
    [SerializeField] private float runspeed;


    [Header("Dash info")]
    [SerializeField] private float dashSpeed;
    [SerializeField] private float dashDuration;
    [SerializeField] private float dashTime;
    [SerializeField] private float dashCD;
    private float dashNumber=1;
    public bool isDash=false;

    [Header("Attack info")]
    [SerializeField] private float attackCD;
    [SerializeField] private float attack2time=-2f;
    [SerializeField] private float attack3time = -2f;
    public bool isAttack;
    [SerializeField] private float attackRunSpeed;
    [SerializeField] private int attackDamage; // 攻击伤害
    private bool attack2Prepared = false;
    private bool attack3Prepared=false;
    private bool doNextAttack = true;
    private int baseAttackDamage;
    private float hitForce=0;

    [Header("Camera shake info")]
    [SerializeField] private CameraFollow cameraShake;
    [SerializeField] private float shakeDuration = 0.05f;//相机振动强度
    [SerializeField] private float shakeMagnitude = 0.07f;//震动时间

    private playerHealth health;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        if (cameraShake == null)
        {
            cameraShake = Camera.main.GetComponent<CameraFollow>();
        }
        health = GetComponent<playerHealth>();

        baseAttackDamage = attackDamage;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        if (health.isdied)
            return;

        getKeyDowmOfJump();
        Attack();
        Dash();

    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        if (!health.isdied)
        {
            if (canRun)
                RunAndDash();
            Jump();
        }
        if (attack2time >= -2)
        {
            attack2time--;
        }
        if (attack3time >= -2)
        {
            attack3time--;
        }
    }

    private void Dash()
    {
        myAnim.SetBool("isDashing", dashTime > 0);
        if (Input.GetKeyDown(KeyCode.LeftShift) && dashTime < dashDuration - dashCD && dashNumber > 0)
        {
            dashTime = dashDuration;
            dashNumber--;
        }
        dashTime -= Time.deltaTime;

        if (isGround)
        {
            dashNumber = 1;
        }

        if (dashTime > 0)
        {
            isDash = true;
            health.canBeHit = false;
        }
        else
        {
            if (isDash)
                health.canBeHit = true;
            isDash = false;
        }
    }

    private void getKeyDowmOfJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && jumpCount > 0)
        {
            jumpPressed = true;
        }
        if (Input.GetKeyDown(KeyCode.Space))//有预先输入
        {
            jumpPress = 10;
        }
    }



    void RunAndDash()
    {
        //float movedirs = Input.GetAxis("Horizontal");//-1------->1
        float movedir = Input.GetAxisRaw("Horizontal");//-1 0 1

        if (dashTime > 0)
        {
            myRigidbody.velocity = new Vector2( dashSpeed*transform.localScale.x, 0);
        }
        else
        {
        if (movedir!=0)
        {
            if (!isAttack)
                myRigidbody.velocity = new Vector2(movedir * runspeed, myRigidbody.velocity.y);
            else if(isAttack)
            {
                myRigidbody.velocity = new Vector2(movedir * attackRunSpeed, myRigidbody.velocity.y);
            }
        }
        else
        {
            if(isGround)
            {
                myRigidbody.velocity = new Vector2(movedir * runspeed, myRigidbody.velocity.y);
            }
        }
        }
        // myRigidbody.AddForce(new Vector2(movedirs , 0)*acc);
        //  if(Mathf.Abs(myRigidbody.velocity.x)>runspeed)
        //  {
        //     myRigidbody.velocity = new Vector2(Mathf.Sign(myRigidbody.velocity.x) * runspeed, myRigidbody.velocity.y);
        //
        
        //动画部分
        bool playerHasXAxisSpeed = Mathf.Abs(myRigidbody.velocity.x) > Mathf.Epsilon;
        if(Mathf.Abs(movedir) <0.7f)
            myAnim.SetBool("runend", true);
        else
            myAnim.SetBool("runend",false);
        myAnim.SetBool("run", playerHasXAxisSpeed);
    }

    void Jump()
    {
        jumpPress--;// 每帧减少预输入值 5》》》》0

        if (isGround)
        {
            jumpCount = canjumpcount;
            ifjumpWhenisJump = true;
        }

        if (jumpPressed && !isGround&& ifjumpWhenisJump && jumpCount > 0)
        {
            jumpCount --;
            ifjumpWhenisJump = false;
            //jumpPressed = false;
        }//直接下落的跳跃判断

        if (jumpPress>0&& isGround && jumpCount > 0)
        {
            // 只有在地面上才能执行跳跃
            myRigidbody.velocity = new Vector2(0, 0);
            myRigidbody.AddForce(Vector2.up * jumpSpeed, ForceMode2D.Impulse);

            jumpCount--;
            jumpPressed = false;
            jumpPress = 0;
            ifjumpWhenisJump = false;
        }
        else if(jumpPressed&&!isGround&&jumpCount>0)
        {   //二段跳

            myRigidbody.velocity = new Vector2(0, 0);
            myRigidbody.AddForce(Vector2.up * jumpSpeed, ForceMode2D.Impulse);

            jumpCount--;
            jumpPressed = false;
            ifjumpWhenisJump = false;
        }

        float verticalVelocity = myRigidbody.velocity.y;
        if (verticalVelocity < 0)
        {
            myRigidbody.gravityScale = fallMultiplier;
            myRigidbody.velocity = new Vector2(myRigidbody.velocity.x, Mathf.Max(myRigidbody.velocity.y, -runspeed*4));
        }
        else if (verticalVelocity > 0&&!Input.GetKey(KeyCode.Space))
        {
            myRigidbody.gravityScale = lowJumpMultiplier;
        }
        else
        {
            myRigidbody.gravityScale = normalgravity;
        }

        //动画部分
        if (verticalVelocity > Mathf.Epsilon)
        {
        // 触发跳跃动画
        myAnim.SetBool("IsJumping", true);
        myAnim.SetBool("IsFalling", false);
        }
        else if (verticalVelocity < -1f)
        {
            // 触发下落动画
            myAnim.SetBool("IsFalling", true);
            myAnim.SetBool("IsJumping", false);
        }

        if(isGround)
        {
            // 表示在地
            myAnim.SetBool("IsJumping", false);
            myAnim.SetBool("IsFalling", false);
            myAnim.SetBool("IsGround", true);
        }
        else
        {
            myAnim.SetBool("IsGround", false);
        }


    }

    void Attack()
    {
        if (dashTime > 0)
        {
            isAttack = false;
            attack2Prepared = false;
            attack3Prepared = false;
        }
        if ((Input.GetKeyDown(KeyCode.Mouse0)|| Input.GetButtonDown("Attack"))&& !isAttack&&attack2time<=0&& attack3time<=0&&doNextAttack&& dashTime <= 0)
        {
            isAttack = true;
            hitForce = 3;
            attackDamage = 1*baseAttackDamage;
            myAnim.SetTrigger("Attack");
            myAnim.SetBool("attackover", true);
            doNextAttack = false;
            StartCoroutine(NextAttack());
            attack2time = 30f;
        }
        else if ((Input.GetKeyDown(KeyCode.Mouse0) || Input.GetButtonDown("Attack")) && attack2time > 0)
        {
            attack2Prepared = true;    
        }
        else if ((Input.GetKeyDown(KeyCode.Mouse0) || Input.GetButtonDown("Attack")) && attack3time > 0)
        {
            attack3Prepared = true;
        }

        if (attack2Prepared&&doNextAttack)
        {
            isAttack = true;
            hitForce = 6;
            attackDamage = 1* baseAttackDamage;
            myAnim.SetTrigger("attack2");
            myAnim.SetBool("attackover", true);
            attack2time = -2f;
            attack2Prepared = false;
            doNextAttack = false;
            StartCoroutine(NextAttack());
            attack3time = 30f;
        }
        if (attack3Prepared && doNextAttack)
        {
            isAttack = true;
            hitForce = 9;
            attackDamage = 2* baseAttackDamage;
            myAnim.SetTrigger("attack3");
            myAnim.SetBool("attackover", true);
            health.canBeHit = false;
            //canRun = false;
            StartCoroutine(Attack3());
            attack3time = -2f;
            attack3Prepared = false;
            doNextAttack = false;
            StartCoroutine(NextAttack());
        }
    }
    private IEnumerator NextAttack()
    {   
        yield return new WaitForSeconds(0.5f); // 假设每次攻击需要0.5秒完成
        doNextAttack = true;
        canRun = true;
        health.canBeHit = true;
    }
    private IEnumerator Attack3()
    {
        yield return new WaitForSeconds(0.2f);
        myRigidbody.AddForce(30 * jumpSpeed * Vector2.up + 35 * new Vector2(transform.localScale.x, 0) * runspeed);
    }
    public void AttackOver()
    {
        isAttack = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
            {
            other.GetComponent<Enemy>().TakeDamage(attackDamage);
            StartCoroutine(cameraShake.Shake(shakeDuration, shakeMagnitude));
            if (transform.localScale.x>0)
            {
                other.GetComponent<Enemy>().GetHit(Vector2.right,2*hitForce, hitForce);
            }
            else if (transform.localScale.x < 0)
            {
                other.GetComponent<Enemy>().GetHit(Vector2.left, 2*hitForce, hitForce);
            }
        }
    }





}
