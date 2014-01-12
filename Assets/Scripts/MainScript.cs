using UnityEngine;
using System.Collections;

public class MainScript : MonoBehaviour {
	public PlayerController player;
	public Monster monster1;
	public Monster monster2;
	public Monster monster3;
	public Monster monster4;
	internal int NumOfMonsterKill;
	internal int NumOfSoulGet;
	internal float passTime;      //total time counter

	//skill variable
	internal bool skillEnable = false;
	internal bool useSkill = false;
	internal float SkillFinishTime;
	public float skillDuration =15f;//15sec
	public int WhenToUseSkill =15;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
