﻿using UnityEngine;
using System.Collections;
using UnityEditor.VersionControl;
using UnityEngine.UI;
using UnityEngine.Events;

public class Gameover : MonoBehaviour {

	public Player player;
	public Text endScoreText;

	void Start(){
		endScoreText.text = "SCORE: " + player.score;
	}
}
