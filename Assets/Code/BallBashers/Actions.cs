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
	bool jumped = false;


	//rolling variables
	public float rollSpeed = 20;
	public bool rolling = true;
	bool canRoll = true;
	bool isRolling = false;
	bool rolled = false;

	//aiming/shooting variables
	bool canAim;
	bool canFire;
	int firing = 0;
	public bool aiming = false;
	bool aimed = false;
	bool isAiming = false;
	public bool grenading = true;
	
	bool canGrenade = true;
}
