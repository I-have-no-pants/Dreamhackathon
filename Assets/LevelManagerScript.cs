using UnityEngine;
using System.Collections;

public class LevelManagerScript : MonoBehaviour {

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {

	}

	/* 
	 * Load a new level additively, i.e. you don't destroy the objects in the current scene.
	 * This method also keeps track of which objects that were loaded, so they can be unloaded additivenly later.
	 */
	public void LoadLevelAdditive (string levelName) {
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
		Debug.Log ("Loading scene " + levelName);
	}

	public void UnloadLevelAdditive(string levelName) {
		Transform level = this.transform.FindChild (levelName);
		Destroy(level.gameObject);
		Debug.Log("Unloading scene " + levelName);
	}
}