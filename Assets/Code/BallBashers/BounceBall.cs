using UnityEngine;
using System.Collections;

public class BounceBall : MonoBehaviour {
	
	public BallBashersGame game;
	
	//force multiplyer for damage
	public float damageMultiplier = 1f;
	public float speedMakeHitWallSound = 5f;
	public int knockPower = 15;
	public float knockedBounce = 10f;
	
	public float knockback = 0f;
	
	public Vector3 knockDirection;
	private float knockDamper = 0.01f;
	private float knockRegression = 10f;
	
	//physics
	
	//model
	
	//hitFXdamage
	public GameObject ballDamageEffect;
	
	//hitSound
	
	//trail
	
	public AudioClip HitWallSound;
	public AudioSource audioPlayer;
	
	
	void Awake() 
	{
		//controlled by ball level of force upgrade
	}
	
	void FixedUpdate() 
	{
		//		if (gameObject.rigidbody.maxAngularVelocity > trailSpeed)
		//		{
		//			GetComponent<TrailRenderer>().enabled = true; 
		//		}
		//		if (Input.GetButtonDown("Fire1"))
		//		{
		//			Debug.Log("Button Press");
		//		}
		//Debug.Log("We are in update");
		
		if (knockback < 0f)
		{
			
			knockback = 0f;
		}
		else if (knockback > 0)
		{	
			
			//GetComponent<CharacterController>().Move(knockDirection*knockback*knockDamper);
			transform.position += knockDirection*knockback*knockDamper*knockedBounce;
			knockback -= Time.deltaTime * knockRegression;
		}
		
	}	
	
	void OnCollisionEnter(Collision collision) 
	{
		GameObject target = collision.gameObject;
		if(target.layer == 8 || target.layer == 13 || target.layer == 14 || target.layer == 12) //8 is block, 13 is enemy 11 is ball
		{
			
			float speed = collision.relativeVelocity.magnitude;
			
			if (target.layer == 14)
			{
				//target.GetComponent<Enemy>().Knockedback = 0.1f;
				Vector3 direction = (target.transform.position - BallBashersGame.Game.Player.transform.position);
				direction.y = 0;
				direction.Normalize();
				target.GetComponent<BounceBall>().knockback = knockPower;
				target.GetComponent<BounceBall>().knockDirection = direction;
				Debug.Log("We hit a ball");
			}

			if (target.layer == 12)
			{

				//target.GetComponent<Enemy>().Knockedback = 0.1f;
				//rigidbody.AddForce(Vector3.forward * knockedBounce);
				GetComponent<Rigidbody>().AddForce(knockedBounce, 0, 0);
				Debug.Log(GetComponent<Rigidbody>().velocity);
//				Vector3 direction = (target.transform.position - BallBashersGame.Game.Player.transform.position);
//				direction.y = 0;
//				direction.Normalize();
//				target.GetComponent<BounceBall>().knockback = knockPower;
//				target.GetComponent<BounceBall>().knockDirection = direction;
				Debug.Log("We hit a wall");
				audioPlayer.clip = HitWallSound;
				Instantiate(ballDamageEffect, transform.position, Quaternion.identity);
			}


			if (target.layer == 13)
			{
				if (DoDamage(target, (speed * damageMultiplier)))
				{
					//Debug.Log("Collision magnitude" + (speed * damageMultiplier));
					Instantiate(ballDamageEffect, transform.position, Quaternion.identity);

						//target.GetComponent<Enemy>().Knockedback = 0.1f;
						Vector3 direction = (target.transform.position - BallBashersGame.Game.Player.transform.position);
						direction.y = 0;
						direction.Normalize();
						target.GetComponent<Enemy>().knockback = knockPower;
						target.GetComponent<Enemy>().knockDirection = direction;
				}
			}
		}
		else if (target.layer == 16) //16 is pickups
		{
			//do nothing
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
	
	private int CalculateDamage(float speed)
	{
		//add damage curve calculation here in the future
		return (int)(speed * damageMultiplier);
	}
	
	private bool DoDamage(GameObject target, float speed)
	{
		Destroyable d = (Destroyable)target.GetComponent<Destroyable>();
		return d.TakeDamage(CalculateDamage(speed));
		//		Debug.Log("Ball does " + (power + force) + " damage");
	}
}