using UnityEngine;
using System.Collections;

public class soul_fly : MonoBehaviour {
	Transform[] path;
	GameObject SoulGroup;
	public GameObject soul;
	// Use this for initialization
	
	void Start () {
		SoulGroup=GameObject.Find("SoulGroup");
		path = SoulGroup.transform.GetComponentsInChildren<Transform>();
		Transform ran=path[(int)(Random.value*path.Length)];
		//print ((int)(Random.value*path.Length));
		rigidbody.AddForce((ran.position-transform.position)*500);
		Destroy(this.gameObject,1);
		Instantiate(soul,ran.position,Quaternion.identity);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
