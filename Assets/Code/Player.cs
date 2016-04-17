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
	public float runSpeed = 8f;
	bool canMove = true;
	GameObject currentAvatarGameObject;
	public AudioClip runSound;
	public AudioClip transformSound;
	public int score = 0;

	Vector3 newVelocity;
	Vector3 platformSpeed;
	bool platformAnimated;
	Quaternion platformFacing;
	Vector3 inputVec;
	Vector3 targetDirection;
	Vector3 targetDashDirection;
	bool dead = false;
	public Color state = Color.Default;
	public float gauge1;
	public float gauge2;
	public float gauge3;
	ParticleSystem whitePuff;
	ParticleSystem bluePuff;
	ParticleSystem yellowPuff;
	ParticleSystem greenPuff;
	ParticleSystem redPuff;
	ParticleSystem spark;
	public int health;
	private AudioSource runningSource;
	private AudioSource transformingSource;

	void Start(){
		health = 3;
		currentAvatarGameObject = this.transform.Find("Shark").gameObject;
		animator = currentAvatarGameObject.GetComponent<Animator> ();
		whitePuff = this.transform.Find ("Puff-White").GetComponent<ParticleSystem> ();
		bluePuff = this.transform.Find ("Puff-Blue").GetComponent<ParticleSystem> ();
		yellowPuff = this.transform.Find ("Puff-Yellow").GetComponent<ParticleSystem> ();
		greenPuff = this.transform.Find ("Puff-Green").GetComponent<ParticleSystem> ();
		redPuff = this.transform.Find ("Puff-Red").GetComponent<ParticleSystem> ();
		spark = this.transform.Find ("Spark").GetComponent<ParticleSystem> ();
		runningSource = currentAvatarGameObject.GetComponent<AudioSource>();
		transformingSource = GetComponent<AudioSource>();
	}

	void FixedUpdate(){
		GetComponent<Rigidbody>().AddForce(0, gravity, 0, ForceMode.Acceleration);
		//check if character can move
		if(canMove){
			UpdateMovement();
		}
		Switch();
	}

	void LateUpdate(){
		//Get local velocity of charcter
		float velocityXel = transform.InverseTransformDirection(GetComponent<Rigidbody>().velocity).x;
		float velocityZel = transform.InverseTransformDirection(GetComponent<Rigidbody>().velocity).z;
		//Update animator with movement values
		//animator.SetFloat("Input X", velocityXel / runSpeed);
		//animator.SetFloat("Input Z", velocityZel / runSpeed);
	}

	void Update(){
		//update character position and facing
		UpdateMovement();
		if(Input.GetKeyDown(KeyCode.P)){
			Debug.Break();
		}
		//if character isn't dead, blocking, or stunned (or in a move)
		if(!dead){
			//Get input from controls relative to camera
			Transform cameraTransform = Camera.main.transform;
			//Forward vector relative to the camera along the x-z plane
			Vector3 forward = cameraTransform.TransformDirection(Vector3.forward);
			forward.y = 0;
			forward = forward.normalized;
			//Right vector relative to the camera Always orthogonal to the forward vector
			Vector3 right = new Vector3(forward.z, 0, forward.x);
			//directional inputs
			float v = Input.GetAxis("Vertical");
			float h = Input.GetAxis("Horizontal");
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

	float UpdateMovement(){
		Vector3 motion = inputVec;
		RotateTowardMovementDirection ();
		if(!dead){
			//reduce input for diagonal movement
			motion *= (Mathf.Abs(inputVec.x) == 1 && Mathf.Abs(inputVec.z) == 1)?0.7f:1;
			//apply velocity based on platform speed to prevent sliding
			newVelocity = motion * runSpeed;
		}
		//no input, character not moving
		else{
			newVelocity = new Vector3(0,0,0);
			inputVec = new Vector3(0,0,0);
		}

		// limit velocity to x and z, by maintaining current y velocity:
		newVelocity.y = GetComponent<Rigidbody>().velocity.y;
		GetComponent<Rigidbody>().velocity = newVelocity;

		//return a movement value for the animator
		return inputVec.magnitude;
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
		if (stateToSwitch == state)
			return;
		currentAvatarGameObject.SetActive (false);
		transformingSource.PlayOneShot (transformSound);
		spark.Emit (12);
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
			currentAvatarGameObject = this.transform.Find("Shark").gameObject;
			break;
		}
		currentAvatarGameObject.SetActive (true);
		animator = animator = currentAvatarGameObject.GetComponent<Animator> ();
	}

	Color MapInputToState(){
		if (Input.GetKey (KeyCode.Z)) {
			return Color.Red;
		} else if (Input.GetKey (KeyCode.X)) {
			return Color.Yellow;
		} else if (Input.GetKey (KeyCode.C)) {
			return Color.Green;
		} else if (Input.GetKey (KeyCode.V)) {
			return Color.Blue;
		} else{
			return Color.Default;
		}
	}
	void OnTriggerEnter(Collider other) {
		if (currentAvatarGameObject.tag == other.tag) {
			score += 1;
			Destroy (other.gameObject);
		} else {
			health -= 1;
			Destroy (other.gameObject);
		}
		print(health);
		print (currentAvatarGameObject.tag);
		print (other.tag);

	}
}
