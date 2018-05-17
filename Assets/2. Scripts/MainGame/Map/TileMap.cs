using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileMap : MonoBehaviour {

    // Use this for initialization
    void Start() {
    }

    // Update is called once per frame
    void Update() {

    }

    //Sprite List
    Sprite[] _spriteArray;

    public void Init()
    {
        _spriteArray = Resources.LoadAll<Sprite>("Sprites/Cave01");
        //CreateTiles();
        CreateRandomMaps();
    }

    // Tile

    public GameObject TileObjPrefabs;

    int _width;
    int _height;

    TileCell[,] _tileCellList;

    public TileCell GetTileCell(int x, int y)
    {
        return _tileCellList[y, x];
    }

    void CreateTiles()
    {
        float _tileSize = 32.0f;

        TextAsset scriptAsset = Resources.Load<TextAsset>("Data/NewMapData1_layer1");
        string[] records = scriptAsset.text.Split('\n');

        {
            string[] token = records[0].Split(',');
            _width = int.Parse(token[1]);
            _height = int.Parse(token[2]);
        }
        _tileCellList = new TileCell[_height, _width];

        for (int y = 0; y < _height; y++)
        {
            int line = y + 2;
            string[] token = records[line].Split(',');

            for (int x = 0; x < _width; x++)
            {
                int spriteIndex = int.Parse(token[x]);

                GameObject tileGameObj = GameObject.Instantiate(TileObjPrefabs);
                tileGameObj.transform.SetParent(transform);
                tileGameObj.transform.localScale = Vector3.one;
                tileGameObj.transform.position = Vector3.zero;
                
                TileObject tileObject = tileGameObj.GetComponent<TileObject>();
                tileObject.Init(_spriteArray[spriteIndex]);
                tileObject.SetTilePosition(x, y);
                _tileCellList[y, x] = new TileCell();
                GetTileCell(x, y).Init();
                GetTileCell(x, y).SetPosition(x * _tileSize / 100.0f, -(y * _tileSize / 100.0f));
                GetTileCell(x, y).SetTile(x, y);
                GetTileCell(x, y).AddObject(eTileLayer.GROUND, tileObject);
            }
        }

        //scriptAsset = Resources.Load<TextAsset>("Data/NewMapData1_layer2");
        //records = scriptAsset.text.Split('\n');

        //for (int y = 0; y < _height; y++)
        //{
        //    int line = y + 2;
        //    string[] token = records[line].Split(',');

        //    for (int x = 0; x < _width; x++)
        //    {
        //        int spriteIndex = int.Parse(token[x]);

        //        if (0 <= spriteIndex)
        //        {
        //            GameObject tileGameObj = GameObject.Instantiate(TileObjPrefabs);
        //            tileGameObj.transform.SetParent(transform);
        //            tileGameObj.transform.localScale = Vector3.one;
        //            tileGameObj.transform.position = Vector3.zero;

        //            TileObject tileObject = tileGameObj.GetComponent<TileObject>();
        //            tileObject.Init(_spriteArray[spriteIndex]);
        //            tileObject.SetCanMove(false);
        //            GetTileCell(x, y).AddObject(eTileLayer.GROUND, tileObject);
        //        }
        //    }
        //}
    }

    public void CreateRandomMaps()
    {
        float _tileSize = 32.0f;

        TextAsset scriptAsset = Resources.Load<TextAsset>("Data/NewMapData1_layer1");
        string[] records = scriptAsset.text.Split('\n');

        {
            string[] token = records[0].Split(',');
            _width = int.Parse(token[1]);
            _height = int.Parse(token[2]);
        }
        _tileCellList = new TileCell[_height, _width];

        for (int y = 0; y < _height; y++)
        {
            int line = y + 2;
            string[] token = records[line].Split(',');

            for (int x = 0; x < _width; x++)
            {
                int spriteIndex = int.Parse(token[x]);

                GameObject tileGameObj = GameObject.Instantiate(TileObjPrefabs);
                tileGameObj.transform.SetParent(transform);
                tileGameObj.transform.localScale = Vector3.one;
                tileGameObj.transform.position = Vector3.zero;

                TileObject tileObject = tileGameObj.GetComponent<TileObject>();
                tileObject.Init(_spriteArray[spriteIndex]);
                tileObject.SetTilePosition(x, y);
                _tileCellList[y, x] = new TileCell();
                GetTileCell(x, y).Init();
                GetTileCell(x, y).SetPosition(x * _tileSize / 100.0f, -(y * _tileSize / 100.0f));
                GetTileCell(x, y).SetTile(x, y);
                GetTileCell(x, y).AddObject(eTileLayer.GROUND, tileObject);
            }
        }

        {
            int interval = 3;

            for (int y = 0; y < _height; y++)
            {
                if (y % interval == 0)
                {
                    for (int x = 0; x < _width; x++)
                    {
                        if (x % interval == 0)
                        {
                            int spriteIndex = 7;

                            GameObject tileGameObj = Instantiate(TileObjPrefabs);
                            tileGameObj.transform.SetParent(transform);
                            tileGameObj.transform.localScale = Vector3.one;
                            tileGameObj.transform.localPosition = Vector3.zero;

                            GameObject lightObj = Instantiate(LightObjPrefabs);
                            AddLighting(lightObj, tileGameObj);
                            

                            TileObject tileObj = tileGameObj.GetComponent<TileObject>();
                            tileObj.Init(_spriteArray[spriteIndex]);
                            tileObj.SetCanMove(false);

                            GetTileCell(x, y).AddObject(eTileLayer.GROUND, tileObj);
                        }
                    }

                    
                }
            }
        }

        for (int y = 0; y < _height; y++)
        {
            for (int x = 0; x < _width; x++)
            {
                if(GetTileCell(x,y).CanMove() == false)
                {
                    if (IsConnectedCell(x, y) == false)
                    {
                        eMoveDirection direction = (eMoveDirection)Random.Range(1, (int)eMoveDirection.DOWN);

                        int searchTileX = x;
                        int searchTileY = y;

                        while (IsConnectedCell(searchTileX, searchTileY) == false)
                        {
                            switch (direction)
                            {
                                case eMoveDirection.LEFT:
                                    searchTileX--;
                                    break;

                                case eMoveDirection.RIGHT:
                                    searchTileX++;
                                    break;

                                case eMoveDirection.UP:
                                    searchTileY--;
                                    break;

                                case eMoveDirection.DOWN:
                                    searchTileY++;
                                    break;
                            }

							if (0 <= searchTileX && searchTileX < _width && 0 <= searchTileY && searchTileY <= _height)
							{
								int spriteIndex = 7;

								GameObject tileGameObj = Instantiate(TileObjPrefabs);
								tileGameObj.transform.SetParent(transform);
								tileGameObj.transform.localScale = Vector3.one;
								tileGameObj.transform.localPosition = Vector3.zero;

								TileObject tileObj = tileGameObj.GetComponent<TileObject>();
								tileObj.Init(_spriteArray[spriteIndex]);
								tileObj.SetCanMove(false);

								GetTileCell(searchTileX, searchTileY).AddObject(eTileLayer.GROUND, tileObj);
							}
						}
                    }
                }
            }
        }
    }

    public bool IsConnectedCell(int x, int y)
    {
        for (int direction = (int)eMoveDirection.LEFT; direction <= (int)eMoveDirection.NONE; direction++)
        {
            int searchTileX = x;
            int searchTileY = y;

            switch((eMoveDirection)direction)
            {
                case eMoveDirection.LEFT:
                    searchTileX--;
                    break;

                case eMoveDirection.RIGHT:
                    searchTileX++;
                    break;

                case eMoveDirection.UP:
                    searchTileY--;
                    break;

                case eMoveDirection.DOWN:
                    searchTileY++;
                    break;
            }

            if (0 <= searchTileX && searchTileX < _width && 0 <= searchTileY && searchTileY < _height)
            {
                if (GetTileCell(searchTileX, searchTileY).IsPathFindable() == false)
                    return true;
            }

            else
                return true;
        }

        return false;
    }

    public int GetWidth()
    {
        return _width;
    }

    public int GetHeight()
    {
        return _height;
    }

    //Move

    public bool CanMoveTile(int tileX, int tileY)
    {
        if (tileX < 0 || _width <= tileX)
            return false;

        else if (tileY < 0 || _height <= tileY)
            return false;

        TileCell tileCell = GetTileCell(tileX, tileY);
        return tileCell.CanMove();
    }

    public void ResetObj(int tileX, int tileY, MapObject mapObj)
    {
        TileCell tileCell = GetTileCell(tileX, tileY);
        tileCell.RemoveObj(mapObj);
    }

    public void SetObj(int tileX, int tileY, MapObject mapObj, eTileLayer layer)
    {
        TileCell tileCell = GetTileCell(tileX, tileY);
        tileCell.AddObject(layer, mapObj);
    }

    public List<MapObject> GetCollisionList(int tileX, int tileY)
    {
        if (tileX < 0 || _width <= tileX)
            return null;

        else if (tileY < 0 || _height <= tileY)
            return null;

        TileCell tileCell = GetTileCell(tileX, tileY);
        return tileCell.GetCollisionList();
    }

    public void ResetPathFinding()
    {
        for (int y = 0; y < _height; y++)
        {
            for (int x = 0; x < _width; x++)
            {
                GetTileCell(x, y).ResetPathFinding();
            }
        }
    }

    // Light

    public GameObject LightObjPrefabs;

    void AddLighting (GameObject lightObj, GameObject parent)
    {
        lightObj.AddComponent<Light>();
        lightObj.GetComponent<Light>().type = LightType.Point;
        lightObj.GetComponent<Light>().lightmapBakeType = LightmapBakeType.Realtime;
        lightObj.GetComponent<Light>().intensity = 5.0f;
        lightObj.GetComponent<Light>().color = Color.gray;
        lightObj.GetComponent<Light>().range = 0.5f;
        lightObj.transform.SetParent(parent.transform);
        lightObj.transform.localScale = Vector3.one;
        lightObj.transform.localPosition = new Vector3(0, 0, -0.2f);
    }
}
