using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveState : State {

    override public void Start()
    {
        base.Start();

        int moveX = _character.GetTileX();
        int moveY = _character.GetTileY();

        switch (_character.GetNextDirection())
        {
            case eMoveDirection.UP:
                moveY--;
                break;

            case eMoveDirection.DOWN:
                moveY++;
                break;

            case eMoveDirection.LEFT:
                moveX--;
                break;

            case eMoveDirection.RIGHT:
                moveX++;
                break;
        }

        if (_character.MoveStart(moveX, moveY) == false)
        {
            _nextState = eStateType.ATTACK;

            /*
            TileMap map = GameManager.Instance.GetMap();
            List<MapObject> collisionList = map.GetCollisionList(moveX, moveY);

            for (int i = 0; i < collisionList.Count; i++)
            {
                switch (collisionList[i].GetObjType())
                {
                    case eMapObjectType.MONSTER:
                        _character.Attack(collisionList[i]);
                        break;
                }
            }
            */
        }

        else
        {
            _character.SetNextDirection(eMoveDirection.NONE);
            _nextState = eStateType.IDLE;
        }
    }
}
