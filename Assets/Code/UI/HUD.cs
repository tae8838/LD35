using UnityEngine;
using System.Collections;
using UnityEditor.VersionControl;

public class HUD : MonoBehaviour {

	public Sprite heartSprite;
	public Sprite heartEmptySprite;
	public GameObject[] hearts;

	//find out how many hearts are needed from difficulty level, create that many hearts
	void SetupNumberOfHearts(){
	}

	//change one of the hearts to a full heart
	void AddHealth(){
	}

	//change a heart to an empty heart image
	void SubtractHealth(){

	}

}
