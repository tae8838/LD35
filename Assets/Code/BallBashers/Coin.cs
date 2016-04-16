using UnityEngine;
using System.Collections;

public class Coin : Pickup {
	
	public int coinAmount = 10;
	float spinSpeed = 300f;
	Player player;
	float distanceToPlayer;
	float collectDistance = 5f;
	float moveSpeed = 2f;
	Vector3 position;
	public bool grounded = false;
	
	
	public override void Start()
	{
		player = BallBashersGame.Game.Player;
	}
	
	public override void Update()
	{
		base.Update();
		transform.Rotate(Vector3.up * Time.deltaTime * spinSpeed, Space.World);
		distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
		
		if (grounded && distanceToPlayer < collectDistance)
		{
			//move towards player at collectSpeed
			position = player.transform.position;
			transform.position = Vector3.Lerp (transform.position, position, Time.deltaTime * moveSpeed);
		}
	}
	
	public override void CollectMe()
	{
		BallBashersGame.Game.AddCoin(coinAmount);
		Instantiate(CollectedFXPrefab, transform.position, transform.rotation);
		Destroy(transform.gameObject);
	}
	
	void OnCollisionEnter(Collision collision) 
	{
		GameObject target = collision.gameObject;
		if(target.layer == 12) //12 is ground
		{
			GetComponent<AudioSource>().Play();
			grounded = true;
		}
	}
}