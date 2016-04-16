using UnityEngine;
using System.Collections;

public class CoinShadow : MonoBehaviour {
	
	public float coinPosY = .1f;
	public Vector3 coinPos;
	public GameObject coin;


	void Start()
	{
		transform.localPosition = new Vector3(0, 0, 0);
	}

	void LateUpdate()
	{
		transform.position = new Vector3(transform.position.x, coinPosY, transform.position.z);
	}

}
