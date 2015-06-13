using UnityEngine;
using System.Collections;

public class GameManagerScript : MonoBehaviour {

	public DisplayColor color;
	public DisplayPlayers players;
	public DisplayDepth depth;

	public enum Gamestate {Menu, Playing, Death};
	public Gamestate state = Gamestate.Menu;

	public int score;

	// Use this for initialization
	void Start () {
		JumpToMenu ();
	}

	public void JumpToMenu() {
		state = Gamestate.Menu;
		color.update = true;
		players.update = true;
		depth.update = true;
	}

	public void JumpToEndgame() {
		state = Gamestate.Death;
	}

	public void StartLevel() {
		state = Gamestate.Playing;
		color.update = false;
		players.update = false;
		depth.update = false;
	}

	public int Protects;
	public void Death() {
		Protects--;
		if (Protects <= 0) {
			JumpToEndgame();
		}
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.F1)){
			StartLevel();
		}
	}
}
