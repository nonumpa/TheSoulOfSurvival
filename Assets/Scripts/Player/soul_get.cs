using UnityEngine;
using System.Collections;

public class soul_get : MonoBehaviour {
	MainScript ScriptCtrl;
	int totalSoul=100;
	int SoulNum=0;
	public float percentageOfSoul=0.0f;
	public GameObject winTitle,minimap,score;
	// Use this for initialization
	void Start () {
		ScriptCtrl = GameObject.Find("ALLScriptCtrl").GetComponent<MainScript>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	 void OnTriggerEnter(Collider other) {
        if(other.tag=="soul")
		{
			SoulNum++;
			if(SoulNum>0)percentageOfSoul= (float)SoulNum/(float)totalSoul;
			Destroy(other.gameObject);
			if(SoulNum == totalSoul){
				YouWin();
			}
		}
    }
	
	void YouWin(){
		NGUITools.SetActive(winTitle,true);
		Invoke("showResult",2.5f);
	}
	void showResult(){
		NGUITools.SetActive(minimap,false);
		NGUITools.SetActive(score,true);
	}
}
