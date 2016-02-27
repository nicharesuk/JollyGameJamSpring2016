using UnityEngine;
using System.Collections;

public class Pacdot : MonoBehaviour {
	GameManager GM;


	void Start()
	{
		GM = GameObject.Find("Game Manager").GetComponent<GameManager>();
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if(other.name == "pacman")
		{
			if(GameManager.bChaser)
			{
				GameManager.p1_score += 10;
			}
		    GameObject[] pacdots = GameObject.FindGameObjectsWithTag("pacdot");
            Destroy(gameObject);

		    if (pacdots.Length == 1)
		    {
		        GameObject.FindObjectOfType<GameGUINavigation>().LoadLevel();
		    }
		}
		if(other.name == "pacman_6")
		{
			if(!GameManager.bChaser)
			{
				GameManager.p2_score += 10;
			}
			GameObject[] pacdots = GameObject.FindGameObjectsWithTag("pacdot");
			Destroy(gameObject);

			if (pacdots.Length == 1)
			{
				GameObject.FindObjectOfType<GameGUINavigation>().LoadLevel();
			}
		}
	}
}
