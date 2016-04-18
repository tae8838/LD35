using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Events;

public class Gameover : MonoBehaviour {

	public Player player;
	public Text endScoreText;
	public Game game;
	public GameObject panel;
	bool wait;

	void Start(){
		endScoreText.text = "SCORE: " + player.score;
		game = GameObject.Find("Main Camera").GetComponent<Game>();
		StartCoroutine(_WaitForInput());
	}

	void Update(){
		if (wait){
			if (Input.GetKey(KeyCode.Z) || Input.GetButton("Fire1")){
				game.Restart();
			} else if (Input.GetKey(KeyCode.X) || Input.GetButton("Fire3")){
				game.Restart();
			} else if (Input.GetKey(KeyCode.C) || Input.GetButton("Fire0")){
				game.Restart();
			} else if (Input.GetKey(KeyCode.V) || Input.GetButton("Fire2")){
				game.Restart();
			}
		}
	}

	IEnumerator _WaitForInput(){
		yield return new WaitForSeconds(1.5f);
		wait = true;
		panel.SetActive(true);
	}
}
