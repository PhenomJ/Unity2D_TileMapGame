using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum eUpdateState
{
    PATHFINDING,
    BUILD,
}

public struct sPosition
{
    public int x;
    public int y;
}

public class PathFindingState : State {
	int count = 0;

    protected struct sPathCommand
    {
        public TileCell tileCell;
        public float heuristic;
    }

    protected List<sPathCommand> _pathFindingQueue = new List<sPathCommand>();
    TileCell _targetTileCell;
    protected TileCell _reverseTileCell = null;
    protected eUpdateState _updateState;

    public override void Start()
    {
        base.Start();

        _targetTileCell = _character.GetTargetTileCell();
        _updateState = eUpdateState.PATHFINDING;
        
        _reverseTileCell = null;

        if (_targetTileCell != null)
        {
            GameManager.Instance.GetMap().ResetPathFinding();
            TileCell startTileCell = GameManager.Instance.GetMap().GetTileCell(_character.GetTileX(), _character.GetTileY());

            sPathCommand command;
            command.tileCell = startTileCell;
            command.heuristic = 0.0f;
            PushCommand(command);
        }

        else
        {
            _nextState = eStateType.IDLE;
        }
    }

    public override void Stop()
    {
        base.Stop();

        _pathFindingQueue.Clear();
        _character.SetTargetTileCell(null);
    }

    public override void Update()
    {
        base.Update();

        switch(_updateState)
        {
            case eUpdateState.PATHFINDING:
                UpdatePathFinding();
                break;

            case eUpdateState.BUILD:
                UpdateBuild();
                break;
        }
    }

    public void UpdatePathFinding()
    {
        if (_pathFindingQueue.Count != 0)
        {
            sPathCommand command = PopCommand();

            if (command.tileCell.Marked() == false)
            {
                command.tileCell.Mark();
				if (command.tileCell.CanMove() == true)
					command.tileCell.SetPathFindingTestMark(Color.blue);

				else if (command.tileCell.CanMove() == false)
					command.tileCell.SetPathFindingTestMark(Color.red);

				if (command.tileCell.GetDistanceFromStart() > 5)
				{
					_reverseTileCell = command.tileCell;
					_updateState = eUpdateState.BUILD;
					return;
				}

				// 목표 도달?
				if (command.tileCell.GetTileX() == _targetTileCell.GetTileX() && command.tileCell.GetTileY() == _targetTileCell.GetTileY())
                {
                    _updateState = eUpdateState.BUILD;
					_reverseTileCell = _targetTileCell;
					return;
                }
				// 주변 타일 검사
				for (int direction = (int)eMoveDirection.UP; direction < (int)eMoveDirection.NONE; direction++)
				{
					sPosition currentTilePosition;
					currentTilePosition.x = command.tileCell.GetTileX();
					currentTilePosition.y = command.tileCell.GetTileY();

					sPosition nextTilePos = GetPositionByDirection(currentTilePosition, (eMoveDirection)direction);

					TileCell nextTileCell = GameManager.Instance.GetMap().GetTileCell(nextTilePos.x, nextTilePos.y);

					//nextTileCell.SetPathFindingTestMark(Color.red);


					// 검사 한타일인지 && 이동 가능한 타일 인지 && 갈수 없는 노드의 타입이 혹시 몬스터? -> 리팩토링하고싶다ㅏㅏ
					if ((nextTileCell.IsPathFindable() == true && nextTileCell.Marked() == false))
					{
						float distanceFromStart = command.tileCell.GetDistanceFromStart() + command.tileCell.GetDistanceWeight() + 1;
						//float heuristic = CalcAStarHeuristic(distanceFromStart, nextTileCell, _targetTileCell);
						float heuristic = 0;

						switch(_character.GetPathFindingType())
						{
							case ePathFindingType.DISTANCE:
								heuristic = distanceFromStart;
								break;

							case ePathFindingType.SIMPLE:
								heuristic = CalcSimpleHeuristic(command.tileCell, nextTileCell, _targetTileCell);
								break;

							case ePathFindingType.COMPLEX:
								heuristic = CalcComplexHeuristic(nextTileCell, _targetTileCell);
								break;

							case ePathFindingType.ASTAR:
								heuristic = CalcAStarHeuristic(distanceFromStart, nextTileCell, _targetTileCell);
								break;
						}

						if (nextTileCell.GetPrevTileCell() == null)
						{
							nextTileCell.SetDistanceFromStart(distanceFromStart);
							nextTileCell.SetPrevTileCell(command.tileCell);

							sPathCommand newCommand;
							newCommand.heuristic = heuristic;
							newCommand.tileCell = nextTileCell;
							PushCommand(newCommand);
						}
					}
				}
			}
        }
    }

    virtual public void UpdateBuild()
    {
        if(_reverseTileCell != null)
        {
            _character.PushPathFindingTileCell(_reverseTileCell);
            _reverseTileCell = _reverseTileCell.GetPrevTileCell();
        }

        else
        {
			//_nextState = eStateType.MOVE;
			_nextState = eStateType.IDLE;
        }
		
    }

    sPosition GetPositionByDirection(sPosition curPosition, eMoveDirection direction)
    {
		sPosition tilePosition = curPosition;

		switch (direction)
		{
			case eMoveDirection.LEFT:
				tilePosition.x--;
				break;

			case eMoveDirection.RIGHT:
				tilePosition.x++;
				break;

			case eMoveDirection.UP:
				tilePosition.y--;
				break;

			case eMoveDirection.DOWN:
				tilePosition.y++;
				break;
		}

		if (tilePosition.x < 0) tilePosition.x = 0;
        if (tilePosition.x > GameManager.Instance.GetMap().GetWidth() - 1) curPosition.x = GameManager.Instance.GetMap().GetWidth() - 1;
        if (tilePosition.y < 0) tilePosition.y = 0;
        if (tilePosition.y > GameManager.Instance.GetMap().GetHeight() - 1) curPosition.y = GameManager.Instance.GetMap().GetHeight() - 1;

        return tilePosition;
    }

    sPathCommand PopCommand()
    {
        sPathCommand command = _pathFindingQueue[0];
        _pathFindingQueue.RemoveAt(0);
        return command;
    }

    void PushCommand(sPathCommand command)
    {
        _pathFindingQueue.Add(command);

		////Sort heuristic값이 작은게 우선
		//_pathFindingQueue.Sort(delegate (sPathCommand c1, sPathCommand c2)
		//{
		//	if (c1.heuristic < c2.heuristic) return 1;
		//	if (c2.heuristic < c1.heuristic) return -1;
		//	return 0;
		//});
	}

    float CalcSimpleHeuristic(TileCell tileCell, TileCell nextTileCell, TileCell targetTileCell)
    {
        float heuristic = 0.0f;
        int deltaTileX = tileCell.GetTileX() - targetTileCell.GetTileX();
        deltaTileX = Mathf.Abs(deltaTileX);
        int nextDeltaTileX = nextTileCell.GetTileX() - targetTileCell.GetTileX();
        nextDeltaTileX = Mathf.Abs(nextDeltaTileX);

        if (deltaTileX < nextDeltaTileX)
            heuristic += 1.0f;
        else if (deltaTileX > nextDeltaTileX)
            heuristic -= 1.0f;

        int deltaTileY = tileCell.GetTileY() - targetTileCell.GetTileY();
        deltaTileY = Mathf.Abs(deltaTileY);
        int nextDeltaTileY = nextTileCell.GetTileY() - targetTileCell.GetTileY();
        nextDeltaTileY = Mathf.Abs(nextDeltaTileY);

        if (deltaTileY < nextDeltaTileY)
            heuristic += 1.0f;
        else if (deltaTileY > nextDeltaTileY)
            heuristic -= 1.0f;

        return heuristic;
    }

    float CalcComplexHeuristic(TileCell nextTileCell, TileCell targetTileCell)
    {
        int deltaX = nextTileCell.GetTileX() - targetTileCell.GetTileX();
        deltaX = deltaX * deltaX;

        int deltaY = nextTileCell.GetTileY() - targetTileCell.GetTileY();
        deltaY = deltaY * deltaY;

        return (float)(deltaX + deltaY);
    }

    float CalcAStarHeuristic(float distance, TileCell nextTileCell, TileCell targetTileCell)
    {
        return distance + CalcComplexHeuristic(nextTileCell, targetTileCell);
    }
}