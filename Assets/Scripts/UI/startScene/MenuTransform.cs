using UnityEngine;
using System.Collections;

public class MenuTransform : MonoBehaviour {
	public int pos;
	public int maxpos=3;
	TweenTransform menuLabel;
	string[] offset = {"Offset-top","Offset-1","Offset-2","Offset-3","Offset-buttom"};
	// Use this for initialization
	void Start () {
		menuLabel = (TweenTransform) (GetComponent("TweenTransform"));
	}
	
	// Update is called once per frame
	void Update () {
		if(pos == 2){
			this.tag = "Enter";
		}
		
		if (!menuLabel.enabled && Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved) {
			if( Input.GetTouch(0).deltaPosition.y >0){
				pos--;
				if(pos<1){	
					this.tag = "offset-top";
					menuLabel.duration = 1.0f/2;
					move (pos+1,pos);
					pos = maxpos;				
				}
				else{
					menuLabel.duration = 1.0f;
					move (pos+1,pos);
				}
			}
			if( Input.GetTouch(0).deltaPosition.y <0){
				pos++;
				if(pos>maxpos){
					this.tag = "offset-buttom";
					menuLabel.duration = 1.0f/2;
					move (pos-1,pos);
					pos = 1;
				}
				else{
					menuLabel.duration = 1.0f;
					move (pos-1,pos);
				}
			}
		}
		
		if( this.tag == "offset-top" && !menuLabel.enabled ){
			move (4,pos);
			this.tag="offset-default";
		}
		if(this.tag == "offset-buttom" && !menuLabel.enabled){
			move (0,pos);
			this.tag="offset-default";
		}
		
	}
	void move(int fr ,int to){
		menuLabel.from = GameObject.Find(offset[fr]).transform ;
		menuLabel.to = GameObject.Find(offset[to]).transform ;
		menuLabel.Reset();
		menuLabel.enabled = true;
	}
}
