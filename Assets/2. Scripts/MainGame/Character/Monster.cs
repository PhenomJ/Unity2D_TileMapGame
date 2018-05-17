using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : Character
{
    private void Awake()
    {
        _type = eMapObjectType.MONSTER;
        _hp = 10;
        _Exp = 200;
        _itemIndex = 0;
    }

    // Use this for initialization
    void Start()
    {
        
    }

    // Update is called once per frame
    override protected void InitState()
    {
        base.InitState();

        {
            State state = new MonsterIdleState();
            state.Init(this);
            _stateMap[eStateType.IDLE] = state;
        }

        _state = _stateMap[eStateType.IDLE];
    }
}
