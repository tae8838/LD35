using UnityEngine;
using System.Collections;
using UnityEditor.VersionControl;
using UnityEngine.UI;

public class HUD : MonoBehaviour {

	public Sprite heartSprite;
	public Sprite heartEmptySprite;
	public GameObject[] hearts;
	public Text score;
	public Text combo;
	public Player player;
	int MAX_HEALTH = 3;

	void Start(){
	}

	void Update(){
		UpdateScore ();
		if (player.addHealth) {
			AddHealth ();
		}
		if (player.decreaseHealth) {
			SubtractHealth ();
		}
	}
	//find out how many hearts are needed from difficulty level, create that many hearts
	void SetupNumberOfHearts(){
	}

	//change one of the hearts to a full heart
	void AddHealth(){
		hearts[player.health - 1].GetComponent<Image> ().sprite = heartSprite;
		player.addHealth = false;
	}

	//change a heart to an empty heart image
	void SubtractHealth(){
		hearts[player.health].GetComponent<Image> ().sprite = heartEmptySprite;
		player.decreaseHealth = false;
	}

	void UpdateScore(){
		score.text = "SCORE: " + player.score;
		combo.text = "COMBO: " + player.combo;
	}
}
