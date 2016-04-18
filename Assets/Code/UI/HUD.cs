using UnityEngine;
using System.Collections;
using UnityEditor.VersionControl;
using UnityEngine.UI;

public class HUD : MonoBehaviour {

	public Sprite heartSprite;
	public Sprite heartEmptySprite;
	public GameObject[] hearts;
	public Text text;
	public Player player;
	int MAX_HEALTH = 3;

	void Start(){
	}

	void Update(){
		UpdateScore ();
		SubtractHealth ();
	}
	//find out how many hearts are needed from difficulty level, create that many hearts
	void SetupNumberOfHearts(){
	}

	//change one of the hearts to a full heart
	void AddHealth(){
	}

	//change a heart to an empty heart image
	void SubtractHealth(){
		if (player.health < MAX_HEALTH && player.health > -1) {
			hearts[player.health].GetComponent<Image> ().sprite = heartEmptySprite;
		}
		
	}

	void UpdateScore(){
		text.text = "SCORE: " + player.score;
	}
}
