using UnityEngine;
using System.Collections;

public class Heart : Pickup {
	
	public int heartAmount = 1;

	public override void Start()
	{

	}
	
	public override void Update()
	{
		base.Update();
	}
	
	public override void CollectMe()
	{
		BallBashersGame.Game.GiveHealth(1);
		Instantiate(CollectedFXPrefab, transform.position, transform.rotation);
		Destroy(transform.gameObject);
	}
	
	void OnCollisionEnter(Collision collision) 
	{

	}
}