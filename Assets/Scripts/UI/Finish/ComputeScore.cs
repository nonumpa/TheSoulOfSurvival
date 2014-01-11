using UnityEngine;
using System.Collections;

public class ComputeScore : MonoBehaviour {
	MainScript ScriptCtrl;
	int Total_score,Soul_score,Kill_score,Time_score;
	public GameObject Time_label,Soul_label,Kill_label,Total_score_label;
	// Use this for initialization
	void Start () {
		Total_score=0;
		Soul_score=0;
		Kill_score=0;
		Time_score=0;
		ScriptCtrl = GameObject.Find("ALLScriptCtrl").GetComponent<MainScript>();
		Soul_score=ScriptCtrl.NumOfSoulGet*10;
		Kill_score=ScriptCtrl.NumOfMonsterKill*100;
		if(ScriptCtrl.passTime>=60000 && ScriptCtrl.passTime <180000)
		{
			Time_score=Mathf.FloorToInt(Mathf.Lerp(5000,3000,ScriptCtrl.passTime/180));
		}
		else if(ScriptCtrl.passTime>=180000 && ScriptCtrl.passTime<300000)
		{
			Time_score=Mathf.FloorToInt(Mathf.Lerp(3000,1000,(ScriptCtrl.passTime-180)/180));
		}
		else
		{
			Time_score=Mathf.FloorToInt(Mathf.Lerp(1000,0,(ScriptCtrl.passTime-360)/360));
		}
		Total_score=Soul_score+Kill_score+Time_score;
		Time_label.GetComponent<UILabel>().text=Time_score.ToString();
		Soul_label.GetComponent<UILabel>().text=Soul_score.ToString();
		Kill_label.GetComponent<UILabel>().text=Kill_score.ToString();
		Total_score_label.GetComponent<UILabel>().text=Total_score.ToString();
	}
	// Update is called once per frame
	void Update () {

	}
}
