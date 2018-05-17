using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileObject : MapObject {
 

	// Use this for initialization
	void Start () {
        _type = eMapObjectType.TILE_OBJECT;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Init(Sprite sprite)
    {
        gameObject.GetComponent<SpriteRenderer>().sprite = sprite;
    }
}
