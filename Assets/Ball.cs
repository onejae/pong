using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour {
	public float speed;
	public float minSpeed = 0.1f;
	public Vector2 ForceTo { get; set; }
	public AudioSource ping;
	public AudioSource pong;
	public AudioSource bound;

	// Use this for initialization
	void Start () {
		
	}
	
	void FixedUpdate () {
		Vector3 currentPosition = transform.localPosition;
		transform.Translate (ForceTo * speed);
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.transform.name == "Player")
			ping.Play ();
		else if (other.transform.name == "Computer")
			pong.Play ();
		else if (other.transform.name == "RightWall" || other.transform.name == "LeftWall")
			bound.Play ();
	}
}
