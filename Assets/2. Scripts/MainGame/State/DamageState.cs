using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageState : State
{

    public override void Start()
    {
        base.Start();

        int damage = _character.GetDamage();
        _character.DecreseHP(damage);
        _character.ShowEffect("ScratchEffect");

        if (_character.IsLive() == false)
        {
            _nextState = eStateType.DEAD;
        }

        else
            _nextState = eStateType.IDLE;
    }
}