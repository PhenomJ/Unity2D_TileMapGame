using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiscoverState : State {

    public override void Start()
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

        TileMap map = GameManager.Instance.GetMap();
        List<MapObject> collisionList = map.GetCollisionList(moveX, moveY);

        if (_character.IsCoolDown() == true)
        {
            for (int i = 0; i < collisionList.Count; i++)
            {
                switch (collisionList[i].GetObjType())
                {
                    case eMapObjectType.MONSTER:
                        Debug.Log("Attack");
                        _character.Attack(collisionList[i]);
                        break;

                    case eMapObjectType.ITEM:
                        Debug.Log("Pick");
                        collisionList[i].SetCanMove(true);
                        GameObject.Destroy(collisionList[i].gameObject);
                        break;
                }
            }
        }

        _character.SetNextDirection(eMoveDirection.NONE);
        _nextState = eStateType.IDLE;
    }
}