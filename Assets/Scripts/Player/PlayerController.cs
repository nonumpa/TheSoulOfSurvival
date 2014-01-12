// Converted from UnityScript to C# at http://www.M2H.nl/files/js_to_c.php - by Mike Hergaarden
// Do test the code! You usually need to change a few small bits.

using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour 
{

	// Require a character controller to be attached to the same game object
	//@script RequireComponent(CharacterController)
	
	public AnimationClip idleAnimation;
	public AnimationClip dieAnimation;
	public AnimationClip runAnimation;
	public AnimationClip jumpPoseAnimation;
	
	public float walkMaxAnimationSpeed = 0.75f;
	public float trotMaxAnimationSpeed = 1.0f;
	public float runMaxAnimationSpeed = 1.0f;
	public float jumpAnimationSpeed = 1.15f;
	public float landAnimationSpeed = 1.0f;
	
	private Animation _animation;
	
	enum CharacterState {
		Idle = 0,
		Dying = 1,
		Trotting = 2,
		Running = 3,
		Jumping = 4,
	}
	
	private CharacterState _characterState;
	
	// The speed when walking
	public float walkSpeed= 2.0f;
	// after trotAfterSeconds of walking we trot with trotSpeed
	public float trotSpeed= 4.0f;
	// when pressing "Fire3" button (cmd) we start running
	public float runSpeed= 6.0f;
	
	public float inAirControlAcceleration= 3.0f;
	
	// How high do we jump when pressing jump and letting go immediately
	public float jumpHeight= 0.5f;
	
	// The gravity for the character
	public float gravity= 20.0f;
	// The gravity in controlled descent mode
	public float speedSmoothing= 10.0f;
	public float rotateSpeed= 500.0f;
	public float trotAfterSeconds= 3.0f;
	
	public bool canJump= true;
	
	private float jumpRepeatTime= 0.05f;
	private float jumpTimeout= 0.15f;
	private float groundedTimeout= 0.25f;
	
	// The camera doesnt start following the target immediately but waits for a split second to avoid too much waving around.
	private float lockCameraTimer= 0.0f;
	
	// The current move direction in x-z
	private Vector3 moveDirection= Vector3.zero;
	// The current vertical speed
	private float verticalSpeed= 0.0f;
	// The current x-z move speed
	private float moveSpeed= 0.0f;
	
	// The last collision flags returned from controller.Move
	private CollisionFlags collisionFlags; 
	
	// Are we jumping? (Initiated with jump button and not grounded yet)
	private bool jumping= false;
	private bool jumpingReachedApex= false;
	
	// Are we moving backwards (This locks the camera to not do a 180 degree spin)
	private bool movingBack= false;
	// Is the user pressing any keys?
	private bool isMoving= false;
	// When did the user start walking (Used for going into trot after a while)
	private float walkTimeStart= 0.0f;
	// Last time the jump button was clicked down
	private float lastJumpButtonTime= -10.0f;
	// Last time we performed a jump
	private float lastJumpTime= -1.0f;
	
	
	// the height we jumped from (Used to determine for how long to apply extra jump power after jumping.)
	private float lastJumpStartHeight= 0.0f;
	
	
	private Vector3 inAirVelocity= Vector3.zero;
	
	private float lastGroundedTime= 0.0f;
	
	private bool isControllable= true;
	//------my define-------
	MainScript ScriptCtrl;

	public bool canTurn = false;
	public float speed = 7f;
	public Transform [] transPos = new Transform[4]; //front, back, right ,left
	public GameObject loseBtn,minimap,score;
	public Texture PlayerDieTexture;
	public GameObject Skill;

	float startTime;
	bool  couldBeSwipe;
	bool  skillFlicker = false;
	float comfortZone;
	float minSwipeDist;
	float getKeyTime;
	float nextFlicker;



	ControllerColliderHit collide;
	ControllerColliderHit hitTemp;
	
	bool grounded = true;
	bool FlipFront = false;
	bool FlipRight = false;
	bool FlipLeft = false;
	
	//bool killable = false;
	Vector2 startPos;
	
	enum TurnKey {
		up = 0,
		down = 1,
		right = 2,
		left = 3,
		Idle = -1,
	}
	private TurnKey _turnkey = TurnKey.Idle;
	
	void  Awake ()
	{
		ScriptCtrl = GameObject.Find("ALLScriptCtrl").GetComponent<MainScript>();
		moveDirection = transform.TransformDirection(Vector3.forward);
		
		_animation = GetComponent<Animation>();
		if(!_animation)
			Debug.Log("The character you would like to control doesn't have animations. Moving her might look weird.");
		
		/*
	public AnimationClip idleAnimation;
	public AnimationClip dieAnimation;
	public AnimationClip runAnimation;
	public AnimationClip jumpPoseAnimation;	
		*/
		if(!idleAnimation) {
			_animation = null;
			Debug.Log("No idle animation found. Turning off animations.");
		}
		if(!dieAnimation) {
			_animation = null;
			Debug.Log("No walk animation found. Turning off animations.");
		}
		if(!runAnimation) {
			_animation = null;
			Debug.Log("No run animation found. Turning off animations.");
		}
		if(!jumpPoseAnimation && canJump) {
			_animation = null;
			Debug.Log("No jump animation found and the character has canJump enabled. Turning off animations.");
		}
				
	}

	void doFlipWall(){
		CharacterController controller = GetComponent<CharacterController>();
		RaycastHit hit;
		float oriY = transform.position.y;
		//transform.Translate(0,5f,0);
		//transform.Translate(0,0,3f);
		verticalSpeed = CalculateJumpVerticalSpeed (jumpHeight*1.2f);
		if(collide.collider.tag == "Wall"){
			hitTemp = collide;
			controller.detectCollisions = false;
			//hitTemp.collider.enabled = false;
		}
		flipAnimation();
		//Physics.IgnoreCollision(this.transform.collider,collide);
		if(Physics.Raycast(transform.position, (-transform.up+new Vector3(-1f,0,0))*100, out hit)){
			Debug.DrawRay(transform.position,(-transform.up+new Vector3(-1f,0,0))*100,Color.blue);
			if(hit.transform.tag == "ground"){
				//transform.Translate(0,-2f,0);
				gravity = 1000f;
				_turnkey = TurnKey.Idle;
				speed = 0;
				//hitTemp.collider.enabled = true;
				controller.detectCollisions = true;
				//transform.position.y = oriY;
			}else{
				//transform.Translate(0,0,0.001f);
			}
		}
		//hitTemp.collider.enabled = true;
	}

	void flipAnimation(){
		for(int i=0;i<360;i++){
			Quaternion rot = new Quaternion();
			rot.eulerAngles = new Vector3(0,(float)i,0);
			this.transform.rotation = rot;
			//moveDirection=transform.TransformDirection(Vector3.forward);
			//Vector3 movement= moveDirection * speed;
			//movement *= Time.deltaTime;
			// Move the controller
			//CharacterController controller = GetComponent<CharacterController>();
			//collisionFlags = controller.Move(movement);
		}
	}
	
	void  UpdateSmoothedMovementDirection ()
	{
		//Transform cameraTransform= Camera.main.transform;
		bool grounded= IsGrounded();
		
		bool wasMoving= isMoving;
		
		// Grounded controls
		if (grounded)
		{
			// Lock camera for short period when transitioning moving & standing still
			lockCameraTimer += Time.deltaTime;
			if (isMoving != wasMoving)
				lockCameraTimer = 0.0f;
		}
		// In air controls
		else
		{
			if (jumping){
				if(_turnkey == TurnKey.down){//kill monster
					gravity=500;
					_turnkey = TurnKey.Idle;
				}
				else if((_turnkey == TurnKey.up)  && FlipFront ){
					//doFlipWall();
				}
				else if((_turnkey == TurnKey.right)  && FlipRight){
					//turnRL();
					//doFlipWall();
				}
				else if((_turnkey == TurnKey.left)  && FlipLeft){
					//turnRL();
					//doFlipWall();
				}
			}//end jumping
			else {
				gravity=20;
			}
			if (isMoving)
				inAirVelocity += moveDirection * Time.deltaTime * inAirControlAcceleration;
		}
		
	
			
	}
	
	void  ApplyJumping (){
		// Prevent jumping too fast after each other
		if (lastJumpTime + jumpRepeatTime > Time.time)
			return;
	
		if (IsGrounded()) {
			// Jump
			// - Only when pressing the button down
			// - With a timeout so you can press the button slightly before landing		
			if (canJump && Time.time < lastJumpButtonTime + jumpTimeout) {
				verticalSpeed = CalculateJumpVerticalSpeed (jumpHeight);
				SendMessage("DidJump", SendMessageOptions.DontRequireReceiver);
			}
		}
	}
	
	
	void  ApplyGravity ()
	{
		if (isControllable)	// don't move player at all if not controllable.
		{
			// Apply gravity
			bool jumpButton= Input.GetButton("Jump");
			
			
			// When we reach the apex of the jump we send out a message
			if (jumping && !jumpingReachedApex && verticalSpeed <= 0.0f)
			{
				jumpingReachedApex = true;
				SendMessage("DidJumpReachApex", SendMessageOptions.DontRequireReceiver);
			}
		
			if (IsGrounded ())
				verticalSpeed = 0.0f;
			else
				verticalSpeed -= gravity*0.0045f;
		}
	}
	
	public float CalculateJumpVerticalSpeed ( float targetJumpHeight  )
	{
		// From the jump height and gravity we deduce the upwards speed 
		// for the character to reach at the apex.
		return Mathf.Sqrt(2 * targetJumpHeight * gravity);
	}
	
	public void DidJump ()
	{
		jumping = true;
		jumpingReachedApex = false;
		lastJumpTime = Time.time;
		lastJumpStartHeight = transform.position.y;
		lastJumpButtonTime = -10;
		_characterState = CharacterState.Jumping;
	}
	
	public void turnUpOrDown(){
		switch(_turnkey){
			case TurnKey.up:
				lastJumpButtonTime = Time.time;    //apply jump
				grounded = false;
				_turnkey = TurnKey.Idle;
				break;
			case TurnKey.down:
				transform.Rotate(new Vector3(0f,1f,0f),180f);
				moveDirection=transform.TransformDirection(Vector3.forward);
				_turnkey = TurnKey.Idle;
				break;
		}
	}
	public void turnRL()
	{
		switch(_turnkey){
			case TurnKey.right:
				transform.Rotate(new Vector3(0f,1f,0f),90f);
				moveDirection=transform.TransformDirection(Vector3.forward);
				_turnkey = TurnKey.Idle;
				break;
			case TurnKey.left:
				transform.Rotate(new Vector3(0f,1f,0f),-90f);
				moveDirection=transform.TransformDirection(Vector3.forward);
				_turnkey = TurnKey.Idle;
				break;
		}
	}

	void Update ()
	{	

		if(ScriptCtrl.useSkill){
			if(Time.time > ScriptCtrl.SkillFinishTime){
				ScriptCtrl.useSkill = false;
				//FlickerTimes=0;
				print("end");
			}

			if(ScriptCtrl.useSkill && (Time.time > ScriptCtrl.SkillFinishTime-4f) && Time.time > nextFlicker){
				nextFlicker = Time.time+0.2f;
				//FlickerTimes++;
				skillFlicker = !skillFlicker;
				NGUITools.SetActive(Skill,skillFlicker);
				print ("flicker");
			}
			else if(ScriptCtrl.useSkill && (Time.time < ScriptCtrl.SkillFinishTime-3f)){
				NGUITools.SetActive(Skill,true);
				speed = 14f;
				print("start");
			}
			else if(!ScriptCtrl.useSkill){
				NGUITools.SetActive(Skill,false);
				speed = 7f;
				print("dieable");
			}
			
		}
		if (!isControllable)
		{
			// kill all inputs if not controllable.
			Input.ResetInputAxes();
		}
		if(speed>0 && !jumping)
		{
			_characterState = CharacterState.Running;
		}
		RaycastHit hit ;
		//detect wall is fornt or left or right then enable flipWall
		
		if(Physics.Raycast(transform.position, this.transform.forward, out hit, 3.0f)){//front
			if(hit.transform.gameObject.tag == "Wall")  {FlipFront=true;}
			else 										{FlipFront=false;}
			Debug.DrawLine(transform.position, hit.transform.position , Color.green);
		}
		
		if(Physics.Raycast(transform.position, this.transform.right, out hit, 3.0f)){//right
			if(hit.transform.gameObject.tag == "Wall")  {FlipRight=true;}
			else 										{FlipFront=false;}
			Debug.DrawLine(transform.position, hit.transform.position , Color.green);
		}
		
		if(Physics.Raycast(transform.position, -this.transform.right, out hit, 3.0f)){//left
			if(hit.transform.gameObject.tag == "Wall")  {FlipLeft=true;}
			else										{FlipFront=false;}
			Debug.DrawLine(transform.position, hit.transform.position , Color.green);
		}
		
		if(getKeyTime <Time.time-1.3){//release turn gesture
			_turnkey = TurnKey.Idle;
		}
		
		_turnkey = getTurnKey();
		/*
		//----------------------------------move-------------------------------------
		if(Input.GetKeyDown(KeyCode.W)){
			_turnkey = TurnKey.up;
			//getKeyTime= Time.time;
		}
		if(Input.GetKeyDown(KeyCode.S) && !jumping){
			_turnkey = TurnKey.down;
			//getKeyTime= Time.time;
		}
		if(Input.GetKeyDown(KeyCode.D)){
			_turnkey = TurnKey.right;
			getKeyTime= Time.time;//enable turn right and left in the duration
		}
		if(Input.GetKeyDown(KeyCode.A)){
			_turnkey = TurnKey.left;
			getKeyTime= Time.time;
		}
		if(Input.touchCount>0){
			Touch touch =Input.touches[0];switch(touch.phase){
				case TouchPhase.Began:
					startPos = touch.position;
					break;
				case TouchPhase.Moved:
					if(Mathf.Abs(touch.position.y - startPos.y) > 100f)
					{
						float swipValue = Mathf.Sign(touch.position.y - startPos.y);
	                    if(swipValue > 0){
							_turnkey = TurnKey.up;
							getKeyTime= Time.time;
							break;
						}
						else if(swipValue < 0  &&  !jumping){
							_turnkey = TurnKey.down;
							getKeyTime= Time.time;
							break;
						}
					}
					
					else if(Mathf.Abs(touch.position.x - startPos.x) > 100f)
					{
						float swipValue = Mathf.Sign(touch.position.x - startPos.x);
						if(swipValue > 0){
							_turnkey = TurnKey.right;
							getKeyTime= Time.time;
							break;
						}
						else if(swipValue < 0){
							_turnkey = TurnKey.left;
							getKeyTime= Time.time;
							break;
						}
					}
				break;
			}
		
		}//---------------------------------move end----------------------------------------
		else{
			turnUpOrDown();
		}*/
		
		
		if (Input.GetButtonDown ("Jump"))
		{
			lastJumpButtonTime = Time.time;
		}
	
		UpdateSmoothedMovementDirection();
		
		// Apply gravity
		// - extra power jump modifies gravity
		// - controlledDescent mode modifies gravity
		ApplyGravity ();
	
		// Apply jumping logic
		ApplyJumping ();
		
		// Calculate actual motion
		Vector3 movement= moveDirection * speed + new Vector3 (0, verticalSpeed, 0) + inAirVelocity;
		movement *= Time.deltaTime;
		// Move the controller
		CharacterController controller = GetComponent<CharacterController>();
		collisionFlags = controller.Move(movement);

		// ANIMATION sector
		if(_animation) {
			if(_characterState == CharacterState.Jumping) 
			{
				if(!jumpingReachedApex) {
					_animation[jumpPoseAnimation.name].speed = jumpAnimationSpeed;
					_animation[jumpPoseAnimation.name].wrapMode = WrapMode.ClampForever;
					_animation.CrossFade(jumpPoseAnimation.name);
				} else {
					_animation[jumpPoseAnimation.name].speed = -landAnimationSpeed;
					_animation[jumpPoseAnimation.name].wrapMode = WrapMode.ClampForever;
					_animation.CrossFade(jumpPoseAnimation.name);				
				}
			} 
			else 
			{
				if(_characterState == CharacterState.Dying) {
					_animation.CrossFade(dieAnimation.name);
				}
				else if(controller.velocity.sqrMagnitude < 0.1f) {
					_animation.CrossFade(idleAnimation.name);
				}
				else 
				{
					if(_characterState == CharacterState.Running) {
						_animation[runAnimation.name].speed = Mathf.Clamp(controller.velocity.magnitude, 0.0f, runMaxAnimationSpeed);
						_animation.CrossFade(runAnimation.name);	
					}
					else if(_characterState == CharacterState.Dying) {
						_animation[dieAnimation.name].speed = Mathf.Clamp(controller.velocity.magnitude, 0.0f, walkMaxAnimationSpeed);
						_animation.CrossFade(dieAnimation.name);	
					}
					
				}
			}
		}
		// ANIMATION sector

		
		// We are in jump mode but just became grounded
		if (IsGrounded())
		{
			lastGroundedTime = Time.time;
			inAirVelocity = Vector3.zero;
			if (jumping)
			{
				jumping = false;
				SendMessage("DidLand", SendMessageOptions.DontRequireReceiver);
			}
		}
		//speed=0 can turn
		if(controller.velocity.sqrMagnitude < 0.1f){
			turnRL();	
		}
	}
	
	TurnKey getTurnKey(){
		if(Input.GetKeyDown(KeyCode.W)){
			getKeyTime= Time.time;
			return TurnKey.up;
		}
		if(Input.GetKeyDown(KeyCode.S)){
			getKeyTime= Time.time;
			return TurnKey.down;
		}
		if(Input.GetKeyDown(KeyCode.D)){
			getKeyTime= Time.time;//enable turn right and left in the duration
			return TurnKey.right;
		}
		if(Input.GetKeyDown(KeyCode.A)){
			getKeyTime= Time.time;
			return TurnKey.left;
		}
		if(Input.touchCount>0){
			Touch touch =Input.touches[0];switch(touch.phase){
				case TouchPhase.Began:
					startPos = touch.position;
					break;
				case TouchPhase.Moved:
					if(Mathf.Abs(touch.position.y - startPos.y) > 100f)
					{
						float swipValue = Mathf.Sign(touch.position.y - startPos.y);
	                    if(swipValue > 0){
							getKeyTime= Time.time;
							return TurnKey.up;
						}
						else if(swipValue < 0){
							getKeyTime= Time.time;
							return TurnKey.down;
						}
					}
					
					else if(Mathf.Abs(touch.position.x - startPos.x) > 100f)
					{
						float swipValue = Mathf.Sign(touch.position.x - startPos.x);
						if(swipValue > 0){
							getKeyTime= Time.time;
							return TurnKey.right;
						}
						else if(swipValue < 0){
							getKeyTime= Time.time;
							return TurnKey.left;
						}
					}
				break;
			}
		}
		else if(!jumping){
			turnUpOrDown();//turn when end touch
		}
		return _turnkey;
	}//get turn key
	
	void OnTriggerEnter(Collider other) {

		if(other.collider.tag == "canturn")
       		turnRL();
		if(!ScriptCtrl.useSkill && other.collider.tag == "Monster"){
			if(jumping){
				//move game object to some where
				Destroy(other.gameObject);
				ScriptCtrl.NumOfMonsterKill++;
				//numMonsterKill++;
			}else{
				NGUITools.SetActive(loseBtn,true);
				//-------pause
				speed = 0f;
				other.GetComponent<NavMeshAgent>().speed = 0.0f;
				//------set state to dying
				_characterState = CharacterState.Dying;
				transform.FindChild("UltraLowPoly").renderer.material.mainTexture = PlayerDieTexture;

				Invoke("showResult",2.5f);
			}
		}
		
		if(other.collider.tag == "Transform-Front"){
			transform.position = new Vector3(transPos[1].position.x,transPos[1].position.y+1f,transPos[1].position.z); //back
			transform.Translate(0,0,2.4f);
		}
		if(other.collider.tag == "Transform-Back"){
			transform.position = new Vector3(transPos[0].position.x,transPos[0].position.y+1f,transPos[0].position.z); //front
			transform.Translate(0,0,2.4f);
		}
		if(other.collider.tag == "Transform-Right"){
			transform.position = new Vector3(transPos[3].position.x,transPos[3].position.y+1f,transPos[3].position.z); //left
			transform.Translate(0,0,2.4f);
		}
		if(other.collider.tag == "Transform-Left"){
			transform.position = new Vector3(transPos[2].position.x,transPos[2].position.y+1f,transPos[2].position.z); //right
			transform.Translate(0,0,2.4f);
		}
    }

	void showResult(){
		NGUITools.SetActive(minimap,false);
		NGUITools.SetActive(score,true);
	}
	
	void OnControllerColliderHit ( ControllerColliderHit hit   )
	{
		Debug.DrawRay(hit.point, hit.normal);
		collide=hit;
		if (hit.moveDirection.y > 0.01f) 
			return;
	}
	
	
	
	public float GetSpeed ()
	{
		return moveSpeed;
	}
	
	public bool IsJumping ()
	{
		return jumping;
	}
	
	public bool IsGrounded ()
	{
		return (collisionFlags & CollisionFlags.CollidedBelow) != 0;
	}
	
	public Vector3 GetDirection ()
	{
		return moveDirection;
	}
	
	public bool IsMovingBackwards ()
	{
		return movingBack;
	}
	
	public float GetLockCameraTimer ()
	{
		return lockCameraTimer;
	}
	
	public bool IsMoving ()
	{
		 return Mathf.Abs(Input.GetAxisRaw("Vertical")) + Mathf.Abs(Input.GetAxisRaw("Horizontal")) > 0.5f;
	}
	
	public bool HasJumpReachedApex ()
	{
		return jumpingReachedApex;
	}
	
	public bool IsGroundedWithTimeout ()
	{
		return lastGroundedTime + groundedTimeout > Time.time;
	}
	
	public void Reset ()
	{
		gameObject.tag = "Player";
	}

}