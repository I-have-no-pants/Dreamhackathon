using UnityEngine;
using System.Collections;

public class LevelManagerScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		LoadLevelAdditive ("Wave1");
		Debug.Log ("hello world");
	}
	
	// Update is called once per frame
	void Update () {
		if (Time.fixedTime > 3.0f) {
			Transform level = this.transform.Find("Wave1");
			if(level) {
				level.parent = null;
			}
		}
	}

	/* 
	 * Load a new level additively, i.e. you don't destroy the objects in the current scene.
	 * This method also keeps track of which objects that were loaded, so they can be unloaded additivenly later.
	 */
	void LoadLevelAdditive (string levelName) {
		/* 
		 * Create a new game object and let it's parent be the LevelManager.
		 * ALL the objects in the scene that's loading will look up this object and
		 * set it to be it's parent. This is so that whenever this game objects is destroyed, 
		 * all it's children are also destroyed and the level gets unloaded.
		 * 
		 * Note: all game objects created in the scene must set this object as it's parent. 
		 * Otherwise things won't work.
		 */ 
		GameObject level = new GameObject ();
		level.transform.parent = this.transform;
		level.name = levelName;

		Application.LoadLevelAdditive (levelName);
	}

	void UnloadLevelAdditive(string levelName) {
		Transform level = this.transform.FindChild (levelName);

		// The level is now an orphan. This will cause it to be destroyed, since no
		// other object refers to it.
		level.parent = null;
	}
}