using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveState : PlayerState
{
    public PlayerMoveState(Player _player, PlayerStateMachine _statemachine, string _animBoolName) : base(_player, _statemachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        if (Input.GetKeyDown(KeyCode.N))
            statemachine.ChangeState(player.idleState);
    }
}
