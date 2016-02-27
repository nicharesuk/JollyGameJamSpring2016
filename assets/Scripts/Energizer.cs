using UnityEngine;
using System.Collections;

public class Energizer : MonoBehaviour {

    private GameManager gm;
	public string powerup;

	// Use this for initialization
	void Start ()
	{
	    gm = GameObject.Find("Game Manager").GetComponent<GameManager>();
        if( gm == null )    Debug.Log("Energizer did not find Game Manager!");
	}

    void OnTriggerEnter2D(Collider2D col)
    {
		if (powerup == "player_inversion") {
			if (col.name == "pacman" || col.name == "pacman_6") {
				GameObject.Find("pacman").GetComponent<PlayerController>().controlSwitch();
			}
		} 
		else if (powerup == "chase_inversion") {
			if(col.name == "pacman")
			{
				gm.InversionToYellow();
			}
			else if(col.name == "pacman_6" )
			{
				gm.InversionToGreen();
			}
			else
			{
				Debug.Log("It broke.");
			}
		} 
		else if (powerup == "key_inversion") {
			if(col.name == "pacman")
			{
				GameObject.Find("pacman_6").GetComponent<Player2Controller>().invertControlsPwrUp();
			}
			else if (col.name == "pacman_6") {
				GameObject.Find("pacman").GetComponent<PlayerController>().invertControlsPwrUp();
			}
		}
		else if (powerup == "negpwrup") {
			if(col.name == "pacman")
			{
				GameObject.Find("pacman").GetComponent<PlayerController>().invertControlsPwrUp();
			}
			else if (col.name == "pacman_6") {
				GameObject.Find("pacman_6").GetComponent<Player2Controller>().invertControlsPwrUp();
			}
		}

		Vector2 currentPos = transform.position;
		for(int i = 0; i < 4; i++) {
			if (Vector2.Distance(currentPos, PickupGenerator.SpawnPoints[i]) < 1)
				PickupGenerator.bPickups[i] = false;
		}

		Destroy(gameObject);
	}
}
