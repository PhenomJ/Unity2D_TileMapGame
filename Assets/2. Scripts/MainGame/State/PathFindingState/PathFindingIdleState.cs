using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFindingIdleState : State {
    public override void Update()
    {
        TileCell targetTileCell = _character.GetTargetTileCell();

        if (targetTileCell != null)
        {
            _nextState = eStateType.PATHFINDING;
        }

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100))
            {
                if (hit.collider != null)
                {
                    MapObject map = hit.collider.gameObject.GetComponent<MapObject>();

                    if (map != null)
                    {
                        if (eMapObjectType.TILE_OBJECT == map.GetObjType())
                        {
                            TileCell target = GameManager.Instance.GetMap().GetTileCell(map.GetTileX(), map.GetTileY());

                            if(target.IsPathFindable() == true)
                            {
                                _character.SetTargetTileCell(target);
                            }
                        }
                    }
                }
            }
        }
    }
}
