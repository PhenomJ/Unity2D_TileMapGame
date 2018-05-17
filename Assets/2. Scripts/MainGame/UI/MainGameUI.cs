using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MainGameUI : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public GameObject HPGaugePrefabs;

    public Slider CreateHPSlider()
    {
        GameObject HPObj = Instantiate(HPGaugePrefabs);
        Slider slider = HPObj.GetComponent<Slider>();
        return slider;
    }

    public GameObject CoolDownGaugePrefabs;

    public Slider CreateCoolDownSlider()
    {
        GameObject CoolDownObj = Instantiate(CoolDownGaugePrefabs);
        Slider slider = CoolDownObj.GetComponent<Slider>();
        return slider;
    }

    public void OnClick()
    {
        //Character enemy = GameManager.Instance.targetCharacter;

        //MessageParam msgParam = new MessageParam();
        //msgParam.sender = null;
        //msgParam.receiver = enemy;
        //msgParam.message = "Attack";
        //msgParam.attackPoint = 1000;

        //MessageSystem.Instance.Send(msgParam);

        SceneManager.LoadScene("TestScene");
    }
}
