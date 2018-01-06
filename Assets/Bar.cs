using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bar : MonoBehaviour {
	public Ball ball;
	public bool auto = false;
	public float maxSpeed;
	private static float moveArea = 2.72f;
	public int hitCount = 0;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

	}

	void FixedUpdate()
	{
		if (auto) {
			float distance = ball.transform.localPosition.x - transform.localPosition.x;
			float moveDistance = distance * Time.fixedDeltaTime * maxSpeed;

			Vector2 moveTo = new Vector2 (moveDistance, 0);

			if (Mathf.Abs (transform.localPosition.x + moveDistance) > moveArea) {
				float direction = transform.localPosition.x > 0 ? 1f : -1f;

				transform.transform.localPosition = new Vector2 (direction * moveArea, transform.localPosition.y);
			} else {
				transform.Translate (moveTo);
			}
		}
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.transform.name == "Ball") {
			float gap = other.transform.localPosition.x - transform.localPosition.x;
			float ballDirection = ball.ForceTo.x > 0f ? 1f : -1f;
			float barDirection = gap > 0f ? 1f : -1f;

			if (ballDirection == barDirection) {
				ball.speed = ball.speed + Mathf.Abs(gap) * 0.05f;
			} else {
				ball.speed = ball.speed - Mathf.Abs(gap) * 0.05f;
			}

			if (ball.speed < ball.minSpeed)
				ball.speed = ball.minSpeed;
			
			ball.ForceTo = new Vector2 (gap * 1.8f, ball.ForceTo.y * -1.0f).normalized;
			hitCount++;
		}
	}
}
