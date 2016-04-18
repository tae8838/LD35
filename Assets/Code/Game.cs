using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class Game : MonoBehaviour {
	public Enemy[] enemySpawn;
	private float enemySpawnPeriod = 20f;
	private float enemySpawnInterval;
	private float timeToNextEnemy;
	public Player player;
	public GameObject stage;
	public GameObject[] spawningAreas;

	public void Restart(){
		Application.LoadLevel (0);
	}
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
		int enemyIndex = Random.Range (0, enemySpawn.Length);
		int spawnAreaIndex = Random.Range (0, spawningAreas.Length);
		Enemy e = (Enemy)Instantiate(enemySpawn[enemyIndex], spawningAreas[spawnAreaIndex].transform.position, new Quaternion(0,0,0,0));
		e.target = player.transform;
		e.player = player;
//		e.stageSize = stage.GetComponent<BoxCollider> ().bounds.size;
	}
}
