using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Monster : MonoBehaviour{
	
	public float loseTargetTimeInterval;
	public float distInterval;
	
	public GameObject spwanPointObj;
	public GameObject pathObj;
	Transform[] spwanPoint;
	Transform[] path;

	public Transform target;
	public Transform player;
	
	public NavMeshAgent navmeshagent;
	//internal speed;

	public float time;
	
	private bool isLockPlayer = false;
	//private bool beforeLock = false;
	
	void Start () {
		
		navmeshagent = gameObject.GetComponent<NavMeshAgent>();
		//speed=GetComponent<NavMeshAgent>().speed;
		
		
		initPath();
		
		transform.position = giveTarget(spwanPoint).position;
		target = giveTarget(path);
		navmeshagent.SetDestination(target.position);
		
	}
	
	void Update () {
		//print((transform.position - player.transform.position).sqrMagnitude);
		if(seeThePlayer())
		{//------------------------------------------------------------
			time = loseTargetTimeInterval;
			target = player;
			isLockPlayer = true;
			navmeshagent.SetDestination (target.position);
		}
		else if(time > 0f)
		{
			time -= Time.deltaTime;
			navmeshagent.SetDestination (target.position);
		}
		else if (time <= 0f && isLockPlayer)
		{
			isLockPlayer = false;
		}//------------------------------------------------------------
		
		if(!isLockPlayer && target == player)
		{
			target = giveTarget(path);
			navmeshagent.SetDestination(target.position);
		}
			
		if (time <= 0f && ((transform.position - target.position).sqrMagnitude < 1f))
		{
			target = giveTarget(path);
			navmeshagent.SetDestination (target.position);
		}
		
		//Vector3 fwd = transform.TransformDirection(Vector3.forward);
		//Debug.DrawLine(transform.position, player.transform.position, Color.yellow);
		
	}
	
	void initPath() {
		int num = spwanPointObj.transform.childCount;
		
		spwanPoint = new Transform[num];
		path = new Transform[num];
		
		for(int i = 0; i < num; i++)
		{
			spwanPoint[i] = spwanPointObj.transform.GetChild(i);
			path[i] = pathObj.transform.GetChild (i);
		}
	}
	
	Transform giveTarget(Transform[] trans) {
		
		int index = Random.Range (1, trans.Length);
		return trans[index]; 
	}
	
	bool isFrontOfYou() {
		Vector3 dirToPlayer = transform.position - player.transform.position;
		float angle = Vector3.Angle(transform.forward, dirToPlayer);
		return (Mathf.Abs(angle) > 90) ? true : false;
	}
	
	bool isCloseToYou() {
		float distToPlayer = Vector3.Distance(transform.position, player.transform.position);
		return (distToPlayer < distInterval) ? true : false;
	}
	
	bool seeThePlayer() {
		//Vector3 fwd = transform.TransformDirection(transform.forward);
		
		Debug.DrawRay(new Vector3(0f,2f,0f)+transform.position, transform.forward*15f , Color.yellow);
		if( isFrontOfYou() && isCloseToYou() )
		{
			
			RaycastHit hit;
			Vector3 range = new Vector3(0f, 2f, 0f);
			//Debug.DrawRay(transform.position, fwd, Color.yellow);
			
			if( Physics.Raycast(transform.position + range, transform.forward*15f, out hit) )
			{
				if( hit.transform.tag == "Player" )
					return true;
				
				return false;
			}
			return false;	
		}
		return false;

	}
	
}
