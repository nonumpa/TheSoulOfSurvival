using UnityEngine;
using System.Collections;

public class Path : MonoBehaviour {
	Transform[] path;
	Color raycolor=Color.white;
	public float soulDistance = 5f;
	public GameObject soul;
	float journeyLength;
	int num;
	Vector3 SoulPos;
	
	// Use this for initialization
	void Start () {
		path = transform.GetComponentsInChildren<Transform>();
		Transform[] path_objs = transform.GetComponentsInChildren<Transform>();
		
		foreach (Transform path_obj in path_objs){
			int i=0;
			if(path_obj != transform)
			{path [i] = path_obj;i++;}
		}
		
		for (int i=0; i < path.Length;i++){
			Vector3 pos =path[i].position;
			if(i>1){
				Vector3 prev = path[i-1].position;				
				journeyLength = Vector3.Distance(prev, pos);
				num = (int)(journeyLength/soulDistance);
				for(int j=0; j<num ;j++){
					SoulPos = Vector3.Lerp(prev, pos, j*soulDistance/journeyLength);
					Instantiate(soul,SoulPos,Quaternion.identity);
				}
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	void OnDrawGizmos(){
		path = transform.GetComponentsInChildren<Transform>();
		Gizmos.color = raycolor;
		Transform[] path_objs = transform.GetComponentsInChildren<Transform>();
		
		foreach (Transform path_obj in path_objs){
			int i=0;
			if(path_obj != transform)
			{path [i] = path_obj;i++;}
		}
		
		for (int i=0; i < path.Length;i++){
			Vector3 pos =path[i].position;
			if(i>1){
				Vector3 prev = path[i-1].position;
				Gizmos.DrawLine(prev,pos);
				Gizmos.DrawWireSphere(pos,0.3f);
				
				journeyLength = Vector3.Distance(prev, pos);
				num = (int)(journeyLength/soulDistance);
				//var distCovered = (Time.time - startTime) * speed;
				//var fracJourney = distCovered / journeyLength;
				for(int j=0; j<num ;j++){
					SoulPos = Vector3.Lerp(prev, pos, j*soulDistance/journeyLength);
//					Instantiate(soul,SoulPos,Quaternion.identity);
				}
			}
		}
	}
}
