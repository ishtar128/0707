using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState 
{
    protected PlayerStateMachine statemachine;
    protected Player player;

    private string animBoolName;

    public PlayerState(Player _player,PlayerStateMachine _statemachine,string _animBoolName)
    {
        this.player = _player;
        this.statemachine = _statemachine;
        this.animBoolName = _animBoolName;
    }

    public virtual void Enter()
    {
        Debug.Log("I enter " + animBoolName);
    }

    public virtual void Update()
    {
        Debug.Log("I am in " + animBoolName);
    }
    public virtual void Exit()
    {
        Debug.Log("I exit " + animBoolName);
    }
}
