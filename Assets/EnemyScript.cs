using UnityEngine;
using System.Collections;

public class EnemyScript : MonoBehaviour {

	private CollisionManager cm;
	private GameManagerScript manager;
	
	// Use this for initialization
	void Start () {
		manager = FindObjectOfType<GameManagerScript> ();
		if (manager)
			manager.Enemies++;

		cm = FindObjectOfType<CollisionManager>();


	}

	void OnDestroy() {
		if (manager)
			manager.EnemyDeath ();
	}


	public void Explode() {
		transform.Translate(Vector3.down*Time.deltaTime);
		if (cm) {
			Vector2 pixel = cm.getTexturePosition (transform.position);
			cm.removePixel (pixel, 15);
		}
		Destroy (gameObject);
	}
}
