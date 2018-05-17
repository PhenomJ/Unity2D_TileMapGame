using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpManager : MonoBehaviour {
    //singleton

    static ExpManager _instance;
    public static ExpManager Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject expManager = new GameObject("ExpManager");
                _instance = expManager.AddComponent<ExpManager>();
                _instance.Init();
            }
            return _instance;
        }
    }

    void Init()
    {

    }

    static int _currentExp;
    static int _levelUPExp;
    static int _level;

    private void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
    }

    public void SaveCurExp(int exp)
    {
        _currentExp = exp;
    }

    public int LoadCurExp()
    {
        return _currentExp;
    }

    public void SaveLevelUPExp(int levelUPExp)
    {
        _levelUPExp = levelUPExp;
    }

    public int LoadNextExp()
    {
        return _levelUPExp;
    }

    public void SaveLevel(int level)
    {
        _level = level;
    }

    public int LoadLevel()
    {
        return _level;
    }
}
