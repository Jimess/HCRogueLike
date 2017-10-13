using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	public Transform target;
	public float offstetX;
	public float offsetY;
	
	//private ref to the level Generator
	private LevelGenerator gen;

	void Start () {
		gen = GameObject.Find ("LevelController").GetComponent <LevelGenerator> (); 
	}

	void LateUpdate ()
	{
		Vector3 newPosition = new Vector2 (target.position.x + offstetX, target.position.y + offsetY);
		newPosition.z = -10;

		transform.position = newPosition;
	}

	void OnTriggerExit2D (Collider2D other) {
		if (other.name == gen.GetLastMapID()) {
			// print("Last ITEM!!");
			//Generate the next map section;
			gen.GenerateNextMapSection (5);
		}
	}
}
