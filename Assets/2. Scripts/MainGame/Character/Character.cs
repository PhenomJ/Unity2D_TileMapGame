using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public enum eMoveDirection
{
	UP,
	DOWN,
	LEFT,
    RIGHT,
    NONE,
}

public enum ePathFindingType
{
	DISTANCE,
	SIMPLE,
	COMPLEX,
	ASTAR,
};


public class Character : MapObject {
    protected GameObject _characterView;

    protected int _attackPoint = 5;

    protected int _level = 1;
    protected int _levelUPExp = 500;
    protected int _currentExp = 0;

    private void Awake()
    {
        
    }

    // Use this for initialization
    void Start () {
        
    }

	// Update is called once per frame
	void Update () {
        if (EventSystem.current.IsPointerOverGameObject())
            return;

        if (_state.GetNextState() != eStateType.NONE)
            ChangeState(_state.GetNextState());

        _coolDownDuration += Time.deltaTime;
        _state.Update();

        UpdateHPGuage();
        UpdateCoolDownGuage();
        LevelUP();
    }

    public void Init(string viewName)
    {
        // Attach View
        string filePath = "Prefabs/CharacterView/" + viewName;
        GameObject characterViewPrefabs = Resources.Load<GameObject>(filePath);

        _characterView = GameObject.Instantiate(characterViewPrefabs);
        _characterView.transform.SetParent(transform);
        _characterView.transform.localPosition = Vector3.zero;
        _characterView.transform.localScale = Vector3.one;

        TileMap map = GameManager.Instance.GetMap();

        _tileX = Random.Range(1, map.GetWidth() - 2);
        _tileY = Random.Range(1, map.GetHeight() - 2);

        while (map.CanMoveTile(_tileX, _tileY) == false)
        {
            _tileX = Random.Range(1, map.GetWidth() - 2);
            _tileY = Random.Range(1, map.GetHeight() - 2);
        }

        map.SetObj(_tileX, _tileY, this, eTileLayer.MIDDLE);

        SetCanMove(false);

        InitState();
        InitCharacterStatus();
    }

    override public void SetSortingOrder(eTileLayer layer, int sortingOrder)
    {
        _tileLayer = layer;
        int sortingID = SortingLayer.NameToID(layer.ToString());
        _characterView.GetComponent<SpriteRenderer>().sortingLayerID = sortingID;
        _characterView.GetComponent<SpriteRenderer>().sortingOrder = sortingOrder;
    }

    override public void ReceiverObjMessage(MessageParam msgParam)
    {
        switch (msgParam.message)
        {
            case "Attack":
                _damage = msgParam.attackPoint;
                _state.NextState(eStateType.DAMAGE);
                break;
        }
    }

    protected int _hp;
    protected bool _isLive = true;

    public void Attack(MapObject enemy)
    {
        ResetCoolDown();
        MessageParam msgParam = new MessageParam();
        msgParam.sender = this;
        msgParam.receiver = enemy;
        msgParam.message = "Attack";
        msgParam.attackPoint = _attackPoint;
        _currentExp += enemy.GetExp();
        ExpManager.Instance.SaveCurExp(_currentExp);

        MessageSystem.Instance.Send(msgParam);
    }

    int _damage;
    public int GetDamage()
    {
        return _damage;
    }

    public void DecreseHP(int damage)
    {
        _characterView.GetComponent<SpriteRenderer>().color = Color.red;
        Invoke("ResetColor", 0.1f);

        _hp -= damage;

        if (_hp < 0)
        {
            _hp = 0;
            _isLive = false;
        }
    }

    void ResetColor()
    {
        _characterView.GetComponent<SpriteRenderer>().color = Color.white;
    }

    public bool IsLive()
    {
        return _isLive;
    }

    // State
    protected Dictionary<eStateType, State> _stateMap = new Dictionary<eStateType, State>();
    protected State _state;

    virtual protected void InitState()
    {
        {
            State state = new IdleState();
            state.Init(this);
            _stateMap[eStateType.IDLE] = state;
        }

        {
            State state = new MoveState();
            state.Init(this);
            _stateMap[eStateType.MOVE] = state;
        }

        {
            State state = new AttackState();
            state.Init(this);
            _stateMap[eStateType.ATTACK] = state;
        }

        {
            State state = new DamageState();
            state.Init(this);
            _stateMap[eStateType.DAMAGE] = state;
        }

        {
            State state = new DeadState();
            state.Init(this);
            _stateMap[eStateType.DEAD] = state;
        }

        _state = _stateMap[eStateType.IDLE];
    }

    void ChangeState(eStateType nextState)
    {
        if (_state != null)
            _state.Stop();

        _state.Stop();
        _state = _stateMap[nextState];
        _state.Start();
    }

    public bool MoveStart(int tileX, int tileY)
    {
        string animationTrigger = "down";
        
        switch (_nextDirection)
        {
            case eMoveDirection.UP:
                animationTrigger = "up";
                break;

            case eMoveDirection.DOWN:
                animationTrigger = "down";
                break;

            case eMoveDirection.LEFT:
                animationTrigger = "left";
                break;

            case eMoveDirection.RIGHT:
                animationTrigger = "right";
                break;
        }

        _characterView.GetComponent<Animator>().SetTrigger(animationTrigger);

        TileMap map = GameManager.Instance.GetMap();
        List<MapObject> collisionList = map.GetCollisionList(tileX, tileY);

        if (collisionList.Count == 0)
        {
            map.ResetObj(_tileX, _tileY, this);
            _tileX = tileX;
            _tileY = tileY;
            map.SetObj(_tileX, _tileY, this, eTileLayer.MIDDLE);
            return true;
        }

        return false;
    }

    eMoveDirection _nextDirection = eMoveDirection.NONE;

    public void SetNextDirection (eMoveDirection nextDirection)
    {
        _nextDirection = nextDirection;
    }

    public eMoveDirection GetNextDirection ()
    {
        return _nextDirection;
    }

    //CoolDown
    float _coolDownTime = 0.5f;
    float _coolDownDuration = 0.0f;

    public bool IsCoolDown()
    {
        if (_coolDownDuration <= _coolDownTime)
        {
            return false;
        }

        else
        {
            return true;
        }
    }

    public void ResetCoolDown()
    {
        _coolDownDuration = 0.0f;
    }

    // UI

    Slider _hpGuage;

    public void LinkHPGuage(Slider hpGuage)
    {
        GameObject canvasObj = transform.Find("Canvas").gameObject;
        hpGuage.transform.SetParent(canvasObj.transform);
        hpGuage.transform.localPosition = new Vector3 (0.0f, 0.5f, 0.0f);
        hpGuage.transform.localScale = Vector3.one;

        TileMap map = GameManager.Instance.GetMap();

        _hpGuage = hpGuage;
        _hpGuage.value = (_hp / 10.0f);
        
    }

    void UpdateHPGuage ()
    {
        _hpGuage.value = (_hp / 10.0f);
    }

    Slider _coolDownGuage;

    public void LinkCoolDownGuage(Slider coolDownGuage)
    {
        GameObject canvasObj = transform.Find("Canvas").gameObject;
        coolDownGuage.transform.SetParent(canvasObj.transform);
        coolDownGuage.transform.localPosition = Vector3.zero;
        coolDownGuage.transform.localScale = Vector3.one;

        _coolDownGuage = coolDownGuage;
        _coolDownGuage.value = (_coolDownDuration / _coolDownTime);
    }

    void UpdateCoolDownGuage ()
    {
        _coolDownGuage.value = (_coolDownDuration / _coolDownTime); 
    }

    TileCell _targetTileCell;

    public void SetTargetTileCell(TileCell targetTileCell)
    {
        _targetTileCell = targetTileCell;

		if (_targetTileCell.GetTileX() != this.GetTileX() && _targetTileCell.GetTileY() != this.GetTileY())
			SetPathFindingType(ePathFindingType.ASTAR);
    }

    public TileCell GetTargetTileCell()
    {
        return _targetTileCell;
    }

    Stack<TileCell> _pathFidningStack = new Stack<TileCell>();

    public void PushPathFindingTileCell(TileCell reverseTileCell)
    {
        _pathFidningStack.Push(reverseTileCell);
    }

    public TileCell PopPathFindingTileCell()
    {
        return _pathFidningStack.Pop();
    }

    public void ClearPathFindingTileCell()
    {
        _pathFidningStack.Clear();
    }

    public bool EmptyPathFindingTileCell()
    {
        if (_pathFidningStack.Count == 0)
            return true;

        return false;
    }

    public eMapObjectType GetType()
    {
        return _type;
    }

    //Effect
    public void ShowEffect(string fileName)
    {
        string filePath = "Prefabs/Effects/" + fileName;
        GameObject effectPrefabs = Resources.Load<GameObject>(filePath);
        GameObject effectObj = Instantiate(effectPrefabs, transform.position, Quaternion.identity);
        GameObject.Destroy(effectObj, 1.5f);
    }

    //Level UP

    protected void InitCharacterStatus()
    {
        ExpManager.Instance.SaveCurExp(_currentExp);
        ExpManager.Instance.SaveLevelUPExp(_levelUPExp);
        ExpManager.Instance.SaveLevel(_level);
    }

    void LevelUP()
    {
        ExpManager.Instance.SaveCurExp(_currentExp);

        if (_levelUPExp < _currentExp)
        {
            _level++;
            Debug.Log("Level : " + _level);
            _currentExp = _currentExp - _levelUPExp;
            _levelUPExp = _levelUPExp * 2;
            _attackPoint = _attackPoint + 10;
            _hp = 100;
            
            ExpManager.Instance.SaveLevelUPExp(_levelUPExp);
            ExpManager.Instance.SaveLevel(_level);
        }
    }

    // Item

    protected int _itemIndex;

    public int GetItemIndex()
    {
        return _itemIndex;
    }

	//PathfindingType

	protected ePathFindingType _pathfindingType;

	void SetPathFindingType(ePathFindingType type)
	{
		_pathfindingType = type;
	}

	public ePathFindingType GetPathFindingType()
	{
		return _pathfindingType;
	}
}
