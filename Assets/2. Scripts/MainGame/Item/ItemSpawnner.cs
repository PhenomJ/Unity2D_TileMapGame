using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawnner : MapObject
{
    //Singleton
    static ItemSpawnner _instance;
    public static ItemSpawnner Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new ItemSpawnner();
                _instance.Init();
            }
            return _instance;
        }
    }

    void Init()
    {

    }

    public void ShowItem(Character character)
    {
        Item item = CreateItem();
        item.gameObject.transform.rotation = Quaternion.Euler(0.0f, 0.0f, Random.Range(0, 360));
        GameManager.Instance.GetMap().SetObj(character.GetTileX(), character.GetTileY(), item, eTileLayer.MIDDLE);
    }

    public Item CreateItem()
    {
        string filePath = "Prefabs/Items/items";

        GameObject itemPrefabs = Resources.Load<GameObject>(filePath);
        GameObject itemObj = GameObject.Instantiate(itemPrefabs);

        Sprite sprite = Resources.Load<Sprite>("Sprites/items");
        itemObj.GetComponent<SpriteRenderer>().sprite = sprite;

        Item item = itemObj.AddComponent<Item>();
        item.SetCanMove(false);

        return item;
    }
}
