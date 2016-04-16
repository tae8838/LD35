using UnityEngine;
using System.Collections;

public class CoinDisappear : MonoBehaviour {

	float spinSpeed = 300f;
	
	void LateUpdate()
	{
		transform.Rotate(Vector3.up * Time.deltaTime * spinSpeed, Space.World);
		transform.localScale += new Vector3(-0.05F, -0.05F, -0.05F);
		if (transform.localScale.z <= 0)
			Destroy(gameObject);
	}
	
}
