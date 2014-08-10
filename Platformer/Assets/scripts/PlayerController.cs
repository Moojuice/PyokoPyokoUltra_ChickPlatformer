using UnityEngine;
using System.Collections;

[RequireComponent(typeof(PlayerPhysics))]
public class PlayerController : Entity
{

	//Constants
	public float gravity = 20;
	public float walkSpeed = 8;
	public float runSpeed = 12;
	public float acceleration = 30;
	public float jumpHeight = 12;
	public float slideDecceleration = 10;

	private float initiateSlideThreshold = .01f;

	public bool facingRight; 
	//System
	private float animationSpeed;
	private float currentSpeed;
	private float targetSpeed;
	private Vector2 amountToMove;

	//States
	private bool jumping;
	private bool sliding;
	private bool stopSliding;

	//Components
	private PlayerPhysics playerPhysics;
	private Animator animator;
	private GameManager manager;

	// Use this for initialization
	void Start ()
	{
		playerPhysics = GetComponent<PlayerPhysics> ();
		animator = GetComponent<Animator> ();
		manager = Camera.main.GetComponent<GameManager>();
		facingRight = true;
	}
	
	// Update is called once per frame
	void Update ()
	{
	if (dead) {
		animator.SetBool ("Dead", true);
	} else {
		//Reset acceleration upon collision
		if (playerPhysics.movementStopped) {
			targetSpeed = 0;
			currentSpeed = 0;
		}

		//If player is touching the ground
		if (playerPhysics.grounded) {
			amountToMove.y = 0;

			if (jumping) {
				jumping = false;
				animator.SetBool ("Jumping", false);
			}
			if (sliding) {
				if (Mathf.Abs (currentSpeed) < .25f || stopSliding) {
					stopSliding = false;
					sliding = false;
					animator.SetBool ("Sliding", false);
					playerPhysics.ResetCollider ();
				}
			}

			// jump
			if (Input.GetButtonDown ("Jump")) {
				if (sliding) {
					stopSliding = true;
				}
				else {
					amountToMove.y = jumpHeight;
					jumping = true;
					animator.SetBool ("Jumping", true);
				}
			}

			// sliding
			if (Input.GetButtonDown ("Slide")) {
				if (Mathf.Abs (currentSpeed) > initiateSlideThreshold) {
					sliding = true;
					animator.SetBool ("Sliding", true);
					targetSpeed = 0;
		
					playerPhysics.SetCollider (new Vector3 (.13f, .1f, 1), new Vector3 (0, -.03f, 0));
				}
			}
		}

		//Set animation parameters
		animationSpeed = IncrementTowards (animationSpeed, Mathf.Abs (targetSpeed), acceleration);
		animator.SetFloat ("Speed", Mathf.Abs (currentSpeed));

		//input
		if (!sliding) {
			float speed = (Input.GetButton ("Run") ? runSpeed : walkSpeed);
			targetSpeed = Input.GetAxisRaw ("Horizontal") * speed;
			currentSpeed = IncrementTowards (currentSpeed, targetSpeed, acceleration);

			//face direction
			float moveDir = Input.GetAxisRaw ("Horizontal");

			if (moveDir != 0) {
				if (moveDir < 0) {
					if (facingRight) {
						facingRight = false;
					}
				}
				else {
					if (!facingRight) {
						facingRight = true;
					}
				}
				transform.eulerAngles = (moveDir < 0) ? Vector3.up * 180 : Vector3.zero;	
			}
		} else {
			currentSpeed = IncrementTowards (currentSpeed, targetSpeed, slideDecceleration);
		}

		//Set amount to move
		amountToMove.x = currentSpeed;
		amountToMove.y -= gravity * Time.deltaTime; 
		playerPhysics.Move (amountToMove * Time.deltaTime);

		}
	}
	
	void OnTriggerEnter(Collider c) {
		if (c.tag == "Checkpoint") {
			manager.SetCheckpoint(c.transform.position);
		}
		if (c.tag == "Finish") {
			manager.EndLevel();
		}
	}
	
	// Increase n towards a target by speed 
	private float IncrementTowards (float n, float target, float a)
	{
		if (n == target) { //if current speed == target, return speed
			return n;
		} else {
			float dir = Mathf.Sign (target - n); //must n be incremented or decremented to get close to target 
			n += a * Time.deltaTime * dir;
			return (dir == Mathf.Sign (target - n)) ? n : target; //if n has now passed target then return target, otherwise return n 
		}
	}
}
