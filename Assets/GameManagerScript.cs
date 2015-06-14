using UnityEngine;
using System.Collections;

public class GameManagerScript : MonoBehaviour {

	public DisplayColor color;
	public DisplayPlayers players;
	public DisplayDepth depth;
	public ImageFuserCollision fusedCollision;
	public ImageFuser fusedImage;

	public LevelManagerScript levels;

	public enum Gamestate {Menu, Playing, Death};
	public Gamestate state = Gamestate.Menu;

	public int score;

	private Animator animator;

	public bool moveMissiles = false;

	
	public int Enemies;

	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator> ();
		JumpToMenu ();

	}

	public void JumpToMenu() {
		state = Gamestate.Menu;
		color.update = true;
		players.update = true;
		Debug.Log ("menu");
		depth.update = true;
	}

	public void JumpToEndgame() {
		state = Gamestate.Death;
		Debug.Log ("endgame");
	}


	public int currentLevel=0;

	public string[] Levels;
	public void NextLevel() {

		//levels.LoadLevelAdditive ("Wave"+currentLevel++);
		// Activate missiles here
		Debug.Log ("Activate missiles");
		moveMissiles = true;
		color.update = false;
		players.update = false;
		depth.update = false;
		fusedCollision.update = false;
		fusedImage.gameMode = true;
	}

	public void EndLevel() {
		if (moveMissiles) {
			//levels.UnloadLevelAdditive (Levels [currentLevel]);

			fusedImage.gameMode = false;
		
			moveMissiles = false;
			color.update = true;
			players.update = true;
			fusedCollision.update = true;
			depth.update = true;
			StartLoadingLevel ();
		}
	}

	public void StartLoadingLevel() {
		currentLevel = (currentLevel + 1) % Levels.Length;
		Debug.Log ("Load " + currentLevel + ": "+ Levels [currentLevel]);
		levels.LoadLevelAdditive (Levels[currentLevel]);
		animator.SetTrigger ("loadNextLevel");

	}

	public int Protects;
	public void Death() {
		Protects--;
		if (Protects <= 0)
			EndLevel ();

	}

	public void EnemyDeath() {
		Enemies--;
		if (Enemies <= 0)
			EndLevel ();
		
	}

	
	// Update is called once per frame
	void Update () {
		/*if(Input.GetKeyDown(KeyCode.F1)){
			NextLevel();
		}

		if(Input.GetKeyDown(KeyCode.F2)){
			EndLevel();
		}*/

	}
}
