using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFindingBuildState : State {
    TileCell reverseTileCell;

    public override void Start()
    {
        base.Start();
        Debug.Log("buildState");
        reverseTileCell = _character.GetTargetTileCell();
    }

    public override void Update()
    {
		if (reverseTileCell != null)
		{
			_character.PushPathFindingTileCell(reverseTileCell);
			//reverseTileCell.SetPathFindingTestMark();
			reverseTileCell = reverseTileCell.GetPrevTileCell();
			//_nextState = eStateType.BATTLE;
		}

		else
			_nextState = eStateType.PATHFINDINGTESTMOVE;
			//_nextState = eStateType.IDLE;
    }
}
