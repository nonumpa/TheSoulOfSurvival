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
		if(ScriptCtrl.NumOfSoulGet < ScriptCtrl.WhenToUseSkill){
			ScriptCtrl.skillEnable = false;
		}
		else{
			ScriptCtrl.skillEnable = true;
		}
	}

	void OnClick(){
		if(ScriptCtrl.skillEnable){
			ScriptCtrl.useSkill = true;
			ScriptCtrl.SkillFinishTime = Time.time + ScriptCtrl.skillDuration;
		}
	}
}
