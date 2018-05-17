using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFindingTestMoveState : State {
    public override void Start()
    {
        base.Start();
		Debug.Log("Move");
        _character.PopPathFindingTileCell();
    }

    public override void Stop()
    {
        base.Stop();

        _character.ClearPathFindingTileCell();
    }

    public override void Update()
    {
        if (_character.EmptyPathFindingTileCell() == false)
        {
            TileCell tileCell = _character.PopPathFindingTileCell();

            sPosition curPosition;
            curPosition.x = _character.GetTileX();
            curPosition.y = _character.GetTileY();

            sPosition nextPosition;
            nextPosition.x = tileCell.GetTileX();
            nextPosition.y = tileCell.GetTileY();

            eMoveDirection direction = GetDirection(curPosition, nextPosition);
            _character.SetNextDirection(direction);

            if((_character.MoveStart(tileCell.GetTileX(), tileCell.GetTileY()) == false))
            {
                //_nextState = eStateType.ATTACK;
                _nextState = eStateType.DISCOVER;
            }
        }

        else
            _nextState = eStateType.IDLE;
    }

    eMoveDirection GetDirection(sPosition curPosition, sPosition nextPosition)
    {
        if (curPosition.x < nextPosition.x)
            return eMoveDirection.RIGHT;
        if (curPosition.x > nextPosition.x)
            return eMoveDirection.LEFT;
        if (curPosition.y < nextPosition.y)
            return eMoveDirection.DOWN;
        if (curPosition.y > nextPosition.y)
            return eMoveDirection.UP;

        return eMoveDirection.DOWN;
    }
}
