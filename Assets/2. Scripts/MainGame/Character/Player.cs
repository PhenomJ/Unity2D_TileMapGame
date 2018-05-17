using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    // Use this for initialization
    private void Awake()
    {
        _hp = 10;
        _Exp = 10;
		_pathfindingType = ePathFindingType.ASTAR;
    }

    void Start () {
        
    }

    protected override void InitState()
    {
        base.InitState();

        {
            State state = new PathFindingIdleState();
            state.Init(this);
            _stateMap[eStateType.IDLE] = state;
        }

        {
            State state = new PathFindingTestMoveState();
            state.Init(this);
            _stateMap[eStateType.MOVE] = state;
        }

        {
			//State state = new PathFindingImmediateState();
			State state = new PathFindingState();
			state.Init(this);
            _stateMap[eStateType.PATHFINDING] = state;
        }

        {
            State state = new DiscoverState();
            state.Init(this);
            _stateMap[eStateType.DISCOVER] = state;
        }

        _state = _stateMap[eStateType.IDLE];
    }
}
