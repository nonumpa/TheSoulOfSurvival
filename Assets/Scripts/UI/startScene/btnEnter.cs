using UnityEngine;
using System.Collections;

public class btnEnter : MonoBehaviour {
	public GameObject Loading;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	void OnClick(){
		GameObject sel=GameObject.FindWithTag("Enter").gameObject;
		if(sel.name == "Btn-Start"){
			NGUITools.SetActive(Loading,true);
			Application.LoadLevel(1);
		}
		else if(sel.name == "Btn-Exit")
			Application.Quit();
	}
}
