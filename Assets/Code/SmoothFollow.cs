using UnityEngine;

public class SmoothFollow : MonoBehaviour
{
	#region Consts
	private const float SMOOTH_TIME = 0.3f;
	#endregion

	#region Public Properties
	public bool LockX;
	public float offSetX;
	public float offSetY = 18;
	public float offSetZ;
	public bool LockY;
	public bool LockZ;
	public bool useSmoothing;
	public Transform target;
	#endregion

	#region Private Properties
	private Transform thisTransform;
	private Vector3 velocity;
	#endregion

	private void Awake()
	{
		thisTransform = transform;

		velocity = new Vector3(0.5f, 0.5f, 0.5f);
	}

	void Update()
	{
		transform.position = new Vector3(target.position.x, 13f, target.position.z);
	}

}
