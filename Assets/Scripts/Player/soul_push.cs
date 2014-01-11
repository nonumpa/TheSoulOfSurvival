using UnityEngine;
using System.Collections;

public class soul_push : MonoBehaviour {
	public GameObject soul_fly;
	public int numberOfSoulTofly;

	// Use this for initialization
	void Start () {
		
		
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 position1 =new Vector3(transform.position.x,transform.position.y+5,transform.position.z);
		if(Input.GetKeyDown("l"))
		{
			for(int i=0;i<numberOfSoulTofly; i++)
			{
				Instantiate(soul_fly,position1, Quaternion.identity);
			}
		}
		
	}
}
