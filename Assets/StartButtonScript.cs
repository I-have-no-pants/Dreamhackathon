using UnityEngine;
using System.Collections;

public class StartButtonScript : MonoBehaviour {
	
	private CollisionManager cm;
	private GameManagerScript manager;

	public float ActivateTimer;

	public float timer;
	
	// Use this for initialization
	void Start () {
		cm = FindObjectOfType<CollisionManager>();
		manager = FindObjectOfType<GameManagerScript> ();
		timer = ActivateTimer;
	}

	void Update () {
		Vector2 pixel = cm.getTexturePosition(transform.position);
		if (pixel.x != 0 || pixel.y != 0) {
			if (cm.getCollision(pixel)) {
				timer -= Time.deltaTime;
				if (timer<=0) {
					manager.StartLoadingLevel();
					Destroy(gameObject);
				}
			} else if (timer<ActivateTimer) {
				timer += Time.deltaTime;
			}
		}
	}
}
