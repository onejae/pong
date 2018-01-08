using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour {
	private float speed;
	private static float limitX = 3.17f;
	public float Speed {
		get {
			return speed;
		}
		set {
			if (value >= minSpeed)
				speed = value;
			else
				speed = minSpeed;
		}
	}
	public float minSpeed = 0.1f;
	private Vector2 forceTo;
	public Vector2 ForceTo { get; set; }
	public AudioSource ping;
	public AudioSource pong;
	public AudioSource bound;
	public bool IsShadow;
	public bool Moving { get; set; }
	public float shadowBaseY;
	public Bar player;
	public Bar computer;

	// Use this for initialization
	void Start () {
		Moving = true;
		Speed = minSpeed;
		shadowBaseY = transform.localPosition.y;
	}

	void FixedUpdate () {
		if (Moving == false)
			return;
		
		Vector3 v3 = new Vector3 (ForceTo.x, ForceTo.y, 0.0f);
		Vector3 aimTo = transform.localPosition + (v3 * speed);
		Vector3 move = v3 * speed;
		bool addRandomMove = false;

		if (Mathf.Abs (aimTo.x) > limitX) {
			move = (v3 * speed) * ((limitX - Mathf.Abs(transform.localPosition.x)) / Mathf.Abs(aimTo.x - transform.localPosition.x));
		}

		Vector3 origin = transform.localPosition + move;
		if (Mathf.Abs (origin.x) > limitX) {
			origin.x = origin.x < 0.0f ? -limitX : limitX;
		}

//		if (origin.y < -4.3f || origin.y > 5.42f) {
//			if (IsShadow) {
//				move = move * ((Mathf.Abs (shadowBaseY) - Mathf.Abs (transform.localPosition.y)) / Mathf.Abs ((origin.y - transform.localPosition.y)));
//				addRandomMove = true;
//			}
//		}

		if (origin.y < -4.3f) {
			if (IsShadow) {
				move = move * ((Mathf.Abs (-4.3f) - Mathf.Abs (transform.localPosition.y)) / Mathf.Abs ((origin.y - transform.localPosition.y)));
				addRandomMove = true;
			} else {
				Debug.Log ("okay");
//				move = move * ((Mathf.Abs (-4.3f) - Mathf.Abs (transform.localPosition.y)) / Mathf.Abs ((origin.y - transform.localPosition.y)));

			}
		} else if (origin.y > 5.42f) {
			if (IsShadow) {
				move = move * ((Mathf.Abs (5.42f) - Mathf.Abs (transform.localPosition.y)) / Mathf.Abs ((origin.y - transform.localPosition.y)));
				addRandomMove = true;
			} else {
//				move = move * ((Mathf.Abs (5.42f) - Mathf.Abs (transform.localPosition.y)) / Mathf.Abs ((origin.y - transform.localPosition.y)));

				Debug.Log ("okay2");
			}
		}
			
		aimTo = transform.localPosition + move;

		if (addRandomMove == true) {
			aimTo.x = aimTo.x + Random.Range (-0.5f, 0.5f);
			Moving = false;
		}

//		transform.localPosition = aimTo;

		GetComponent<Rigidbody2D> ().MovePosition (new Vector2(aimTo.x, aimTo.y));

		if (Mathf.Abs (aimTo.x) >= limitX) {
			// bounce on the wall
			float reflectDirection = 1.0f;
			if (aimTo.x < 0) {
				reflectDirection = 1.0f;
			} else {
				reflectDirection = -1.0f;
			}

			ForceTo = new Vector2 (Mathf.Abs (ForceTo.x) * reflectDirection, ForceTo.y);
		}
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (IsShadow)
			return;
		
		if (other.transform.name == "Player")
			ping.Play ();
		else if (other.transform.name == "Computer")
			pong.Play ();
		else if (other.transform.name == "RightWall" || other.transform.name == "LeftWall")
			bound.Play ();
	}
}
