using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MapObject  {

    private void Awake()
    {
        _type = eMapObjectType.ITEM;
    }

    public void Init (Character character)
    {
        
    }
}
