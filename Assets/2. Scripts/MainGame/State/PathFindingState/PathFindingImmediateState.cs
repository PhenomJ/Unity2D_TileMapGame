using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFindingImmediateState : PathFindingState {
    public override void Start()
    {
        base.Start();

        while (_pathFindingQueue.Count != 0)
        {
            if (_updateState == eUpdateState.BUILD)
                break;

            UpdatePathFinding();
        }

        while (_nextState != eStateType.MOVE)
        {
            UpdateBuild();
        }
    }

    public override void Update()
    {
    }
}