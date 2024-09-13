using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    protected Rigidbody2D myRigidbody;
    protected Animator myAnim;
    protected bool isGround;
    protected bool isWallDetected;

    [Header("Collider info")]
    [SerializeField] protected Transform groundCheck;
    [SerializeField] protected float groundCheckDistance;
    //[SerializeField] protected LayerMask whatIsGround;
    [SerializeField] protected Transform wallCheck;
    [SerializeField] protected float wallCheckDistance;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        myAnim = GetComponent<Animator>();
        if (wallCheck == null)
            wallCheck = transform;
        if (groundCheck == null)
            groundCheck = transform;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        collidercheck();
        
        Flip();
    }
    protected virtual void FixedUpdate()
    {

    }
    protected virtual void collidercheck()
    {
        isGround = isground();
        isWallDetected = Physics2D.Raycast(wallCheck.position, new Vector2(1, 0) * transform.localScale.x, wallCheckDistance, LayerMask.GetMask("Ground"));
    }
    protected virtual bool isground()
    {
        //if (GroundTrigger.IsTouchingLayers(LayerMask.GetMask("Ground")))
        Debug.DrawRay(groundCheck.position, Vector2.down*groundCheckDistance, Color.blue);
        Debug.DrawRay(wallCheck.position, new Vector2(wallCheckDistance, 0)*transform.localScale.x, Color.red);
        if (Physics2D.Raycast(groundCheck.position,Vector2.down,groundCheckDistance, LayerMask.GetMask("Ground")))
            return true;
        else
            return false;

    }
    protected virtual void Flip()// 左右翻转的情况
    {
        bool playerHasXAxisSpeed = Mathf.Abs(myRigidbody.velocity.x) > Mathf.Epsilon;
        if (playerHasXAxisSpeed)
        {
            if (myRigidbody.velocity.x > 0.1f)
            {
                // transform.localRotation = Quaternion.Euler(0, 0, 0);
                transform.localScale = new Vector3(1, 1, 1);
            }

            if (myRigidbody.velocity.x < -0.1f)
            {
                // transform.localRotation = Quaternion.Euler(0, 180, 0);
                transform.localScale = new Vector3(-1, 1, 1);
            }
        }
    }

}
