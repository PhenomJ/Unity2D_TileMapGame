using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum eMapObjectType
{
    NONE,
    MONSTER,
    ITEM,
    TILE_OBJECT,
}

public class MapObject : MonoBehaviour {

    protected int _tileX;
    protected int _tileY;

    public int GetTileX()
    {
        return _tileX;
    }

    public int GetTileY()
    {
        return _tileY;
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetPosition(Vector2 position)
    {
        gameObject.transform.localPosition = position;
    }

    //Sort
    virtual public void SetSortingOrder(eTileLayer layer, int sortingOrder)
    {
        _tileLayer = layer;
        int sortingID = SortingLayer.NameToID(layer.ToString());
        
        gameObject.GetComponent<SpriteRenderer>().sortingLayerID = sortingID;
        gameObject.GetComponent<SpriteRenderer>().sortingOrder = sortingOrder;
    }

    public void BecomeViewer()
    {
        Camera.main.transform.SetParent(transform);
        Camera.main.transform.localPosition = new Vector3(0, 0, Camera.main.transform.localPosition.z);
    }

    //Layer
    protected eTileLayer _tileLayer;

    public eTileLayer GetCurrentLayer()
    {
        return _tileLayer;
    }

    //CanMove
    protected bool _canMove = true;

    public bool CanMove()
    {
        return _canMove;
    }

    public void SetCanMove(bool canMove)
    {
        _canMove = canMove;
    }

    //Info
    protected eMapObjectType _type = eMapObjectType.NONE;

    public eMapObjectType GetObjType()
    {
        return _type;
    }

    //Message
    virtual public void ReceiverObjMessage(MessageParam msgParam)
    {
       
    }

    public void SetTilePosition(int x, int y)
    {
        _tileX = x;
        _tileY = y;
    }

    //Exp
    protected int _Exp;

    public int GetExp()
    {
        return _Exp;
    }
}
