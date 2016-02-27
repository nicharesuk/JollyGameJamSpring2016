using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIScript : MonoBehaviour {

	public int p2_score, p1_score;

	public List<Image> lives = new List<Image>(3);

	public string chaser;

	Text txt_p1_score, txt_p2_score, txt_chaser, txt_winner;

	public GameManager GM;
	
	// Use this for initialization
	void Start () 
	{
		txt_p1_score = GetComponentsInChildren<Text>()[1];
		txt_p2_score = GetComponentsInChildren<Text>()[0];
        txt_chaser = GetComponentsInChildren<Text>()[2];
		txt_winner = GetComponentsInChildren<Text>()[3];
		GM = GameObject.Find("Game Manager").GetComponent<GameManager>();

	}
	
	// Update is called once per frame
	void Update () 
	{
        // update score text
        p1_score = GameManager.p1_score;
		p2_score = GameManager.p2_score;

		if(!GameManager.bChaser) {
			chaser = "Green";
		}
		else {
			chaser = "Pink";
		}

		txt_p1_score.text = "Green Score\n" + p1_score;
		txt_p2_score.text = "Pink Score\n" + p2_score;
	    txt_chaser.text = "Chaser\n" + chaser;

		int winner = GM.FindWinningPlayer();
		if(GM.timeLeft >= 0){
			if(winner == 1)
			{
				txt_winner.text = "Green is\n Winning " + (int)Mathf.Ceil(GM.timeLeft);
			}
			else if (winner == 2)
			{
				txt_winner.text = "Pink is\n Winning " + (int)Mathf.Ceil(GM.timeLeft);
			}
			else
			{
				txt_winner.text = "";
			}

		}
		else if (GM.timeLeft <= 0)
		{
			txt_winner.text = "";
		}

	}


}
