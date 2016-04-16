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
	
	public bool hasStopped = false;

	public Player player;
	public EnemyController enemy;
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

	public Enemy()
	{      
		switch(enemyState)
		{      
		case EnemyState.Normal:

			break;
			
		case EnemyState.KnockedBack:

			break;
			
		case EnemyState.KnockedDown:
			break;

		case EnemyState.Pain:
			break;

		case EnemyState.Attack1:
			break;

		case EnemyState.Pursue:
			break;

		default:
			break;
		}
	}
	
	void Awake()
	{

	}

	public override void Start ()
	{
		player = BallBashersGame.Game.Player;
		blockLayerMask = 1 << 8;
		//animator = GetComponent<Animator>();
		nav = GetComponent<NavMeshAgent>();
		enemyTransform = this.transform;



		playerTransform = player.transform;

		target = playerTransform;

		//Set a random knock resist
		knockResist = Random.Range(.01f, .05f);

		inPursuit = true;
		//StartCoroutine("COPursueTarget");

		StartCoroutine(COPursueTarget(playerTransform));
	}
	
	protected override void Update()
	{
		base.Update();

		BloodLoss();

		// Calculate knockback
		if (knockback < 0f)
		{
			knockback = 0f;
			//animator.SetBool("Knockback", false);
			GetComponent<Rigidbody>().Sleep();
		}
		else if (knockback > 0)
		{
			animator.SetTrigger("Knockback");
			KnockBack();
			knockback -= Time.deltaTime * knockRegression;
		}
		
		if (!bAttacking && knockback==0f)
		{

		}
	}

	public IEnumerator COPursueTarget(Transform pursuitTarget)
	{
//		Debug.Log("COPursueTarget");
		while (inPursuit)
		{

			//Debug.Log("In PursueTarget");
			target = playerTransform;
			Navigation(playerTransform.position);
//			Debug.Log("In hot pursuit" + pursuitTarget);
			moving = true;
			nav.speed = 2;
			nav.acceleration = 600;
			hasStopped = false;
			yield return null;
		}
		yield return null;
	}
	
	public IEnumerator HaltMovement()
	{
		//Debug.Log("In HaltMovement");
		inPursuit = false;
		//StartCoroutine(COPursueTarget(playerTransform, false));
		nav.speed = 0;
		nav.acceleration = 0;
		hasStopped = true;
		moving = false;
		animator.SetBool("Moving", false);
		Navigation(enemyPos);

		target = enemyTransform;
		//Debug.Log("target is enemy transform" + enemyTransform);
		yield return 0;
	}

	//move toward destination
	public Vector3 Navigation(Vector3 navDestination)
	{
		//GetComponent<NavMeshAgent>().destination = target.position;
		//Debug.Log("In Navigation");
		//nav target set to Navigation vector
		destination = navDestination;
		return destination;
	}

	public virtual void FixedUpdate()
	{
		tempTargetPositon = target.position;
		tempTargetPositon.y = transform.position.y;
		Vector3 raycastOrigin = transform.position;
		
		Debug.DrawRay(raycastOrigin, target.position);

		//PursueTarget(target);

//		Debug.Log("Nav velocity =" + nav.velocity.magnitude);
//		Debug.Log("Nav acceleration =" + nav.acceleration);

		if (nav.velocity.magnitude > 0 && hasStopped == false)
		{
			//if (moving == true)
				animator.SetBool("Moving", true);
		}
		else
		{
			animator.SetBool("Moving", false);
		}

		float xVel = transform.InverseTransformDirection(nav.velocity).x;
		float zVel = transform.InverseTransformDirection(nav.velocity).z;
		
		//Update animator with movement values
		animator.SetFloat("Velocity X", xVel);
		animator.SetFloat("Velocity Z", zVel);

		CheckForCollisions();

		enemyPos = this.transform.position;

		destination = target.position;

		nav.destination = destination;
	}

	void BloodLoss()
	{
		float bleedamount = 30f;
		bleedamount -= Time.deltaTime * .95f;
		bloodfx.emissionRate -= Time.deltaTime * bleedamount;
	}
	
	void OnCollisionEnter(Collision collision) 
	{
		//Debug.Log("Enemy collided with something");
	}

	void OnCollisionExit(Collision collision) 
	{
		if((collision.transform.tag == "Player" ))
		{

		}
	}
	
	protected override bool ApplyDamage(int damage) 
	{ 
		bool damaged = base.ApplyDamage(damage);
		knockback = damage * Ball.knockPower;

		if (damage > minDamageToDropCoins)
		{
			if (health > 0)
			{
				dropCoins(coinDropRate);
				enemyState = EnemyState.KnockedBack;
				KnockBack();
			}
			else
			{
				dropCoins(1f);
			}
		}
		return damaged;
	}
	
	protected void dropCoins(float dropRate)
	{
		if (Random.value < dropRate)
		{
			for (int i = 0; i < Random.Range(minCoinsToDrop, maxCoinsToDrop+1); ++i)
			{
				GameObject coin = Instantiate(coinPrefab, transform.position, transform.rotation) as GameObject;
				coin.GetComponent<Rigidbody>().velocity = new Vector3(Random.Range(-3,3),Random.Range(10,14),Random.Range(-3,3));
			}
		}
	}
	
	public virtual void KnockBack()
	{
		float knockDamper = 0.1f;
		GetComponent<Rigidbody>().AddForce((GetComponent<Rigidbody>().velocity * knockback * knockDamper), ForceMode.Impulse);
		bloodfx.emissionRate = damage * 4;
	}

	public bool IsGrounded()
	{
		float distanceToGround;
		float threshold = .2f;
		
		RaycastHit hit;
		if (Physics.Raycast(transform.position, -Vector3.up, out hit, .25f))
		{
			distanceToGround = hit.distance;
			if (distanceToGround < threshold)
			{
				isGrounded = true;
			}
			else
			{
				isGrounded = false;
			}
		}
		return isGrounded;
	}
		
		protected override void Die()
	{
		base.Die ();
	}

	public IEnumerator COAttacking()
	{
		while(bAttacking)
		{
			//Debug.Log("In COAttack");
			animator.SetTrigger("Attack1");
			yield return new WaitForSeconds(timeForAttack);
			DoDamage();
			yield return new WaitForSeconds(timeBetweenAttacks);
		}
	}
	
	public void DoDamage()
	{
		Player p = (Player)playerTransform.GetComponent("Player");
		BallBashersGame.Game.TakeDamage(1);
//		p.ApplyDamage(damage);
		Vector3 direction = (target.position - transform.position).normalized;
//		p.knockback = knockPower * damage;
//		p.knockDirection = direction;
//		p.KnockBack(knockback);
	}

	public void CheckForCollisions()
	{

	}
}