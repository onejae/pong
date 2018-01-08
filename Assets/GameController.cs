using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {
	public Ball ball;
	public TextMesh ComputerScore;
	public TextMesh PlayerScore;
	public TextMesh SignText;
	public GameObject GuideText;
	public GameObject AdsText;
	public Bar computer;
	public Bar player;
	private static int GoalScore = 3;
	enum GameState{init, ready, playing};
	private GameState gameState = GameState.init;
	private static float moveArea = 2.72f;
	public AudioSource missAudio;
	public AudioSource pointAudio;
	public AudioSource winAudio;
	public AudioSource touchAudio;
	public AdsController adsController;
	public TextMesh GoalText;
	public TextMesh goalDown;
	public TextMesh goalUp;
	public GameObject GoalPanel;
	public AudioSource touch2Audio;

	// level
	private static int Level = 2;	// 1 : easy , 2 : normal, 3: crazy
	public TextMesh LevelText;
	public TextMesh LevelDown;
	public TextMesh LevelUp;
	public GameObject LevelPanel;

	// auto
	private static bool Auto = false;
	public TextMesh AutoText;
	public TextMesh AutoDown;
	public TextMesh AutoUp;
	public GameObject AutoPanel;

	// Use this for initialization
	void Start () {
		Init ();
	}

	private void SetLevel(int level)
	{
		if (Level == 1) {
			LevelText.text = "EASY";
			player.maxSpeed = 7.0f;
			computer.maxSpeed = 7.0f;
			player.myShadowBall.minSpeed = 0.12f;
			computer.myShadowBall.minSpeed = 0.12f;
		} else if (Level == 2) {
			LevelText.text = "NORMAL";
			player.maxSpeed = 7.0f;
			computer.maxSpeed = 7.0f;
			player.myShadowBall.minSpeed = 0.2f;
			computer.myShadowBall.minSpeed = 0.2f;
		} else {
			LevelText.text = "HARD";
			player.maxSpeed = 7.0f;
			computer.maxSpeed = 7.0f;
			player.myShadowBall.minSpeed = 0.5f;
			computer.myShadowBall.minSpeed = 0.5f;
		}

		computer.Level = level;
		player.Level = level;
	}
	
	// Update is called once per frame
	void Update () {
		if (gameState == GameState.playing && player.auto == false) {
			Vector3 movePos = Vector3.zero;
			Vector3 v3 = Vector3.zero;

			if (Input.touchCount > 0)
				v3 = Input.GetTouch (0).position;
			else
				v3 = Input.mousePosition;
			
			v3.z = 10.0f;
			movePos = Camera.main.ScreenToWorldPoint (v3);

			if (Mathf.Abs (movePos.x) > moveArea) {
				float direction = movePos.x > 0 ? 1f : -1f;
				player.transform.localPosition = new Vector2 (moveArea * direction, player.transform.localPosition.y);
			} else {
				player.transform.localPosition = new Vector2 (movePos.x, player.transform.localPosition.y);
			}
		} else if (gameState == GameState.init || gameState == GameState.ready) {
//			if (Input.touchCount > 0 && Input.GetTouch (0).phase == TouchPhase.Began) {
//			Vector3 v3 = Input.GetTouch (0).position;

			if (Input.GetMouseButtonUp(0) == true){
				Vector3 v3 = Input.mousePosition;
				v3.z = 10.0f;
				Vector3 touchWorldPos = Camera.main.ScreenToWorldPoint (v3);
				Vector2 touchPosWorld2D = new Vector2 (touchWorldPos.x, touchWorldPos.y);

				RaycastHit2D hitInformation = Physics2D.Raycast (touchPosWorld2D, Camera.main.transform.forward);
				if (hitInformation.collider != null) {
					//We should have hit something with a 2D Physics collider!
					GameObject touchedObject = hitInformation.transform.gameObject;
					//touchedObject should be the object someone touched.

					string name = touchedObject.transform.name;

					if (name == "AdsTrigger")
						adsController.ShowAd ();
					else if (name == "goalUp") {
						if (GoalScore < 7) {
							GoalScore += 2;
						} else {
							GoalScore = 1;
						}
						GoalText.text = GoalScore.ToString ();
						touch2Audio.Play ();
					} else if (name == "goalDown") {
						if (GoalScore > 1) {
							GoalScore -= 2;
						} else {
							GoalScore = 7;
						}
						GoalText.text = GoalScore.ToString ();
						touch2Audio.Play ();
					} else if (name == "levelUp") {
						if (Level != 3)
							Level++;
						else
							Level = 1;

						SetLevel (Level);
						touch2Audio.Play ();
					} else if (name == "levelDown") {
						if (Level != 1)
							Level--;
						else
							Level = 3;
						
						SetLevel (Level);
						touch2Audio.Play ();
					} else if (name == "autoUp" || name == "autoDown") {
						Auto = !Auto;
						AutoText.text = Auto ? "AUTO" : "NORMAL";
						player.auto = Auto;
						touch2Audio.Play ();
					} else {
						if (gameState == GameState.init && touchPosWorld2D.y >= -1.4f)
							StartGame ();
						else if (gameState == GameState.ready)
							StartGame ();
					}
				}
			}
		}
			
		// game scoring
		Vector2 ballPosition = ball.transform.localPosition;

		if (Mathf.Abs(ballPosition.x) > 3.4f) {
			// out of screen, the last hitter loses
			int winner = computer.hitCount < player.hitCount ? 1 : 0;

			if (Score (winner)) {
				Win (winner);
			} else {
				SetBall ();
			}
		}
		else if (ballPosition.y > computer.transform.localPosition.y + 0.2f) {
			if (Score (0)) {
				Win (0);
			} else {
				SetBall ();
			}
		} else if (ballPosition.y < player.transform.localPosition.y - 0.2f) {
			if (Score (1)) {
				Win (1);
			} else {
				SetBall ();
			}
		}
	}

	private void Init()
	{
		SignText.text = "Pong";
		SetLevel (2);

		ComputerScore.transform.gameObject.SetActive (false);
		PlayerScore.transform.gameObject.SetActive (false);
	}

	private void Win(int winner)
	{
		adsController.LoadAd ();
		SetBall ();

		if (winner == 0) {
			SignText.text = "YOU WIN";
		}
		else
			SignText.text = "YOU LOSE";
		
		ComputerScore.text = "0";
		PlayerScore.text = "0";

		ComputerScore.gameObject.SetActive (false);
		PlayerScore.gameObject.SetActive (false);
		SignText.transform.gameObject.SetActive (true);
		GuideText.SetActive (true);
		AdsText.SetActive (true);
		GoalPanel.SetActive (true);
		LevelPanel.SetActive (true);
		AutoPanel.SetActive (true);
		gameState = GameState.init;
	}

	private void SetBall()
	{
		ball.Speed = ball.minSpeed;
		ball.transform.localPosition = new Vector2 (0.0f, 0.0f);
		ball.ForceTo = new Vector2 (0.0f, 0.0f);

		player.myShadowBall.ForceTo = ball.ForceTo;
		player.myShadowBall.transform.localPosition = ball.transform.localPosition;
		computer.myShadowBall.ForceTo = ball.ForceTo;
		computer.myShadowBall.transform.localPosition = ball.transform.localPosition;

		player.transform.localPosition = new Vector2 (0.0f, player.transform.localPosition.y);

		gameState = GameState.ready;
	}

	private bool Score(int who)
	{
		int currentScore = 0;

		if (who == 0) {
			// player gets a point
			currentScore = int.Parse (PlayerScore.text);
			PlayerScore.text = (currentScore + 1).ToString ();

			if (currentScore + 1 >= GoalScore) {
				winAudio.Play ();
			} else {
				pointAudio.Play ();
			}
		} else {
			// computer
			currentScore = int.Parse (ComputerScore.text);
			ComputerScore.text = (currentScore + 1).ToString ();
			missAudio.Play ();
		}

		return currentScore + 1 >= GoalScore ? true : false;
	}

	public void StartGame() {
		touchAudio.Play ();
		ball.ForceTo = new Vector2 (0.0f, 1.0f);

		PlayerScore.transform.gameObject.SetActive (true);
		ComputerScore.transform.gameObject.SetActive (true);
		SignText.transform.gameObject.SetActive (false);
		GuideText.SetActive (false);
		AdsText.SetActive (false);

		gameState = GameState.playing;

		computer.hitCount = 0;
		player.hitCount = 0;

		GoalPanel.SetActive (false);
		LevelPanel.SetActive (false);
		AutoPanel.SetActive (false);
	}
}
