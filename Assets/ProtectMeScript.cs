using UnityEngine;
using System.Collections;

public class ProtectMeScript : MonoBehaviour {

	private GameManagerScript manager;
	private CollisionManager cm;
	public int explosionRadius = 20;

	// Use this for initialization
	void Start () {
		manager = FindObjectOfType<GameManagerScript> ();
		if (manager)
			manager.Protects++;
		cm = FindObjectOfType<CollisionManager>();
	}
	
	// Update is called once per frame
	void Update () {
		if (cm) {
			Vector2 pixel = cm.getTexturePosition(transform.position);
			if (cm.getCollision(pixel)) {
				cm.removePixel(pixel,explosionRadius);
				Kill();
			}
		}
	}

	void OnCollisionEnter2D(Collision2D coll) {
		EnemyScript e = coll.gameObject.GetComponent<EnemyScript>();
		if (e) {
			Kill ();
			e.Explode();

		}
	}

	public void Kill() {
		Destroy (gameObject);
		if (manager)
			manager.Protects--;
	}
}
