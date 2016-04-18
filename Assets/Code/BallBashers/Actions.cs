using UnityEngine;
using System.Collections;

public class Actions : MonoBehaviour 
{
	public bool walking = false;

	//action variables
	bool canReload;
	public bool reloading = false;
	bool canItem;
	public bool item = true;
	bool canSignal;
	public bool signal = true;
	bool canAbility;
	public bool ability = true;
	bool canMelee;
	public bool melee = false;

	//jumping variables
	public float jumpSpeed = 8;
	public float gravity = -9;
	bool canJump;
	public bool isJumping;


	//rolling variables
	public float rollSpeed = 20;
	public bool rolling = true;

}
