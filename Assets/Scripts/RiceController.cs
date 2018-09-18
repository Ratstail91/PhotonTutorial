using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace Com.KRGameStudios.PhotonTutorial {
	public class RiceController : MonoBehaviourPun {
		//static members
		public static GameObject staticRice;

		//components
		Animator animator;
		Rigidbody2D rigidBody;

		//constants
		const float moveForce = 10f;
		const float jumpForce = 160f;
		const float maxSpeed = 2.5f;
		const float fallSpeed = 1f;

		//movements
		bool jumping = false;
		bool grounded = false;
		float horizontalInput = 0f;

		void Awake() {
			if (photonView.IsMine) {
				staticRice = this.gameObject;
			}

			DontDestroyOnLoad(this.gameObject);
		}

		void Start() {
			animator = GetComponent<Animator>();
			rigidBody = GetComponent<Rigidbody2D>();
		}

		void Update() {
			if (photonView.IsMine == false && PhotonNetwork.IsConnected == true) {
				return;
			}

			if (photonView.IsMine) {
				HandleInput();
			}
			SendAnimationInfo();
		}

		void FixedUpdate() {
			HandleMovement();
		}

		void HandleInput() {
			//determine if on the ground
			grounded  = Physics2D.Linecast(transform.position, transform.position + new Vector3( 0.16f, -0.24f, 0), 1 << LayerMask.NameToLayer("Ground"));
			grounded |= Physics2D.Linecast(transform.position, transform.position + new Vector3(-0.16f, -0.24f, 0), 1 << LayerMask.NameToLayer("Ground"));

			//determine if jumping
			if (Input.GetButtonDown("Jump") && grounded) {
				jumping = true;
			}

			//get horizontal speed, potentially negative
			horizontalInput = Input.GetAxis ("Horizontal");
		}

		void HandleMovement() {
			//move in the inputted direction, if not at max speed
			if (horizontalInput * rigidBody.velocity.x < maxSpeed) {
				rigidBody.AddForce (Vector2.right * horizontalInput * moveForce);
			}

			//slow the player down when it's travelling too fast
			if (Mathf.Abs (rigidBody.velocity.x) > maxSpeed) {
				rigidBody.velocity = new Vector2 (Mathf.Sign (rigidBody.velocity.x) * maxSpeed, rigidBody.velocity.y);
			}

			if (Mathf.Abs (rigidBody.velocity.y) > maxSpeed) {
				rigidBody.velocity = new Vector2 (rigidBody.velocity.x, Mathf.Sign (rigidBody.velocity.y) * maxSpeed);
			}

			//jump up
			if (jumping) {
				rigidBody.AddForce (new Vector2 (0f, jumpForce));
				jumping = false;
			}
		}

		void SendAnimationInfo() {
			animator.SetFloat("xSpeed", rigidBody.velocity.x);
		}

		void OnDrawGizmos() {
			Gizmos.color = Color.red;
			Gizmos.DrawLine(transform.position, transform.position + new Vector3( 0.16f, -0.24f, 0));
			Gizmos.DrawLine(transform.position, transform.position + new Vector3(-0.16f, -0.24f, 0));
		}
	}
}