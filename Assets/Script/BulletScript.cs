using UnityEngine;
using System.Collections;

public class BulletScript : MonoBehaviour {

	private CollisionManager cm;

	// Use this for initialization
	void Start () {
		cm = FindObjectOfType<CollisionManager>();
	}
	
	// Update is called once per frame
	void Update () {
		transform.Translate(Vector3.down*Time.deltaTime);
		Vector2 pixel = cm.getTexturePosition(transform.position);
		if (cm.getCollision(pixel)) {
			cm.removePixel(pixel,5);
			//Destroy(gameObject);
		}
	}
}
