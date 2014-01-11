using UnityEngine;
using System.Collections;

public class TimeAndStart : MonoBehaviour {
	public GameObject ready;
	UILabel txtTime;
	MainScript ScriptCtrl;
	internal string minsecS,secS,minS;
	int minsecI,secI,minI;
	bool waiting = true;

	void Awake(){
		ScriptCtrl = GameObject.Find("ALLScriptCtrl").GetComponent<MainScript>();
		txtTime = (UILabel)GetComponentInChildren<UILabel>();
		init ();
	}

	// Use this for initialization
	void Start () {
		Invoke("startGame",2.5f);
	}
	
	// Update is called once per frame
	void Update () {
		if(!waiting){
			TimeScore();
		}
	}

	void TimeScore(){
		ScriptCtrl.passTime += Time.deltaTime;

		if(Mathf.FloorToInt((100.0f*(ScriptCtrl.passTime-Mathf.Floor(ScriptCtrl.passTime))))<10){
			minsecS = "0"+Mathf.FloorToInt((100.0f*(ScriptCtrl.passTime-Mathf.Floor(ScriptCtrl.passTime)))).ToString();
		}else{
			minsecS = Mathf.FloorToInt((100.0f*(ScriptCtrl.passTime-Mathf.Floor(ScriptCtrl.passTime)))).ToString();
		}

		secI = (Mathf.FloorToInt(ScriptCtrl.passTime)%60);
		if(secI<10){
			secS = "0"+secI.ToString();
		}
		else{
			secS = secI.ToString();
		}

		minI = Mathf.FloorToInt(ScriptCtrl.passTime)/60;
		if((Mathf.FloorToInt(ScriptCtrl.passTime)/60)<10){
			minS = "0"+minI.ToString();
		}
		else{
			minS = minI.ToString();
		}

		txtTime.text =minS+":"+secS+":"+minsecS;
	}

	void init(){
		ScriptCtrl.passTime = 0;
		ScriptCtrl.player.speed = 0;
		ScriptCtrl.monster1.GetComponent<NavMeshAgent>().speed = 0;
		ScriptCtrl.monster2.GetComponent<NavMeshAgent>().speed = 0;
		waiting = true;
	}

	void startGame(){
		ScriptCtrl.player.speed = 7f;
		ScriptCtrl.monster1.GetComponent<NavMeshAgent>().speed = 5f;
		ScriptCtrl.monster2.GetComponent<NavMeshAgent>().speed = 5f;
		NGUITools.SetActive(ready,false);
		waiting = false;
	}
	void OnClick(){

		//Application.LoadLevel(2);
	}
}
