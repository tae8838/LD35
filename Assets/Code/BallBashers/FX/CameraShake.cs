using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour {

	public static CameraShake camShake;


	// Use this for initialization


	private Vector3 originPosition;
	private Quaternion originRotation;
	public float shake_decay;
	public float shake_intensity;
	
	void Update (){
		if (shake_intensity > 0){
			transform.position = originPosition + Random.insideUnitSphere * .3f;
//			transform.rotation = new Quaternion(
//				originRotation.x + Random.Range (-shake_intensity,shake_intensity) * .2f,
//				originRotation.y + Random.Range (-shake_intensity,shake_intensity) * .2f,
//				originRotation.z + Random.Range (-shake_intensity,shake_intensity) * .2f,
//				originRotation.w + Random.Range (-shake_intensity,shake_intensity) * .2f);
			shake_intensity -= shake_decay;
		}
	}
	
	public void ShakeCam(float shakeamount){
		originPosition = transform.position;
		originRotation = transform.rotation;
		shake_intensity = shakeamount;
		shake_decay = 0.04f;
	}
}
