﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameManagerScript : MonoBehaviour {

	public GameObject[] lifeObjects;

	public DisplayColor color;
	public DisplayPlayers players;
	public DisplayDepth depth;
	public ImageFuserCollision fusedCollision;
	public ImageFuser fusedImage;

	public LevelManagerScript levels;

	public enum Gamestate {Menu, Playing, Death};
	public Gamestate state = Gamestate.Menu;

	public PostToTwitterScript twitter;

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

		FindObjectOfType<Visualizer>().Next();

	}

	public void EndLevel() {
		if (moveMissiles) {
			finishedLevels++;
			//levels.UnloadLevelAdditive (Levels [currentLevel]);

			fusedImage.gameMode = false;
		
			moveMissiles = false;
			color.update = true;
			players.update = true;
			fusedCollision.update = true;
			depth.update = true;
			FindObjectOfType<Visualizer>().Next();
			StartLoadingLevel ();
		}
	}

	public void StartLoadingLevel() {
		currentLevel = (currentLevel + 1) % Levels.Length;
		Debug.Log ("Load " + currentLevel + ": "+ Levels [currentLevel]);
		levels.LoadLevelAdditive (Levels[currentLevel]);
		animator.SetTrigger ("loadNextLevel");

	}

	public int lifes = 3;

	public StartButtonScript start;

	public void RestartGame() {


		Debug.Log ("RESTART");
		Application.LoadLevel (0);
		/*start.gameObject.SetActive (true);
		currentLevel = -1;
		lifes = 3;*/
	}

	public InputField name;

	public int finishedLevels;

	public int Protects;
	public void Death(bool killed = false) {
		
		if (killed) {
			//if (lifes>0 && lifes < 3)

			lifes--;
			lifeObjects[lifes].SetActive(false);

		}

		Protects--;

		if (lifes <= 0 && moveMissiles) {
			fusedImage.gameMode = false;
			
			moveMissiles = false;
			color.update = true;
			players.update = true;
			fusedCollision.update = true;
			depth.update = true;
			animator.SetTrigger ("gameOver");

			
			twitter.postString = name.text + " reached level " + finishedLevels + " in Human Shield #dreamhackathon";

			Debug.Log(twitter.postString);
			StartCoroutine (twitter.Post ());

		} else if (Protects <= 0 && moveMissiles)
			EndLevel ();



	}

	public void EnemyDeath() {
		Enemies--;
		if (Enemies <= 0 && moveMissiles)
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
