using UnityEngine;
using System.Collections;

public class PickupGenerator : MonoBehaviour
{

	public static bool[] bPickups;
	public GameObject[] Pickups;
	public static Vector2[] SpawnPoints= new Vector2[]{new Vector2(4,8),new Vector2 (25,26), new Vector2(4,26), new Vector2(25,8)};

	public float TimeBetween;
	public int MaxSimultaneous;
	private float timeUntilNext;
	private Vector2 nullVector = new Vector2(-1,-1);

	void Start ()
	{
		this.timeUntilNext = this.TimeBetween;
		var t = this.gameObject.transform;
		bPickups = new bool[]{true, true, true, true};
		for(int i = 0; i <4; i++) {
			
		}
	}
		

	void FixedUpdate ()
	{
		//GameObject[] activePickups = GameObject.FindGameObjectsWithTag ("Powerup");

		//Debug.Log(activePickups.Length);
		int numPickups = 0;
		for (int i = 0; i < 4; i++) {
			if (bPickups[i])
				numPickups++;
		}

		if (numPickups < this.MaxSimultaneous)
		{
			this.timeUntilNext -= Time.fixedDeltaTime;
			if (this.timeUntilNext < 0)
			{
				Vector2 sPoint= pickSpawn();
				int f = Random.Range(0, 3);
				this.timeUntilNext = this.TimeBetween;
				if (sPoint != nullVector) {
					var go = GameObject.Instantiate (this.Pickups[f], sPoint, Quaternion.identity) as GameObject;
					Debug.Log("Instantiated " + sPoint.ToString());
				}
			}
		}
	}

	Vector2 pickSpawn() 
	{
//		bool empty = true;
//		int f = Random.Range(0, 4);
//		int counter =0;
//		foreach (GameObject g in active) {
//				counter++;
//				if (Vector2.Distance(((Vector2)g.transform.position), (SpawnPoints[f])) < 1) {
//					empty = false;
//				}
//			}
//			if (empty) {
//				f = counter;
//		}
		int i = 0;
		while(bPickups[i]) {
			i++;
			if (i >= 4) {
				return nullVector;
			}
		}
		bPickups[i] = true;

		return SpawnPoints[i];
	}
}
