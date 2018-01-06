using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour {
	public Ball ball;
	public float ReflectDirection;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject == ball.gameObject) {
			ball.ForceTo = new Vector2 (Mathf.Abs(ball.ForceTo.x) * ReflectDirection, ball.ForceTo.y);
		}
	}
}
