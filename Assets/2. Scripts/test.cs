using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Debug.Log("Cur Exp : " +ExpManager.Instance.LoadCurExp());
        Debug.Log("Next Exp : " + ExpManager.Instance.LoadNextExp());
        Debug.Log("Level : " + ExpManager.Instance.LoadLevel());
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
