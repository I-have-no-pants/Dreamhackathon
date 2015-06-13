using UnityEngine;
using System.Collections;

public class ProtectMeScript : MonoBehaviour {

	private GameManagerScript manager;

	// Use this for initialization
	void Start () {
		manager = FindObjectOfType<GameManagerScript> ();
		manager.Protects++;
	}
	
	// Update is called once per frame
	void Update () {
	
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
		manager.Protects--;
	}
}
