using UnityEngine;
using System.Collections;

public class Skill : MonoBehaviour {
	MainScript ScriptCtrl;
	// Use this for initialization
	void Start () {
		ScriptCtrl = GameObject.Find("ALLScriptCtrl").GetComponent<MainScript>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnClick(){
		ScriptCtrl.useSkill = true;
	}
}
