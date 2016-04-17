using UnityEngine;
using System.Collections;

public class MusicPlayer : MonoBehaviour {

	public AudioClip[] clips;
	AudioSource audioSource;

	void Start(){
		audioSource = this.GetComponent<AudioSource>();
		int tracks = clips.Length;
		int playMusic = Random.Range(0, tracks);
		audioSource.clip = clips[playMusic];
		audioSource.Play();
	}
}
