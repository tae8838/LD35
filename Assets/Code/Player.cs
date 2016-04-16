using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour{

	public enum Element{
		Default,
		Fire,
		Water,
		Earth
	}

	Animator animator;
	public GameObject target;
	float rotationSpeed = 15f;
	public float gravity = -9.83f;
	public float runSpeed = 8f;
	bool canMove = true;
	public int health;

	Vector3 newVelocity;
	Vector3 platformSpeed;
	bool platformAnimated;
	Quaternion platformFacing;
	Vector3 inputVec;
	Vector3 targetDirection;
	Vector3 targetDashDirection;
	bool dead = false;
	public Element state = Element.Default;
	public float gauge1;
	public float gauge2;
	public float gauge3;

	void Start(){
		animator = this.GetComponent<Animator>();
	}

	void FixedUpdate(){
		GetComponent<Rigidbody>().AddForce(0, gravity, 0, ForceMode.Acceleration);
		//check if character can move
		if(canMove){
			UpdateMovement();
		}
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
			animator.SetBool("Moving", true);
			if(v != 0 && h != 0){
				//set that character is moving
				animator.SetBool("Moving", true);
			}
			else{
				//character is not moving
				animator.SetBool("Moving", false);
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

		if(!dead){
			//reduce input for diagonal movement
			motion *= (Mathf.Abs(inputVec.x) == 1 && Mathf.Abs(inputVec.z) == 1)?0.7f:1;
			//apply velocity based on platform speed to prevent sliding
			float platformVelocity = platformSpeed.magnitude * .4f;
			Vector3 platformAdjust = platformSpeed * platformVelocity;
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
		if(!dead){
			//if character is moving but not strafing
			if(inputVec != Vector3.zero){
				//take the camera orientated input vector and apply it to our characters facing with smoothing
				transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(targetDirection), Time.deltaTime * rotationSpeed);
			}
		}
	}

	void Dead(){
		animator.applyRootMotion = true;
		animator.SetTrigger("DeathTrigger");
		dead = true;
	}

	void Switch(string stateChange){
		switch (stateChange)
		{
		case "fire":
			state = Element.Fire;

			break;
		case "water":
			state = Element.Water;
			break;
		case "earth":
			state = Element.Earth;
			break;
		default:
			break;
		}
	}
}
