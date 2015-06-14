using UnityEngine;
using System.Collections;

public class MissileLogic : MonoBehaviour {
	public float speed;

	private CollisionManager cm;
	public Transform target;


	private GameManagerScript manager;

	public int explosionRadius = 20;
	public int drillRadius = 10;
	public int drillLength = 3;
	public bool drill = false;
	public bool clusterBomb = false;
	public bool clusterBombChild = false;
	public int clusterBombCount = 4;
	private float clusterTimer = 1;

	public MissileLogic ClusterChild;
	
	// Use this for initialization
	void Start () {
		cm = FindObjectOfType<CollisionManager>();
		if (target) {
			transform.rotation = Quaternion.LookRotation(target.transform.position - transform.position, new Vector3(1,0,0));
		}
		transform.position = new Vector3(transform.position.x,transform.position.y,-1);
		manager = FindObjectOfType<GameManagerScript> ();
	}

	bool deleted = false;

	public float time = 5;

	// Update is called once per frame
	void Update () {
		//transform.position += speed * transform.right * Time.deltaTime;
		//transform.Translate (speed * transform.right * Time.deltaTime);
		if (!manager || manager.moveMissiles) {

			if(!deleted) {
				Destroy(gameObject, time);
				deleted = true;
			}

			GetComponent<Rigidbody2D> ().velocity = speed * transform.forward;
			if (cm) {
				Vector2 pixel = cm.getTexturePosition (transform.position);

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
		}
	}

	void dieOnCollision(Vector2 pixel) {
		cm.removePixel(pixel,explosionRadius);
		if (clusterBomb) {
			while (clusterBombCount > 0) {
				clusterBombCount--;
				Quaternion rotation = Quaternion.LookRotation(new Vector3(Random.Range(-100,100),Random.Range(-100,100),0),new Vector3(0,0,1));
				MissileLogic bullet = Instantiate(ClusterChild,transform.position,rotation) as MissileLogic;
			}
		}
		GetComponent<EnemyScript>().Explode();
	}
	
	int drillOnCollision(Vector2 pixel, int count) {
		if (count > 0)
			cm.removePixel(pixel,drillRadius);
		else
			dieOnCollision(pixel);
		return count-1;
	}
}
