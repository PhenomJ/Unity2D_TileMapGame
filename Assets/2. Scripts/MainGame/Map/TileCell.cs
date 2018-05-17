using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum eTileLayer
{
    GROUND,
    MIDDLE,
    MAXCOUNT,
}

public class TileCell{

    Vector2 _position;
    List<List<MapObject>> _mapObj = new List<List<MapObject>>();
    eTileLayer _layer;

    public void Init()
    {
        for (int i = 0; i < (int)eTileLayer.MAXCOUNT; i++)
        {
            List<MapObject> tileObjList = new List<MapObject>();
            _mapObj.Add(tileObjList);
        }
    }

    public void SetPosition(float x, float y)
    {
        _position.x = x;
        _position.y = y;
    }

    public void AddObject(eTileLayer layer, MapObject mapObject)
    {
        List<MapObject> mapObjList = _mapObj[(int)layer];
        _layer = layer;
        int sortingOrder = mapObjList.Count;
        mapObject.SetSortingOrder(layer, sortingOrder);
        mapObject.SetPosition(_position);

        mapObjList.Add(mapObject);
    }

    public void RemoveObj(MapObject mapObj)
    {
        List<MapObject> mapObjList = _mapObj[(int)mapObj.GetCurrentLayer()];
        mapObjList.Remove(mapObj);
    }

    public bool CanMove()
    {
        for (int layer = 0; layer < (int)eTileLayer.MAXCOUNT; layer++)
        {
            List<MapObject> mapObjList = _mapObj[layer];
            
            for (int i = 0; i < mapObjList.Count; i++)
            {
                if (mapObjList[i].CanMove() == false)
                {
                    return false;
                }
            }
        }
        return true;
    }

    public List<MapObject> GetCollisionList()
    {
        List<MapObject> collisionList = new List<MapObject>();

        for (int layer = 0; layer < (int)eTileLayer.MAXCOUNT; layer++)
        {
            List<MapObject> mapObjList = _mapObj[layer];
            
            for (int i = 0; i < mapObjList.Count; i++)
            {
                if (mapObjList[i].CanMove() == false)
                {
                    collisionList.Add(mapObjList[i]);
                }
            }
        }
        return collisionList;
    }

    //PathFinding

    bool _mark;
    TileCell _prevTileCell;

    public void ResetPathFinding()
    {
        _mark = false;
        _distanceFromStart = 0.0f;
        _prevTileCell = null;
    }

    public bool Marked()
    {
        return _mark;
    }

    public void Mark()
    {
        _mark = true;
    }

    float _distanceFromStart;
    float _distanceFromWeight;

    public float GetDistanceFromStart()
    {
        return _distanceFromStart;
    }

    public void SetDistanceFromStart(float distanceFromStart)
    {
        _distanceFromStart = distanceFromStart;
    }

    public float GetDistanceWeight()
    {
        return _distanceFromWeight;
    }

    int _tileX;
    int _tileY;

    public int GetTileX()
    {
        return _tileX;
    }

    public int GetTileY()
    {
        return _tileY;
    }

    public void SetTile(int x, int y)
    {
        _tileX = x;
        _tileY = y;
    }

    public void SetPathFindingTestMark(Color color)
    {
        _mapObj[(int)eTileLayer.GROUND][0].gameObject.GetComponent<SpriteRenderer>().color = color;
    }

    public void ResetPathFindingTestMark()
    {
        _mapObj[(int)eTileLayer.GROUND][0].gameObject.GetComponent<SpriteRenderer>().color = Color.white;
    }

    public void SetPrevTileCell(TileCell prevTileCell)
    {
        _prevTileCell = prevTileCell;
    }

    public TileCell GetPrevTileCell()
    {
        return _prevTileCell;
    }
    
    public eTileLayer GetLayer()
    {
        return _layer;
    }

    public bool IsPathFindable()
    {
        if (_layer == eTileLayer.MIDDLE && CanMove() == false)
            return false;

        return true;
    }
}
