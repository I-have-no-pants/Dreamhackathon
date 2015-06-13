using UnityEngine;
using System.Collections;

public class StartButtonScript : MonoBehaviour {
	
	private CollisionManager cm;
	private GameManagerScript manager;
	
	// Use this for initialization
	void Start () {
		cm = FindObjectOfType<CollisionManager>();
		manager = FindObjectOfType<GameManagerScript> ();
	}

	void Update () {
		Vector2 pixel = cm.getTexturePosition(transform.position);
		if (pixel.x != 0 || pixel.y != 0) {
			if (cm.getCollision(pixel)) {
				manager.StartLoadingLevel();
				Destroy(gameObject);
			}
		}
	}
}
