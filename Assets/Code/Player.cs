using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour{

	public enum Color{
		Default,
		Red,
		Yellow,
		Green,
		Blue
	}


	Animator animator;
	public GameObject target;
	float rotationSpeed = 15f;
	public float gravity = -9.83f;
	public float runSpeed = 0f;
	bool canMove = true;
	public GameObject currentAvatarGameObject;
	public AudioClip runSound;
	public AudioClip transformSound;
	public AudioClip scoreSound;
	public int score = 0;
	public GameObject stage;

	Vector3 newVelocity;
	Vector3 platformSpeed;
	bool platformAnimated;
	Quaternion platformFacing;
	Vector3 inputVec;
	Vector3 targetDirection;
	Vector3 targetDashDirection;
	public bool dead = false;
	public Color state = Color.Default;
	ParticleSystem whitePuff;
	ParticleSystem bluePuff;
	ParticleSystem yellowPuff;
	ParticleSystem greenPuff;
	ParticleSystem redPuff;
	ParticleSystem blueAbsorb;
	ParticleSystem greenAbsorb;
	ParticleSystem redAbsorb;
	ParticleSystem yellowAbsorb;
	ParticleSystem spark;
	ParticleSystem stars;
	public int health;
	private AudioSource runningSource;
	private AudioSource transformingSource;
	public int combo = 1;
	public GameObject gameOverScreen;
	Vector3 stageSize;
	public bool addHealth = true;
	public bool decreaseHealth = false;

	void Start(){
		health = 3;
		currentAvatarGameObject = this.transform.Find("Player").gameObject;
		animator = currentAvatarGameObject.GetComponent<Animator> ();
		whitePuff = this.transform.Find ("Puff-White").GetComponent<ParticleSystem> ();
		bluePuff = this.transform.Find ("Puff-Blue").GetComponent<ParticleSystem> ();
		yellowPuff = this.transform.Find ("Puff-Yellow").GetComponent<ParticleSystem> ();
		greenPuff = this.transform.Find ("Puff-Green").GetComponent<ParticleSystem> ();
		redPuff = this.transform.Find ("Puff-Red").GetComponent<ParticleSystem> ();
		blueAbsorb = this.transform.Find ("Absorb-Blue").GetComponent<ParticleSystem> ();
		redAbsorb = this.transform.Find ("Absorb-Red").GetComponent<ParticleSystem> ();
		greenAbsorb = this.transform.Find ("Absorb-Green").GetComponent<ParticleSystem> ();
		yellowAbsorb = this.transform.Find ("Absorb-Yellow").GetComponent<ParticleSystem> ();
		spark = this.transform.Find ("Spark").GetComponent<ParticleSystem> ();
		stars = this.transform.Find ("Stars").GetComponent<ParticleSystem> ();
		runningSource = currentAvatarGameObject.GetComponent<AudioSource>();
		transformingSource = GetComponent<AudioSource>();
		stageSize = stage.GetComponent<BoxCollider> ().bounds.size;
	}

	void FixedUpdate(){
	}

	void LateUpdate(){
		float velocityXel = transform.InverseTransformDirection(GetComponent<Rigidbody>().velocity).x * runSpeed;
		float velocityZel = transform.InverseTransformDirection(GetComponent<Rigidbody>().velocity).z * runSpeed;
	}

	void Update(){
		//update character position and facing

		if(Input.GetKeyDown(KeyCode.P)){
			Debug.Break();
		}
		//if character isn't dead, blocking, or stunned (or in a move)
		if(!dead){
			UpdateMovement();
			Switch();
			//Get input from controls relative to camera
			Transform cameraTransform = Camera.main.transform;
			//Forward vector relative to the camera along the x-z plane
			Vector3 forward = cameraTransform.TransformDirection(Vector3.forward);
			forward.y = 0;
			forward = forward.normalized;
			//Right vector relative to the camera Always orthogonal to the forward vector
			Vector3 right = new Vector3(forward.z, 0, forward.x);
			//directional inputs
			float v = Input.GetAxis("Vertical") * runSpeed;
			float h = Input.GetAxis("Horizontal") * runSpeed;
			//float dv = Input.GetAxisRaw("DashVertical");
			//float dh = Input.GetAxisRaw("DashHorizontal");
			// Target direction relative to the camera
			targetDirection = h * right + v * forward;
			// Target dash direction relative to the camera
			//targetDashDirection = dh * right + dv * -forward;
			inputVec = new Vector3(h, 0, v);
			//if there is some input (account for controller deadzone)
			if(v != 0 || h != 0){
				//set that character is moving
				animator.SetBool("Moving", true);
				if (!runningSource.isPlaying) {
					runningSource.Play ();
				}
			}
			else{
				//character is not moving
				animator.SetBool("Moving", false);
				runningSource.Stop();
			}
		}
		//character is dead or blocking, stop character
		else{
			newVelocity = new Vector3(0, 0, 0);
			inputVec = new Vector3(0, 0, 0);
		}
		if(!dead){
			//if(Input.GetButtonDown("Death")){
		//		Dead();
		//	}
		}
	}

	void UpdateMovement(){
		Vector3 motion = inputVec;
		if(!dead){
			newVelocity = motion;
		}
		//no input, character not moving
		else{
			newVelocity = new Vector3(0,0,0);
			inputVec = new Vector3(0,0,0);
		}
		RotateTowardMovementDirection ();
		// limit velocity to x and z, by maintaining current y velocity:
		GetComponent<Rigidbody>().velocity = inputVec * 1.5f;
		//return a movement value for the animator
	}

	//face character along input direction
	void RotateTowardMovementDirection(){
		Vector3 NextDir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
		if (NextDir != Vector3.zero) {
			transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(NextDir), Time.deltaTime * rotationSpeed);
		}
	}

	void Dead(){
		animator.applyRootMotion = true;
		animator.SetTrigger("DeathTrigger");
		dead = true;
	}

	void Switch(){
		Color stateToSwitch = MapInputToState ();
		if(stateToSwitch == state){
			return;
		}
		currentAvatarGameObject.SetActive (false);
		transformingSource.PlayOneShot (transformSound);
		spark.Emit (12);
		combo = 1;
		switch (stateToSwitch)
		{
		case Color.Red:
			state = Color.Red;
			redPuff.Emit (15);
			currentAvatarGameObject = this.transform.Find("Red").gameObject;
			break;
		case Color.Yellow:
			state = Color.Yellow;
			yellowPuff.Emit (15);
			currentAvatarGameObject = this.transform.Find("Yellow").gameObject;
			break;
		case Color.Green:
			state = Color.Green;
			greenPuff.Emit (15);
			currentAvatarGameObject = this.transform.Find("Green").gameObject;
			break;
		case Color.Blue:
			state = Color.Blue;
			bluePuff.Emit (15);
			currentAvatarGameObject = this.transform.Find("Blue").gameObject;
			break;
		default:
			state = Color.Default;
			whitePuff.Emit (15);
			currentAvatarGameObject = this.transform.Find("Player").gameObject;
			break;
		}
		currentAvatarGameObject.SetActive (true);
		runningSource = currentAvatarGameObject.GetComponent<AudioSource>();
		animator = currentAvatarGameObject.GetComponent<Animator> ();
	}

	Color MapInputToState(){
		if (Input.GetKey (KeyCode.Z) || Input.GetButton("Fire1")) {
			return Color.Red;
		}
		else if (Input.GetKey (KeyCode.X) || Input.GetButton("Fire3")) {
			return Color.Yellow;
		}
		else if (Input.GetKey (KeyCode.C) || Input.GetButton("Fire0")) {
			return Color.Green;
		}
		else if (Input.GetKey (KeyCode.V) || Input.GetButton("Fire2")) {
			return Color.Blue;
		}
		else{
			return Color.Default;
		}
	}
	void OnTriggerEnter(Collider other) {
		if(state.ToString() == other.tag) {
			transformingSource.PlayOneShot (scoreSound);
			if (health < 4) {
				health += 1;
				addHealth = true;
			}
			score += 1 * combo;
			combo += 1;
			stars.Emit (20);
			Vector3 effectOffset = new Vector3(0, 1, 0);
			if (state == Color.Red){
				redAbsorb.transform.position = other.gameObject.transform.position + effectOffset;
				redAbsorb.Emit(20);
			}
			if (state == Color.Blue){
				blueAbsorb.transform.position = other.gameObject.transform.position + effectOffset;
				blueAbsorb.Emit(20);
			}
			if (state == Color.Green){
				greenAbsorb.transform.position = other.gameObject.transform.position + effectOffset;
				greenAbsorb.Emit(20);
			}
			if (state == Color.Yellow){
				yellowAbsorb.transform.position = other.gameObject.transform.position + effectOffset;
				yellowAbsorb.Emit(20);
			}
			Destroy(other.gameObject);
		}
		else {
			if (other.tag == "Collide"){
			} else{
				health -= 1;
				decreaseHealth = true;
				Destroy(other.gameObject);
				combo = 1;
				if (health < 1){
					GameOver();
				}
			}
		}
	}

	void GameOver(){
		dead = true;
		gameOverScreen.SetActive (true);
		newVelocity = new Vector3(0,0,0);
		inputVec = new Vector3(0,0,0);
		newVelocity.y = GetComponent<Rigidbody>().velocity.y;
		GetComponent<Rigidbody>().velocity = newVelocity;
	}
}
