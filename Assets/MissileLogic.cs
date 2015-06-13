using UnityEngine;
using System.Collections;

public class MissileLogic : MonoBehaviour {
	public float speed;

	private CollisionManager cm;

	public bool Homing = true;

	private GameManagerScript manager;
	
	// Use this for initialization
	void Start () {
		cm = FindObjectOfType<CollisionManager>();
		if (Homing) {
			ProtectMeScript target = FindObjectOfType<ProtectMeScript>();
			transform.rotation = Quaternion.LookRotation(target.transform.position - transform.position, new Vector3(1,0,0));
		}
		manager = FindObjectOfType<GameManagerScript> ();
	}

	
	// Update is called once per frame
	void Update () {
		//transform.position += speed * transform.right * Time.deltaTime;
		//transform.Translate (speed * transform.right * Time.deltaTime);
		if (!manager || manager.moveMissiles) {
			GetComponent<Rigidbody2D> ().velocity = speed * transform.forward;
			if (cm) {
				Vector2 pixel = cm.getTexturePosition (transform.position);
				if (pixel.x != 0 || pixel.y != 0) {
					if (cm.getCollision (pixel)) {
						cm.removePixel (pixel, 20);
						Destroy (gameObject);
					}
				}
			}
		}
	}
}
