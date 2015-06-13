using UnityEngine;
using System.Collections;

public class BulletScript : MonoBehaviour {

	private CollisionManager cm;
	public int explosionRadius = 20;
	public int drillRadius = 10;
	public int drillLength = 3;
	public bool drill = false;
	public bool clusterBomb = false;
	public bool clusterBombChild = false;
	public int clusterBombCount = 4;
	private float clusterTimer = 1;

	// Use this for initialization
	void Start () {
		cm = FindObjectOfType<CollisionManager>();
	}
	
	// Update is called once per frame
	void Update () {
		transform.Translate(Vector3.down*Time.deltaTime);
		transform.Translate(Vector3.right*Time.deltaTime);
		Vector2 pixel = cm.getTexturePosition(transform.position);
		if (pixel.x != 0 || pixel.y != 0) {
			if (cm.getCollision(pixel)) {
				if (drill)
					drillLength = drillOnCollision(pixel,drillLength);
				else
					dieOnCollision(pixel);
			}
		}
		if (clusterBombChild) {
			if (clusterTimer < 0)
				dieOnCollision(pixel);
			clusterTimer -= Time.deltaTime;
		}
	}

	void dieOnCollision(Vector2 pixel) {
		cm.removePixel(pixel,explosionRadius);
		if (clusterBomb) {
			while (clusterBombCount > 0) {
				clusterBombCount--;
				Quaternion rotation = Quaternion.LookRotation(new Vector3(Random.Range(-100,100),Random.Range(-100,100),0),new Vector3(0,0,1));
				BulletScript bullet = Instantiate(this,transform.position,rotation) as BulletScript;
				bullet.clusterBomb = false;
				bullet.drill = false;
				bullet.clusterBombChild = true;
				bullet.explosionRadius = explosionRadius/2;
			}
		}
		Destroy(gameObject);
	}

	int drillOnCollision(Vector2 pixel, int count) {
		if (count > 0)
			cm.removePixel(pixel,drillRadius);
		else
			dieOnCollision(pixel);
		return count-1;
	}
}
