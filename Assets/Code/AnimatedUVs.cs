using UnityEngine;
using System.Collections;

public class AnimatedUVs : MonoBehaviour {
		
	float scrollSpeed = 0.2f;
		
	void Update()
	{
		float offset = Time.time * scrollSpeed;
		GetComponent<Renderer>().material.mainTextureOffset = new Vector2(offset % 1, 0);
	}
}