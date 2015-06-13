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
		transform.Translate(Vector3.right*Time.deltaTime);
		Vector2 pixel = cm.getTexturePosition(transform.position);
		Debug.Log(pixel);
		if (pixel.x != 0 || pixel.y != 0) {
			if (cm.getCollision(pixel)) {
				cm.removePixel(pixel,20);
				Destroy(gameObject);
			}
		}
	}
}
