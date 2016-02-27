using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    //--------------------------------------------------------
    // Game variables

    public static int Level = 0;
    public static int lives = 0;

	public enum GameState { Init, Game, Dead, Scores }
	public static GameState gameState;
	public int differenceNum = 100;

    private GameObject pacman1;
	private GameObject pacman2;
    private GameObject blinky;
    private GameObject pinky;
    private GameObject inky;
    private GameObject clyde;
    private GameGUINavigation gui;

	public static bool bChaser;
    static public int p1_score;
	static public int p2_score;

	public Vector2[] deathPoints;

	//Win condition stuff
	public float TimeToWin;
	public GameObject LeadPlayer;
	public float timeLeft;

	public float scareLength;
	private float _timeToCalm;

    public float SpeedPerLevel;
    
    //-------------------------------------------------------------------
    // singleton implementation
    private static GameManager _instance;

    public static GameManager instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<GameManager>();
                DontDestroyOnLoad(_instance.gameObject);
            }

            return _instance;
        }
    }

    //-------------------------------------------------------------------
    // function definitions

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            if(this != _instance)   
                Destroy(this.gameObject);
        }

        AssignGhosts();
    }

	void Start () 
	{
		gameState = GameState.Init;
		timeLeft = (int)Mathf.Ceil(TimeToWin);
	}


	public int FindWinningPlayer()
	{
		int value = p1_score - p2_score;
		if(Mathf.Abs(value) > differenceNum)
		{
			if(value > 0)
			{
				return 1;
			}
			else if (value < 0)
			{
				return 2;
			}
			else{
				Debug.Log("Find winning player broke");
				return 0;
			}
		}
		return 0;
	}

    void OnLevelWasLoaded()
    {
        if (Level == 0) lives = 3;

        Debug.Log("Level " + Level + " Loaded!");
        AssignGhosts();
        ResetVariables();


        // Adjust Ghost variables!
//        clyde.GetComponent<GhostMove>().speed += Level * SpeedPerLevel;
//        blinky.GetComponent<GhostMove>().speed += Level * SpeedPerLevel;
//        pinky.GetComponent<GhostMove>().speed += Level * SpeedPerLevel;
//        inky.GetComponent<GhostMove>().speed += Level * SpeedPerLevel;
//        pacman1.GetComponent<PlayerController>().speed += Level*SpeedPerLevel/2;
//		pacman2.GetComponent<PlayerController>().speed += Level*SpeedPerLevel/2;
    }

    private void ResetVariables()
    {
        _timeToCalm = 0.0f;
		bChaser = true;
        PlayerController.killstreak = 0;
    }

    // Update is called once per frame
	void Update () 
	{
		//if(scared && _timeToCalm <= Time.time)
		//	CalmGhosts();


		int winner = FindWinningPlayer();
		if(winner > 0)
		{
			timeLeft -= Time.deltaTime;
		}
		else
		{
			timeLeft = TimeToWin;
		}

		if(timeLeft <= 0)
		{
			LoseLife();
			timeLeft = 0;
		}
			
	}

	public void ResetScene()
	{

		pacman1.transform.position = new Vector3(15f, 11f, 0f);
		pacman2.transform.position = new Vector3(15f, 11f, 0f);
//		blinky.transform.position = new Vector3(15f, 20f, 0f);
//		pinky.transform.position = new Vector3(14.5f, 17f, 0f);
//		inky.transform.position = new Vector3(16.5f, 17f, 0f);
//		clyde.transform.position = new Vector3(12.5f, 17f, 0f);

		pacman1.GetComponent<PlayerController>().ResetDestination();
		pacman2.GetComponent<PlayerController>().ResetDestination();
//		blinky.GetComponent<GhostMove>().InitializeGhost();
//		pinky.GetComponent<GhostMove>().InitializeGhost();
//		inky.GetComponent<GhostMove>().InitializeGhost();
//		clyde.GetComponent<GhostMove>().InitializeGhost();

        gameState = GameState.Init;  
        gui.H_ShowReadyScreen();

	}

	public void ToggleChaser()
	{
		if(!bChaser)	InversionToGreen();
		else 			InversionToYellow();
		Debug.Log("Inversion");
	}

	public void InversionToGreen()
	{
		bChaser = false;

	}

	public void InversionToYellow()
	{
		bChaser = true;

		//PlayerController.killstreak = 0;
    }

    void AssignGhosts()
    {
        // find and assign ghosts
//        clyde = GameObject.Find("clyde");
//        pinky = GameObject.Find("pinky");
//        inky = GameObject.Find("inky");
//        blinky = GameObject.Find("blinky");
        pacman1 = GameObject.Find("pacman");
		pacman2 = GameObject.Find("pacman_6");

        //if (clyde == null || pinky == null || inky == null || blinky == null) Debug.Log("One of ghosts are NULL");
        if (pacman1 == null) Debug.Log("Pacman1 is NULL");
		if (pacman2 == null) Debug.Log("Pacman2 is NULL");

        gui = GameObject.FindObjectOfType<GameGUINavigation>();

        if(gui == null) Debug.Log("GUI Handle Null!");

    }

    public void LoseLife()
    {
        lives--;
        gameState = GameState.Dead;
    
        // update UI too
        UIScript ui = GameObject.FindObjectOfType<UIScript>();
        //Destroy(ui.lives[ui.lives.Count - 1]);
        //ui.lives.RemoveAt(ui.lives.Count - 1);
    }

    public static void DestroySelf()
    {
        p1_score = 0;
		p2_score = 0;
		bChaser = true;
        Level = 0;
        lives = 3;
        Destroy(GameObject.Find("Game Manager"));
    }


}
