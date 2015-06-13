using UnityEngine;
using System.Collections;

public class ProtectMeScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
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
	}
}
