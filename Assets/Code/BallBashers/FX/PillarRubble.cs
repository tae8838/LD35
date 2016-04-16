using UnityEngine;
using System.Collections;

public class PillarRubble : MonoBehaviour {
	
	void Start () 
	{
		Quaternion rotation = Quaternion.Euler(new Vector3(-90, Random.Range(0f,360f), 0));
		transform.rotation = rotation;
		float x = Random.Range(3f, 3.75f);
		transform.localScale = new Vector3(x,x,x);
	}
}
