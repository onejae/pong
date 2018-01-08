using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bar : MonoBehaviour {
	public Ball ball;
	public Ball myShadowBall;
	public Ball otherShadowBall;
	public bool auto = false;
	public float maxSpeed;
	private static float moveArea = 2.72f;
	public int hitCount = 0;
	public int Level = 2;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

	}

	void FixedUpdate()
	{
		if (auto) {
			Ball follower;

			if (transform.localPosition.y > 0f) {
				if (ball.ForceTo.y < 0)
					follower = ball;
				else
					follower = myShadowBall;
			} else {
				if (ball.ForceTo.y > 0)
					follower = ball;
				else
					follower = myShadowBall;
			}
			
			float distance = follower.transform.localPosition.x - transform.localPosition.x;
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
				ball.Speed = ball.Speed + Mathf.Abs(gap) * 0.07f;
			} else {
				ball.Speed = ball.Speed - Mathf.Abs(gap) * 0.04f;
			}
				
			ball.ForceTo = new Vector2 (gap * 1.8f, ball.ForceTo.y * -1.0f).normalized;

//			if (auto == false) {
//			otherShadowBall.Speed = ball.Speed;
			float factor = 1;

			if (Level == 1)
				factor = 1.05f;
			else if (Level == 2)
				factor = 1.2f;
			else
				factor = 1.6f;
			
			otherShadowBall.Speed = ball.Speed * factor;
				otherShadowBall.ForceTo = ball.ForceTo;
				otherShadowBall.transform.localPosition = ball.transform.localPosition;
				otherShadowBall.Moving = true;
//			}
			hitCount++;
		}
	}
}
