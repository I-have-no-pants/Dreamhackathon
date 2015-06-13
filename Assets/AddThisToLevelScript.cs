using UnityEngine;
using System.Collections;

public class AddThisToLevelScript : MonoBehaviour {
	// Use this for initialization
	void Start () {
		// This is needed to properly destroy the object when the level is destroyed.
		GameObject levelManager = GameObject.Find ("LevelManager");
		if (levelManager) {
			GameObject sceneNameObj = GameObject.Find ("SceneName");
			string sceneName = sceneNameObj.GetComponent<SceneNameScript> ().sceneName;
			Debug.Log (sceneName);
			Transform level = levelManager.transform.Find (sceneName); 
			this.transform.parent = level;

		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
