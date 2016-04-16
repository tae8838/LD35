using UnityEngine;
using System.Collections;

public class Ball : MonoBehaviour {
	
	public BallBashersGame game;

	//Components
	public CameraShake cam;
	private Animator animator;
	public SpringJoint spring;
	public TrailRenderer trail;
	public SphereCollider sphereCollider;
	public LineRenderer chain;

	//force multiplyer for damage
	public float damageMultiplier = 1f;
	public float speedMakeHitWallSound = 5f;
	public static float knockPower = 1f;
	public float slingTime = .5f;
	public int hitcount = 0;
	public bool attached = true;
	public bool retracting = false;
	private float boostBall = 1.80f;
	private float ballRadius = .75f;
	private float accelerationBoost = 0.6f;
	private float launchSpeed = 0.3f;

	//Spring variables
	public float springMaxdist = 6f;
	private float springBase = 2f;
	public float springExtents = 6f;
	private float springDamper = 20f;

	//Effects variables
	public float whooshSpeed = 40f;
	public float trailSpeed = 4;
	public float smokeSpeed = 60;
	public float sparkSpeed = 6;
	
	//Effects Prefab
	public GameObject ballDamageEffect;
	public GameObject comboPrefab;
	public GameObject smokefx;
	public GameObject sparkfx;
	public ParticleSystem smoke;
	public ParticleSystem spark;
	
	//Sounds
	public AudioClip HitWallSound;
	public AudioSource audioPlayer;
	public AudioClip whooshSound;
	public AudioClip comboSound;

	void Awake() 
	{
		//controlled by ball level of force upgrade
		spring = gameObject.GetComponent<SpringJoint>();
		trail = gameObject.GetComponent<TrailRenderer>();
		springMaxdist = spring.maxDistance;
		smoke = smokefx.GetComponentInChildren<ParticleSystem>();
		spark = sparkfx.GetComponentInChildren<ParticleSystem>();
		chain = gameObject.GetComponent<LineRenderer>();
		sphereCollider = gameObject.GetComponent<SphereCollider>();
	}

	void Start()
	{
		smoke.enableEmission = false;
		spark.enableEmission = false;
		springMaxdist = 1f;
	}
	
	public IEnumerator SlingBall (float seconds)
	{
		spring.maxDistance = springExtents;
		yield return new WaitForSeconds(seconds);
		spring.maxDistance = springBase;
	}
	
	void Update()
	{
		//Add some momentum to ball so it requires less to get it going.
		GetComponent<Rigidbody>().velocity += GetComponent<Rigidbody>().velocity * accelerationBoost * Time.deltaTime;

		if (Input.GetButton("Ball"))
		{
			//If Button is held
			if (springMaxdist == springBase)
			{
				springMaxdist = springExtents;
				spring.maxDistance = springExtents;
				attached = false;
//				Vector3 direction = (BallBashersGame.Game.Player.transform.position + transform.position);
				if (Chain.dist < (spring.maxDistance - 1f))
					GetComponent<Rigidbody>().velocity = GetComponent<Rigidbody>().velocity * boostBall;

				sphereCollider.radius = ballRadius * 1.5f;
				BallBashersGame.Game.Player.runSpeed = BallBashersGame.Game.Player.runSpeed * 1.25f;
				hitcount = 0;
				accelerationBoost = launchSpeed;
			}
		}
		else
		{
			springMaxdist = springBase;
			spring.maxDistance = springBase;
			retracting = true;
		}

		if (Chain.dist > (springBase * 6))
		{
			retracting = true;
			BallBashersGame.Game.Player.runSpeed = BallBashersGame.Game.Player.runSpeed;
		}

		if (Chain.dist < (springBase * 4) && retracting)
		{
			attached = true;
			sphereCollider.radius = ballRadius;
		}

		if (Chain.dist < (springBase * 5) && attached)
		{
			retracting = false;
		}

		if (retracting)
		{
			spring.damper = springDamper;
		}

		if(attached)
		{
			spring.damper = 0;
		}

		if (Chain.dist <= (springBase * 3))
		{
			hitcount = 0;
		}
	}
	
	void OnCollisionEnter(Collision collision) 
	{
		GameObject target = collision.gameObject;
		if(target.layer == 8 || target.layer == 13 || target.layer == 14 || target.layer == 15) //8 is block, 13 is enemy 11 is ball
		{
			float speed = collision.relativeVelocity.magnitude;

			if (target.layer == 14)
			{
//				//target.GetComponent<Enemy>().Knockedback = 0.1f;
//				Vector3 direction = (target.transform.position - BallBashersGame.Game.Player.transform.position);
//				direction.y = 0;
//				direction.Normalize();
//				target.GetComponent<BounceBall>().knockDirection = direction;
			}

			if (target.layer == 13 || target.layer == 15) //13 is enemy  15 is destroyable
			{
				if (DoDamage(target, (speed * damageMultiplier)))
				{
//					Debug.Log("Collision magnitude" + (speed * damageMultiplier));
					Instantiate(ballDamageEffect, transform.position, Quaternion.identity);

					if (target.layer == 13) //13 is enemy
					{
						float damagetoShake;
						damagetoShake = speed / 80;
						cam.ShakeCam(damagetoShake);
						Vector3 direction =  (target.transform.position - (transform.position / 2));  
						direction.y = 0;
						direction.Normalize();
						Debug.DrawRay(transform.position, direction, Color.red, 3);
						target.GetComponent<Enemy>().knockDirection = direction;
						hitcount += 1;
						BallBashersGame.Game.CrowdCheer(speed / 300);

						if (hitcount > 1)
						{
							audioPlayer.clip = comboSound;
							audioPlayer.loop = false;
							audioPlayer.volume = .8f;
							audioPlayer.Play ();
							Instantiate(comboPrefab, transform.position, Quaternion.identity);
						}
					}
				}
			}
		}
		else if (target.layer == 16) //16 is pickups
		{
			//allow ball to pickup
			Pickup pickup = target.gameObject.GetComponent<Pickup>();
			if (pickup != null)
			{
				pickup.CollectMe();
			}
		}
		else
		{
			if (collision.relativeVelocity.sqrMagnitude > speedMakeHitWallSound * speedMakeHitWallSound)
			{
				audioPlayer.clip = HitWallSound;
				audioPlayer.loop = false;
				audioPlayer.Play ();
			}
		}
	}

	void LateUpdate()
	{
		if (GetComponent<Rigidbody>().velocity.magnitude >= trailSpeed)
		{
			trail.enabled = true;
			smoke.enableEmission = true;
		}
		else
		{
			trail.enabled = false; 
			smoke.enableEmission = false;
		}

		if (GetComponent<Rigidbody>().velocity.magnitude >= smokeSpeed)
		{
			smoke.enableEmission = true;
		}
		else
		{
			smoke.enableEmission = false;
		}

		if (GetComponent<Rigidbody>().velocity.magnitude >= 2 && GetComponent<Rigidbody>().velocity.magnitude >= sparkSpeed)
		{
			spark.enableEmission = true;
		}
		else
		{
			spark.enableEmission = false;
		}

		if (GetComponent<Rigidbody>().velocity.magnitude >= whooshSpeed)
		{
			//play audio
		}
	
	}
	
	private int CalculateDamage(float speed)
	{
		//add damage curve calculation here in the future
		return (int)(speed * damageMultiplier);
	}
	
	private bool DoDamage(GameObject target, float speed)
	{
			Destroyable d = (Destroyable)target.GetComponent<Destroyable>();
			return d.TakeDamage(CalculateDamage(speed));
	}
}