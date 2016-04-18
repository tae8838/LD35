using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HUD : MonoBehaviour {

	public Sprite heartSprite;
	public Sprite heartEmptySprite;
	public GameObject[] hearts;
	public Text score;
	public Text combo;
	public Player player;

	void Update(){
		UpdateScore ();
		if (player.addHealth) {
			AddHealth ();
		}
		if (player.decreaseHealth) {
			SubtractHealth ();
		}
		if (player.dead) {
			gameObject.SetActive (false);
		}
	}

	//change one of the hearts to a full heart
	void AddHealth(){
		hearts[player.health - 1].GetComponent<Image> ().sprite = heartSprite;
		player.addHealth = false;
	}

	//change a heart to an empty heart image
	void SubtractHealth(){
		if (!player.dead) {
			hearts [player.health].GetComponent<Image> ().sprite = heartEmptySprite;
			player.decreaseHealth = false;
		}
	}

	void UpdateScore(){
		score.text = "SCORE: " + player.score;
		combo.text = "COMBO: " + player.combo;
	}

	public void PulseScore(){
		StartCoroutine(_PulseScore());
	}

	IEnumerator _PulseScore(){
		score.rectTransform.localScale = new Vector3(1.25f, 1.25f, 1.25f);
		yield return new WaitForEndOfFrame();
		yield return new WaitForEndOfFrame();
		score.rectTransform.localScale = new Vector3(1f, 1f, 1f);
	}
}
