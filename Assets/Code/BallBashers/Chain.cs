using UnityEngine;
using System.Collections;

public class Chain : MonoBehaviour 
{
	public LineRenderer line;
	public GameObject player;
	public Material material;
	public static float dist;
	public float scaleY;

	public Chain chainScript;

	void Start () 
	{
		line = gameObject.GetComponent<LineRenderer>();
		player = GameObject.FindGameObjectWithTag("Player");
	}

	void LateUpdate()
	{
		UpdateChain();

		line.SetPosition(1, (player.transform.position) + new Vector3(0, 1, 0));
		Chain.dist = Vector3.Distance(transform.position, player.transform.position);

		material.mainTextureScale = new Vector2(dist, 1);
	}

	public void UpdateChain()
	{
		line.SetPosition(0, transform.position);
	}
}


