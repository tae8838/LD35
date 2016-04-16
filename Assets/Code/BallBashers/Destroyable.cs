using UnityEngine;
using System.Collections;

public class Destroyable : MonoBehaviour {
	
	public int scoreAmount = 10;
	public int health = 80;
	public int minimumDamageAmount = 60;
	public float damagedStatePercentage = 0.5f;
	public int damagedStageAmount;
	public float pickupDropRate = 0.7f;
	private Vector3 transformpositionzero;
	
	public GameObject damageFXPrefab;
	public GameObject destroyFXPrefab;
	public GameObject pickupPrefab;
	
	public Mesh damagedMesh;
	public Material damagedMaterial;
	public Animator animator;
	
//	public GameObject undamagedStatePrefab;
//	public GameObject damagedStatePrefab;
//	public GameObject destroyedStatePrefab;
	
	public AudioClip damageAudio;
	//moved to destroyFXPrefab
	//public AudioClip destroyAudio;
	
	public AudioSource audioPlayer;
	
	public enum DamageState 
	{
		Undamaged = 2,
		Damaged = 1,
		Destroyed = 0,
	}
	
	public DamageState damageState = DamageState.Undamaged;
	
	void Awake()
	{
		damagedStageAmount = (int)(health * damagedStatePercentage);
//		Debug.Log("DamageStateAmount = " +damagedStageAmount);
	}
	
	protected virtual void Update()
	{
		if (health < 0)
		{
			Die();
//				Debug.Log("Entity " + gameObject + " dies");
		}
	}

	public virtual void Start()
	{
		//animator = GetComponent<Animator>();
	}
	
	public bool TakeDamage(int damage)
	{
		return ApplyDamage(damage);
	}

	public virtual void TakePain()
	{
		if (animator != null)
			animator.SetTrigger("Damage");
	}

	protected virtual bool ApplyDamage(int damage) 
	{

		if (damageState == DamageState.Destroyed)
		{
			return false;
		}
//		Debug.Log("Entity takes " + damage + " damage");
		if (damage >= minimumDamageAmount)
		{
			health -= damage;
			transformpositionzero = new Vector3(transform.position.x, 0, transform.position.z);
			Instantiate(damageFXPrefab, transformpositionzero, Quaternion.identity);
			PlayAudio(damageAudio, audioPlayer);
			TakePain();

			if (damageState == DamageState.Undamaged && health <= damagedStageAmount)
			{
				damageState = DamageState.Damaged;
				swapDamagedMesh();
				//swap model or renderer with damaged version
//				Debug.Log("Switch model do damage state");
			}
			return true;

		}
		return false;
	}
	
	private void swapDamagedMesh()
	{
		//magic swap to be added
		if (damagedMaterial)
		{
			MeshRenderer mr = gameObject.GetComponent<MeshRenderer>();
			mr.material = damagedMaterial;
		}
		if (damagedMesh)
		{
			MeshFilter mf = gameObject.GetComponent<MeshFilter>();
			mf.mesh = damagedMesh;
		}
	}
	
	protected virtual void Die()
	{
		damageState = DamageState.Destroyed;
		if (pickupPrefab != null)
		{
			if (Random.value < pickupDropRate)
			{
				Instantiate(pickupPrefab, transform.position, Quaternion.identity);
			}
		}
		if (destroyFXPrefab != null)
		{	
			transformpositionzero = new Vector3(transform.position.x, 0, transform.position.z);
			Instantiate(destroyFXPrefab, transformpositionzero, Quaternion.identity);
		}

		/*
		if (destroyAudio != null)
		{
			PlayAudio(destroyAudio, (AudioSource)BallBashersGame.Game.BallAndChain.GetComponent("AudioSource"));
		}
		*/
		//BallBashersGame.Game.DestroyedBlock();
		BallBashersGame.Game.AddScore(scoreAmount);
		Destroy(gameObject);
	}
	
	private void PlayAudio(AudioClip clip, AudioSource source)
    {
        source.clip = clip;
        source.loop = false;
        source.Play();
//      Debug.Log("Playing " + Clip.name);
    }
}
