using UnityEngine;
using System.Collections;

public class Game : MonoBehaviour {
	public Enemy[] enemySpawn;
	private float enemySpawnPeriod = 20f;
	private float enemySpawnInterval;
	private float timeToNextEnemy;
	public Player player;
	public GameObject stage;

	// Use this for initialization
	void Start () {
		//calculate enemy spawn interval
		enemySpawnInterval = 2f;

	}
	
	// Update is called once per frame
	void Update () {
		if (timeToNextEnemy <= 0)
		{
			SpawnEnemy();
			timeToNextEnemy = enemySpawnInterval;
			enemySpawnInterval /= 1.01f;
		}
		else
		{
			timeToNextEnemy -= Time.deltaTime;
		}
	}

	void SpawnEnemy()
	{
		int index = Random.Range (0, enemySpawn.Length);
		Enemy e = (Enemy)Instantiate(enemySpawn[index], new Vector3(0,0,0), new Quaternion(0,0,0,0));
		e.target = player.transform;
		e.player = player;
		e.stageSize = stage.GetComponent<BoxCollider> ().bounds.size;
	}
}
