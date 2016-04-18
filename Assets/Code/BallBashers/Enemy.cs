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
	public int minCoinsToDrop = 1;
	public int maxCoinsToDrop = 2;
	public float coinDropRate = 0.5f;
	
	public bool isAlive = true;
	public bool damaged;
	public bool moving = false;
	private int xBoundary = 0;
	private int zBoundary = 0;

	[HideInInspector]
	public float knockback = 0f;
	[HideInInspector]
	public Vector3 knockDirection;

	public float timeForAttack = 0.3f;
	public float timeBetweenAttacks = 1f;

	private float knockRegression = 120f;

	private Transform myTransform;
	public bool bAttacking = false;
	private Vector3 tempTargetPositon;
	private LayerMask blockLayerMask;
	public ParticleSystem bloodfx;
	private float runSpeed;
	private float rotationSpeed = 3f;
	private float changeDirectionIn = 1f;
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
		motion = new Vector3 (Random.Range (-1, 2), 0, Random.Range (-1, 2));
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
		UpdateBoundaries ();
		motion = GetMotion ();
		RotateTowardMovementDirection (motion);
		//reduce input for diagonal movement

		motion *= (Mathf.Abs(motion.x) == 1 && Mathf.Abs(motion.z) == 1)?0.7f:1;

		// limit velocity to x and z, by maintaining current y velocity:
		motion.Normalize();
		GetComponent<Rigidbody>().velocity = motion * runSpeed;
		float velocityXel = transform.InverseTransformDirection(GetComponent<Rigidbody>().velocity).x;
		float velocityZel = transform.InverseTransformDirection(GetComponent<Rigidbody>().velocity).z;
	}

	void RotateTowardMovementDirection(Vector3 NextDir){
		if (NextDir != Vector3.zero) {
			transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(NextDir), Time.deltaTime * rotationSpeed);
		}
	}
	Vector3 GetMotion(){
		Vector3 output;

		if ((player.transform.position - transform.position).magnitude < 3) {
			
			if (player.state.ToString () == tag) {
				runSpeed = 2;
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

		if (zBoundary == 1 && output.z > 0) {
			output.z = 0;
		}
		if (zBoundary == -1 && output.z < 0) {
			output.z = 0;
		}
		if (xBoundary == 1 && output.x > 0) {
			output.x = 0;
		}
		if (xBoundary == -1 && output.x < 0) {
			output.x = 0;
		}

		return output;
	}

	void UpdateBoundaries(){
		zBoundary = 0;
		xBoundary = 0;

		if (stageSize.z / 2.2 < transform.position.z) {
			zBoundary = 1;
		}
		if (stageSize.z / -2.2 > transform.position.z) {
			zBoundary = -1;
		}
		if (stageSize.x / 2.2 < transform.position.x) {
			xBoundary = 1;
		}
		if (stageSize.x / -2.2 > transform.position.x) {
			xBoundary = -1;
		}
	}
}