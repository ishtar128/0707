using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class simpleEnemy : Enemy
{

    public override void AI()
    {
        if (beAttackedTime > 0)
            return;
        if (!isGround || isWallDetected)
            FlipSelf();
        myRigidbody.velocity = new Vector2(moveSpeed * transform.localScale.x, myRigidbody.velocity.y);
    }

    protected override void Flip()
    {

    }
    public void FlipSelf()
    {
        transform.localScale =new Vector2( -transform.localScale.x,1);
    }
}
