using UnityEngine;
using System.Collections;

public class UnloadLevel : MonoBehaviour {
	
	private GameManagerScript manager;

	public int myLevel;

	// Use this for initialization
	void Start () {
		manager = FindObjectOfType<GameManagerScript> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (manager && manager.currentLevel != myLevel)
			Destroy (gameObject);
	}
}
