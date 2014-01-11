using UnityEngine;
using System.Collections;

public class SoulBar : MonoBehaviour {
	public UISprite prog;
	public soul_get sg;
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		prog.fillAmount=sg.percentageOfSoul;
	}
}
