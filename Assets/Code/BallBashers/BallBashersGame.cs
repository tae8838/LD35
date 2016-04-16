using UnityEngine;
using System.Collections;

public enum BallBashersGameState { Playing, Won, Lost };

public class BallBashersGame : MonoBehaviour
{
    public static BallBashersGame Game;
	
	public int time;
    private BallBashersGameState gameState;

//	public GUIManager guiManager;
	public PlayerProfile playerProfile;

	public GameObject heart1;
	public GameObject heart2;
	public GameObject heart3;
	public int playerHealth = 3;
	public GameObject collectedHeartPrefab;

	public TimeKeeper timeKeeper;

//	public KeyboardInput keyboardInput;

//	public CharacterSpawn characterSpawn;
	public GameObject[] enemySpawns;
	public GameObject enemyPrefab;
	public float timeAllotted = 180;
	public int enemiesToSpawn = 30;
	public float gracePeriod = 3f;
	public float enemySpawnPeriod = 150f;
	
	public GameObject blockSpawns;
	public GameObject blockPrefab;
	public int blockCount = 2;
	
	public float pauseLength = 0.3f;
	public float pauseScale = 0.2f;
	
	public int score;
	public int coin = 0;

	public GameObject crowdAudio;
	public AudioSource crowdAudioPlayer;
	public AudioClip crowdcheer;
	private float crowdvolume = .2f;
	float crowdroar = .2f;

	public bool canStart = false;
	public AudioSource audioPlayer;
	public AudioClip BipCount;
	public AudioClip BipStart;
	public float CountDownFrom = 5;
	private bool _isCounting = false;
	
	//	[HideInInspector]
//	public Character localCharacter;
	[HideInInspector]
	public GameObject BallAndChain;
	public Player Player;

	private float enemySpawnInterval;
	private float timeToNextEnemy;
	
    void Awake()
    {

		if(Game == null)
		{
			DontDestroyOnLoad(gameObject);
			Game = this;
		}
		else if (Game != this)
		{
			Destroy(gameObject);
		}

        Coin = 0;
		Game = this;
		BallAndChain = GameObject.Find("BallAndChain");
        
        gameState = BallBashersGameState.Playing;
        
        Time.timeScale = 1.0f;
		time = 100;
		
		//blocks spawning
		spawnBlocks(blockCount);
		
		//calculate enemy spawn interval
		enemySpawnInterval = enemySpawnPeriod / enemiesToSpawn;
    }
	
	void Start()
	{
		playerProfile = new PlayerProfile();
		playerProfile.LoadProfile();
		StartGame();
		
		if (CountDownFrom > 0)
		{
			CountDown();
		}
		
		//play crowd noise
		crowdAudioPlayer.clip = crowdcheer;
		crowdAudioPlayer.loop = true;
		crowdAudioPlayer.volume = .2f;
		crowdAudioPlayer.Play ();
	}
	
	void Update()
	{
		if (Time.timeSinceLevelLoad > gracePeriod && enemiesToSpawn > 0)
		{
			if (timeToNextEnemy <= 0)
			{
				SpawnEnemy();
				timeToNextEnemy = enemySpawnInterval;
				--enemiesToSpawn;
			}
			else
			{
				timeToNextEnemy -= Time.deltaTime;
			}
		}

		crowdroar -= Time.deltaTime * (.9999f / 10);
		crowdvolume = crowdroar;
		
		if (crowdvolume < .2f)
			crowdvolume = .2f;
		
		if (crowdroar < 0f)
			crowdroar = 0f;

		crowdAudioPlayer.volume = crowdvolume;
	}

	public void CrowdCheer(float cheerlevel)
	{
		StartCoroutine(COCrowdCheer(cheerlevel));
	}

	IEnumerator COCrowdCheer(float cheerlevel)
	{
		crowdroar += cheerlevel;
//		Debug.Log ("Crowd Roar " + crowdroar);
//		Debug.Log ("Crowd Volume " + crowdvolume);
		yield return null;
	}
		
		void spawnBlocks(int count)
	{
		if (count > blockSpawns.transform.childCount)
		{
			count = blockSpawns.transform.childCount;
		}
		Transform[] blockTransform = new Transform[blockSpawns.transform.childCount];
		bool[] blockUsed = new bool[blockSpawns.transform.childCount];
		int i = 0;
		foreach (Transform child in blockSpawns.transform)
		{
			blockTransform[i++] = child;
		}
		
		for (int j = 0; j < count; ++j)
		{
			do
			{
				i = Random.Range(0, blockSpawns.transform.childCount);
			}
			while (blockUsed[i]);
			blockUsed[i] = true;
			Instantiate(blockPrefab, blockTransform[i].position, blockTransform[i].rotation);
		}
	}

	public void TakeDamage(int pain)
	{
		playerHealth -= pain;

		if (playerHealth == 2)
		{
//			heart3.renderer.enabled = false;
//			heart2.renderer.enabled = true;
//			heart1.renderer.enabled = true;
			Instantiate(collectedHeartPrefab, heart3.transform.position, transform.rotation);
		}

		if (playerHealth == 1)
		{
//			heart2.renderer.enabled = false;
//			heart1.renderer.enabled = true;
			Instantiate(collectedHeartPrefab, heart2.transform.position, transform.rotation);
		}

		if (playerHealth == 0)
		{
//			heart1.renderer.enabled = false;
			Instantiate(collectedHeartPrefab, heart1.transform.position, transform.rotation);
			EndGame();
		}
	}

	public void GiveHealth(int pain)
	{
		playerHealth += pain;

		if (playerHealth == 4)
		{
			playerHealth = 3;
		}

		if (playerHealth == 3)
		{
//			heart3.renderer.enabled = true;
//			heart2.renderer.enabled = true;
//			heart1.renderer.enabled = true;
		}
		
		if (playerHealth == 2)
		{
//			heart3.renderer.enabled = false;
//			heart2.renderer.enabled = true;
//			heart1.renderer.enabled = true;
		}
		
		if (playerHealth == 1)
		{
//			heart2.renderer.enabled = false;
//			heart1.renderer.enabled = true;
		}
		
		if (playerHealth == 0)
		{
//			heart1.renderer.enabled = false;
			EndGame();
		}
	}
	
	void SpawnEnemy()
	{
		int spawnIndex = Random.Range(0, 3);
		Instantiate(enemyPrefab, enemySpawns[spawnIndex].transform.position, enemySpawns[spawnIndex].transform.rotation);
	}
	
    void OnGUI(){
    
        GUILayout.Space(10);

        if (gameState == BallBashersGameState.Lost)
        {
            GUILayout.Label("You Lost!");
            if (GUILayout.Button("Try again"))
            {
                Application.LoadLevel(Application.loadedLevel);
            }
        }
        else if (gameState == BallBashersGameState.Won)
        {
            GUILayout.Label("You won!");
            if (GUILayout.Button("Play again"))
            {
                Application.LoadLevel(Application.loadedLevel);
            }
        }
    }
	
	/*
    public void DestroyedBlock()
    {
        blocksHit++;
//		Time.timeScale = pauseScale;
//		PauseTime(pauseLength);
//        Debug.Log("Paused Time!");
        if (blocksHit >= totalBlocks)
        {
            WonGame();
        }
    }
    */
	
	public void AddScore(int scoreAmount)
	{
		Score += scoreAmount;
	}
	
	public void AddCoin(int coinAmount)
	{
		Coin += coinAmount;
	}
	
    public void WonGame()
    {
        Time.timeScale = 0.0f; //Pause game
        gameState = BallBashersGameState.Won;
		Debug.Log("You won the game!");
    }
    
    void CountDown()
    {
        if (!_isCounting)
        {
            StartCoroutine(Wait());
        }
    }

    IEnumerator Wait()
    {
        _isCounting = true;
        for (float i = CountDownFrom; i >= 0; i--)
        {
//          Debug.Log(i);
            if (i == 3||i== 2||i== 1)
                PlayAudio(BipCount);
            else if (i== 0)
            {
                PlayAudio(BipStart);
                canStart= true;

            }
            yield return new WaitForSeconds(1);
        }
        
        _isCounting = false;
    }
	
	public void StartGame()
	{
		Score = 0;
//		if (localCharacter != null)
//		{
//			Destroy(localCharacter.gameObject);
//		}
//		if (characterSpawn != null)
//		{
//			localCharacter = characterSpawn.SpawnCharacter(this);
//			localCharacter.characterMotor.buttonInput = buttonInput;
//			localCharacter.characterMotor.keyboardInput = keyboardInput;
//			camFollow.Target = localCharacter.transform;
//		}
		Time.timeScale = 1f;
		timeKeeper.StartCountdown(timeAllotted);
	}
	
	public void EndGame()
	{
		// TODO: show GUI: score, highest point value word, longest word, alltime best score, play again, main menu
//		if (localCharacter != null) localCharacter.HideCharacter(true);
//		guiManager.RenderEndGame();
		//shut off the game
		//Time.timeScale = 0.0f;
	}
    
    void PlayAudio(AudioClip Clip, bool bLooping=false)
    {
        audioPlayer.clip = Clip;
        audioPlayer.loop = bLooping;
        audioPlayer.Play();
//      Debug.Log("Playing " + Clip.name);
    }
	
	public int Score
	{
		get 
		{ 
			return score; 
		}
		set 
		{ 
			score = value;
//			guiManager.RenderScore();
		}
	}
	
	public int Coin
	{
		get 
		{ 
			return coin; 
		}
		set 
		{ 
			coin = value;
//			guiManager.RenderCoin();
		}
	}
	
    private IEnumerator PauseTime(float seconds)
    {
        Debug.Log("We are in Pause Time Function");
        yield return new WaitForSeconds(seconds);
        Debug.Log("Waited");
		Time.timeScale = 1.0f;
    }
}
