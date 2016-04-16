using UnityEngine;
using System.Collections;

public class CameraGroup : MonoBehaviour {
	GameObject[] players;
	Camera camera;
	Transform middlepoint;
	Vector3 target;
	float offsetY;
	float offsetZ;
	float distance = 5f;
	public float minDistance = 6f;
	public float maxDistance = 14f;
	public float edgeAmount = 2f;
	public float ratio = 1.5f;
	public float transformSmoothing = 1f;
	
	void Start()
	{
		camera = gameObject.GetComponent<Camera>();
		players = GameObject.FindGameObjectsWithTag("Player");
		
		if (GameObject.FindWithTag("Player"))
		{
			for(int i = 0; i < players.Length; i++)
			{
				Debug.Log("Player Number "+i+" is named "+players[i].name);
			}
		}
		else
			Debug.Log("No players found!");
	}
	
	void LateUpdate() 
	{
		
		// calculate midpoint between all the player positions
		var length = players.Length;
		var sum = new Vector3();
		for (var i = 0; i < length ; i++) {
			sum += players[i].transform.position;
		}
		var midpoint = sum / length;
		
		// calculate maximum distance between each player and midpoint
		var distance = minDistance * minDistance; // must be >= this minimum
		
		for (var i = 0; i < length; i++) {
			// using squared distances to avoid costly square root calculations
			var sqrDist = (players[i].transform.position - midpoint).sqrMagnitude;
			if (sqrDist > distance) {
				distance = sqrDist;
			}
		}
		
		if (distance > maxDistance * maxDistance) {
			distance = maxDistance; // must be <= this maximum
		} else {
			distance = Mathf.Sqrt(distance); // back to regular non-squared distance
		}
		
		// orient camera to point at midpoint
		camera.transform.LookAt(midpoint);
		
		// update camera position so it is near midpoint but away from field
		offsetY = distance * ratio + edgeAmount;
		offsetZ = distance * ratio + edgeAmount;
		
		target.x = midpoint.x;
		target.y = midpoint.y + offsetY;
		target.z = midpoint.z + offsetZ;
		
		camera.transform.position = Vector3.Lerp(transform.position,
		                                         target,
		                                         transformSmoothing * Time.deltaTime);
	}
}	