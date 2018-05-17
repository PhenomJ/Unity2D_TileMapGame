using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State {
    protected eStateType _nextState;
    protected Character _character;

    public void Init (Character character)
    {
        _character = character;
    }

    virtual public void Start ()
    {
        _nextState = eStateType.NONE;
    }

    virtual public void Stop()
    {

    }

    virtual public void Update()
    {

    }

    public void NextState(eStateType nextState)
    {
        _nextState = nextState;
    }

    public eStateType GetNextState()
    {
        return _nextState;
    }
}

public enum eStateType
{
    IDLE,
    MOVE,
    ATTACK,
    NONE,
    DAMAGE,
    DEAD,
    DISCOVER,
    PATHFINDINGIDLE,
    PATHFINDINGTESTMOVE,
    PATHFINDING,
}
