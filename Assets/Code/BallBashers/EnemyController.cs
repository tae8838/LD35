using UnityEngine;
using System.Collections;


[RequireComponent (typeof (Rigidbody))]
[RequireComponent (typeof (CapsuleCollider))]
public class EnemyController : Enemy 
{

	//Components

	public Camera cam;
	
	public int soldiertype = 0;

	//pain death variables
	bool pain = false;
	int death = 0;
	
	public override void Start ()
	{
		base.Start();
		//set the animator component
	}
	
	//function to play a one shot animation 
	public IEnumerator PlayOneShot ( string paramName )
	{
		animator.SetBool( paramName, true );
		yield return null;
		animator.SetBool( paramName, false );
	}

	//function to switch weapons
//	public IEnumerator SwitchWeapon ( string weaponname, int weaponnumber )
//	{	
//		//sets Weapon to 0 first to reset
//		animator.SetInteger( weaponname, 0 );
//		yield return null;
//		yield return null;
//		animator.SetInteger( weaponname, weaponnumber );
//	}
//	
//	//function to play the death anims
//	public IEnumerator SetDeath ( string deathname, int deathnumber )
//	{	
//		//sets Weapon to 0 first to reset
//		animator.SetInteger( deathname, 0 );
//		yield return null;
//		animator.SetInteger( deathname, deathnumber );
//	}
//
//	public override void KnockBack()
//	{
//		base.KnockBack();
//		animator.SetBool("Knockback", true);
//		bloodfx.enableEmission = true;
//	}

	protected override void Update()
	{
		base.Update();


		if(animator)
		{

			//If the character is on the ground
			if (isGrounded)
			{
				//set the animation back to idle
//				animator.SetInteger("Jumping", 0);



				//check if we press jump button
//				if ( canJump && Input.GetButtonDown("Jump"))
				{
					// Apply the current movement to launch velocity
//					rigidbody.velocity += jumpSpeed * Vector3.up;

					//set variables
	//				animator.SetTrigger("Jump");
//					canJump = false;
//					isJumping = true;
				}
			}
			else
			{    
			//set the animation back to idle
//				animator.SetInteger("Jumping", 2);

				//set variables
//				jumped = true;
			}

			//Check if we are grounded

		}
	}

	//plays a random death# animation between 1-3
//	void Death()
//	{
//		//stop character movement
//		animator.SetBool("Moving", false);
//		Input.ResetInputAxes();
//		isMoving = false;
//		
//		int deathnumber = 0;
//		deathnumber = (Random.Range(1, 4));
//		animator.SetInteger("Death",deathnumber);
//	}



	public override void FixedUpdate ()
	{
		base.FixedUpdate();
		//gravity
//		rigidbody.AddForce(0, gravity, 0, ForceMode.Acceleration);

		//Get local velocity of charcter

	}
}
