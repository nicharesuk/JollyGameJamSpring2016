using System;
using UnityEngine;
using System.Collections;

public class Player2Controller : MonoBehaviour
{

	public float speed = 0.4f;
	public Vector2 _dest = Vector2.zero;
	Vector2 _dir = Vector2.zero;
	Vector2 _nextDir = Vector2.zero;

	public KeyCode right = KeyCode.D;
	public KeyCode left = KeyCode.A;
	public KeyCode up = KeyCode.W;
	public KeyCode down = KeyCode.S;

	bool arrows = false;

	[Serializable]
	public class PointSprites
	{
		public GameObject[] pointSprites;
	}

	public PointSprites points;

	public static int killstreak = 0;

	// script handles
	private GameGUINavigation GUINav;
	private GameManager GM;
	private ScoreManager SM;

	private bool _deadPlaying = false;

	// Use this for initialization
	void Start()
	{
		GM = GameObject.Find("Game Manager").GetComponent<GameManager>();
		SM = GameObject.Find("Game Manager").GetComponent<ScoreManager>();
		GUINav = GameObject.Find("UI Manager").GetComponent<GameGUINavigation>();
		_dest = transform.position;
	}

	// Update is called once per frame
	void FixedUpdate()
	{
		switch (GameManager.gameState)
		{
		case GameManager.GameState.Game:
			ReadInputAndMove();
			Animate();
			break;

		case GameManager.GameState.Dead:
			if (!_deadPlaying)
				StartCoroutine("PlayDeadAnimation");
			break;
		}


	}

	//TODO Just switch positions
	public void controlSwitch() {

		GameObject p1 = GameObject.Find("pacman");

		Vector2 temp = transform.position;
		transform.position = p1.GetComponent<Transform>().position;
		p1.GetComponent<Transform>().position = temp;

		this._dest = transform.position;
		p1.GetComponent<PlayerController>()._dest = p1.GetComponent<Transform>().position;

//		if (arrows) {
//			arrows = false;
//			right = KeyCode.D;
//			left = KeyCode.A;
//			up = KeyCode.W;
//			down = KeyCode.S;
//		} 
//		else {
//			arrows = true;
//			right = KeyCode.RightArrow;
//			left = KeyCode.LeftArrow;
//			up = KeyCode.UpArrow;
//			down = KeyCode.DownArrow;
//		}
	}

	public void invertControlsPwrUp() {
		KeyCode temp = left;
		left = right;
		right = temp;

		temp = up;
		up = down;
		down = temp;
	}

	IEnumerator PlayDeadAnimation()
	{
		_deadPlaying = true;
		GetComponent<Animator>().SetBool("Die", true);
		yield return new WaitForSeconds(1);
		GetComponent<Animator>().SetBool("Die", false);
		_deadPlaying = false;

		if (GameManager.lives <= 0)
		{
			Debug.Log("Treshold for High Score: " + SM.LowestHigh());
			//if (GameManager.score >= SM.LowestHigh())
			//	GUINav.getScoresMenu();
			//else
			//ff	GUINav.H_ShowGameOverScreen();
		}

		else
			GM.ResetScene();
	}

	void Animate()
	{
		Vector2 dir = _dest - (Vector2)transform.position;
		GetComponent<Animator>().SetFloat("DirX", dir.x);
		GetComponent<Animator>().SetFloat("DirY", dir.y);
	}

	bool Valid(Vector2 direction)
	{
		// cast line from 'next to pacman' to pacman
		// not from directly the center of next tile but just a little further from center of next tile
		Vector2 pos = transform.position;
		direction += new Vector2(direction.x * 0.45f, direction.y * 0.45f);
		RaycastHit2D hit = Physics2D.Linecast(pos + direction, pos);
		return hit.collider.name == "pacdot" || (hit.collider == GetComponent<Collider2D>());
	}

	public void ResetDestination()
	{
		_dest = new Vector2(15f, 11f);
		GetComponent<Animator>().SetFloat("DirX", 1);
		GetComponent<Animator>().SetFloat("DirY", 0);
	}

	void ReadInputAndMove()
	{
		// move closer to destination
		Vector2 p = Vector2.MoveTowards(transform.position, _dest, speed);
		GetComponent<Rigidbody2D>().MovePosition(p);

		// get the next direction from keyboard
		if (Input.GetKeyDown(right)) _nextDir = Vector2.right;
		if (Input.GetKeyDown(left)) _nextDir = -Vector2.right;
		if (Input.GetKeyDown(up)) _nextDir = Vector2.up;
		if (Input.GetKeyDown(down)) _nextDir = -Vector2.up;

		// if pacman is in the center of a tile
		if (Vector2.Distance(_dest, transform.position) < 0.00001f)
		{
			if (Valid(_nextDir))
			{
				_dest = (Vector2)transform.position + _nextDir;
				_dir = _nextDir;
			}
			else   // if next direction is not valid
			{
				if (Valid(_dir))  // and the prev. direction is valid
					_dest = (Vector2)transform.position + _dir;   // continue on that direction

				// otherwise, do nothing
			}
		}
	}

	public Vector2 getDir()
	{
		return _dir;
	}

	public void UpdateScore()
	{
		killstreak++;

		// limit killstreak at 4
		if (killstreak > 4) killstreak = 4;

		Instantiate(points.pointSprites[killstreak - 1], transform.position, Quaternion.identity);
		GameManager.p2_score += (int)Mathf.Pow(2, killstreak) * 100;

	}

	void OnTriggerEnter2D(Collider2D col) {
		//Steal points
		if(col.name == "pacman" && GameManager.bChaser) {
			GameManager.p2_score += 300;
			GameManager.p1_score -= 300;

			Vector2 curPos = col.GetComponent<Transform>().transform.position;
			Vector2 max = getMax(curPos);
			col.GetComponent<Transform>().transform.position = max;
			col.GetComponent<PlayerController>()._dest = max;
		}
	}


	Vector2 getMax(Vector2 curPos)
	{
		Vector2 max = GM.deathPoints[0];
		foreach (Vector2 v in GM.deathPoints)
		{
			if(Vector2.Distance(v, curPos) > Vector2.Distance(curPos,max))
			{
				max = v;
			}
		}
		return max;
	}
}
