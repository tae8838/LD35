using UnityEngine;
using System.Collections;

public class CollisionControl : MonoBehaviour {

	public Enemy enemy;
	public Collider attack1Collision;
	public bool attack1HasCollision;

	void OnStart()
	{
		//enemyTransform = enemy.transform;
	}

	void OnTriggerEnter(Collider other) 
	{
		if ((other.transform.tag == "Player"))
		{
			attack1HasCollision = true;
			enemy.bAttacking = true;
			//Debug.Log("Enemy is halting");

			//enemy.PursueTarget(enemy.transform);

			//enemyTransform = other.transform;
		}
	}
}