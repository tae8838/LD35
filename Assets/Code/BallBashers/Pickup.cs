using UnityEngine;
using System.Collections;

[RequireComponent (typeof(MeshRenderer))]
public class Pickup : MonoBehaviour {
	
	public GameObject CollectedFXPrefab;
	public GameObject RemovedFXPrefab;
	public float LifeSpan = 15f;
	public float WarningTime = 5f;
	public float BlinkInterval = 0.5f;
	
	private MeshRenderer meshRenderer;
	
	[HideInInspector]
	private float spawnTime;
	
	// Use this for initialization
	public virtual void Start ()
	{
	}
	
	void Awake()
	{
		spawnTime = Time.time;
		meshRenderer = GetComponent<MeshRenderer>();
		Invoke ("RemoveMe", LifeSpan);
	}
	
	// Update is called once per frame
	public virtual void Update ()
	{
		float warningTimer = Time.time - spawnTime - LifeSpan + WarningTime;
		if (warningTimer > 0f)
		{
			//voodoo blinking magic! don't ask, Kris
			if (((int)(warningTimer/BlinkInterval))%2 == 0)
			{
				meshRenderer.enabled = true;
			}
			else
			{
				meshRenderer.enabled = false;
			}
		}
		//rotate
	}
	
	public virtual void CollectMe()
	{
		//instantiate pFX
		Instantiate(CollectedFXPrefab, transform.position, transform.rotation);
		Destroy(transform.gameObject);
	}
	
	public virtual void RemoveMe()
	{
		Instantiate(RemovedFXPrefab, transform.position, transform.rotation);
		Destroy(transform.gameObject);
	}
}
