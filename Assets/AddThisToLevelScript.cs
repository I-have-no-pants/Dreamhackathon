using UnityEngine;
using System.Collections;

public class AddThisToLevelScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		// This is needed to properly destroy the object when the level is destroyed.
		GameObject levelManager = GameObject.Find ("LevelManager");
		this.transform.parent = levelManager.transform;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
