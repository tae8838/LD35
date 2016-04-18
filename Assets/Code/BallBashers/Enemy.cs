using UnityEngine;
using System.Collections;

public class Enemy : Destroyable 
{
	public Transform target;
	public Vector3 enemyPos;
	public Transform enemyTransform;
	public Transform playerTransform;
	public Vector3 destination;
	public bool inPursuit;
	public string type;
	
	public bool hasStopped = false;

	public Player player;
	public CollisionControl collisionControl;
	public NavMeshAgent nav;

	public int pointsvalue;

	public float damage = 5f;
	public float knockPower = 1f;
	public float knockResist;
	public bool isGrounded = true;
	
	public GameObject coinPrefab;
	public float minDamageToDropCoins = 20f;
	public float coinDropRate = 0.5f;
	
	public bool isAlive = true;
	public bool damaged;
	public bool moving = false;

	[HideInInspector]
	public float knockback = 0f;
	[HideInInspector]
	public Vector3 knockDirection;

	private float knockRegression = 120f;

	private Transform myTransform;
	public bool bAttacking = false;
	private Vector3 tempTargetPositon;
	private LayerMask blockLayerMask;
	public ParticleSystem bloodfx;
	private float runSpeed;
	private float rotationSpeed = 1000f;
	private float changeDirectionIn = 1.5f;
	Vector3 motion;
	public Vector3 stageSize;
	
	public enum EnemyState 
	{
		KnockedDown,
		KnockedBack,
		Pain,
		Normal,
		Attack1,
		Pursue,
	}
	
	public EnemyState enemyState = EnemyState.Normal;

	public Enemy(Player player)
	{      
		player = player;
	}
	
	void Awake()
	{

	}

	public override void Start ()
	{
		enemyTransform = this.transform;
		motion = new Vector3 (0,0,0);
		changeDirectionIn = 1.5f;
		runSpeed = Random.Range (2f, 7f);
	}
	
	protected override void Update()
	{
		base.Update ();
		UpdateMovement();
		changeDirectionIn -= Time.deltaTime;

	}

	public virtual void FixedUpdate()
	{
		enemyPos = this.transform.position;
	}

	void UpdateMovement() {
		motion = GetMotion ();
		RotateTowardMovementDirection ();
		//reduce input for diagonal movement

		motion *= (Mathf.Abs(motion.x) == 1 && Mathf.Abs(motion.z) == 1)?0.7f:1;


		// limit velocity to x and z, by maintaining current y velocity:
		motion.Normalize();
		GetComponent<Rigidbody>().velocity = motion * runSpeed;
		float velocityXel = transform.InverseTransformDirection(GetComponent<Rigidbody>().velocity).x;
		float velocityZel = transform.InverseTransformDirection(GetComponent<Rigidbody>().velocity).z;

	}

	void RotateTowardMovementDirection(){
		if (motion.magnitude != 0) {
			var targetRotation = Quaternion.LookRotation (motion, Vector3.up);
			transform.rotation = Quaternion.Slerp (transform.rotation, targetRotation, Time.deltaTime * 10.0f);
		}
	}
	Vector3 GetMotion(){
		Vector3 output;

		if ((player.transform.position - transform.position).magnitude < 6) {
			
			if (player.state.ToString () == tag) {
				runSpeed = 4;
				output = (transform.position - target.position);
			} else {
				runSpeed = 4;
				output = (target.position - transform.position);
			}
		} else if (changeDirectionIn < 0) {
			runSpeed = Random.Range (3, 10) / 2.0f;
			changeDirectionIn = Random.Range (1, 4);
			output = new Vector3 (Random.Range (-1, 2), 0, Random.Range (-1, 2));
		} else {
			output = motion;
		}

		return output;
	}
}