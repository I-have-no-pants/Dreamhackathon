using UnityEngine;
using System.Collections;

public class EnemySpawnerLogic : MonoBehaviour {
	enum Direction {
		DownRight,
		DownLeft,
		Left,
		Right
	}
	enum SpawnPosition {
		TopLeft,
		TopMiddle,
		TopRight,
		MiddleLeft,
		MiddleRight,
	}
	public Transform prefab;
	// Use this for initialization
	void Start () {
		Instantiate (prefab, new Vector3 (4, 5, 0), new Quaternion ());
		CreateMissile (SpawnPosition.TopRight, Direction.DownLeft);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void CreateMissile(SpawnPosition pos, Direction dir) {
		Instantiate (prefab, new Vector3 (4, -5, 0), DirectionToQuaternion(dir));
	}

	Quaternion DirectionToQuaternion(Direction dir) {
		switch (dir) {
		case Direction.DownLeft:
			//return Quaternion.LookRotation(new Vector3(-0.5f, -0.5f, 0.0f));
			return Quaternion.LookRotation(new Vector3(1.0f, 1.0f, 0.0f));
		default:
			Debug.Log("Not implemeted yet!");
			return new Quaternion();
		}
	}

	Vector3 SpawnPositionToPosition(SpawnPosition spawnPosition) {
		return new Vector3(0.0f, -2.0f, 0.0f);
	}
}
