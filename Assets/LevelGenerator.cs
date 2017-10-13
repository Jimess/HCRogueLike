using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class LevelGenerator : MonoBehaviour {

	private int genCount = 50;
	public int squareWidth = 1;

	public int mapMaxSize = 500;

	public Sprite sprite;
	private SpriteRenderer rend;
	private Camera _mainCam;

	public GameObject goalPrefab;
	public GameObject endGoalPrefab;

	List<GameObject> map;

	public Transform parent;
	
	//perlin noise vars that need to be consistent
	float xSoundPos = 1f; //can add more RNG here by adding a RNG number and then incrementing
	float ySoundScale = 10f;

	public float lastEdgeX = 1f;
	public float lastEdgeY = 1f;
	public float perlinNoiseStep;

	// end game spawn boolean. makes sure it's not spawned again
	private bool _isGoalSpawned = false;

	public PhysicsMaterial2D groundMaterial;

	public GameObject carDriveEndTarget;

	public GameObject carPrefab;
	public Transform carSpawn;


//	public Square map;

	// Use this for initialization
	void Start () {
		perlinNoiseStep = Random.Range(0.0001f, 0.242f);
		print(perlinNoiseStep);
		xSoundPos = Random.Range (0.1f, 1000000f);
		map = new List<GameObject>();
		GenerateMap (genCount);
		GenerateLeftCollider();
		_mainCam = Camera.main;

		//Instantiate Car
		GameObject car = Instantiate (carPrefab, carSpawn.position, Quaternion.identity);
		//setting camera to this OBJ
		_mainCam.GetComponent<CameraController>().target = car.transform;


		//backwheel setup, loading from save data
		JointSuspension2D back_susp = new JointSuspension2D() {angle = car.GetComponent<CarMover>().backwheel.suspension.angle,
			frequency = GameController.controller.carInfo.back_freq, dampingRatio = GameController.controller.carInfo.back_damp};
			car.GetComponent<CarMover>().backwheel.suspension = back_susp;

		//front wheel setup, loading from save data
		JointSuspension2D front_susp = new JointSuspension2D() {angle = car.GetComponent<CarMover>().frontwheel.suspension.angle,
			frequency = GameController.controller.carInfo.front_freq, dampingRatio = GameController.controller.carInfo.front_damp};
			car.GetComponent<CarMover>().frontwheel.suspension = front_susp;
		
	}
	
	// Update is called once per frame
	void Update () {
		// if (_mainCam.)
	}

	void GenerateMap (int count) {
		#region Map Generation code
		// setting first tile start point
		//setting a parent to which gameobject will be  part of
		/* creating the first square to be the start of the map */

		float tempValue = GetSoundNumber (ySoundScale, xSoundPos);
		
		Square sq = new Square (0, lastEdgeX, tempValue, tempValue, 0, sprite.bounds.size.y);

			// lastEdgeX++;
		lastEdgeY = tempValue;

		GameObject temp = new GameObject ("0");
		//adding info script that can be accessed
		temp.AddComponent<MapBlockInfo> ();
		temp.GetComponent<MapBlockInfo>().TopPosition = sq.offsetYTOP;
		temp.tag = "Ground";
		temp.transform.SetParent (parent);

		//setting the GO position to be in the centre of collider
		temp.transform.localPosition = new Vector2 (sq.offsetX, sq.offsetY);

		PolygonCollider2D tempCol = temp.AddComponent <PolygonCollider2D> ();
		tempCol.points = sq.GetPoints ().ToArray (); // setting polygon collider points
		tempCol.sharedMaterial = groundMaterial; // adding a physics 2d material to the ground

		//adding a sprite renderer and its texture
		// print("Size" + sprite.bounds.size.y);

		GameObject spriteGO = new GameObject ();
		spriteGO.transform.SetParent (temp.transform);

		rend = spriteGO.AddComponent<SpriteRenderer> ();
		rend.sprite = sprite;

		spriteGO.transform.localRotation = Quaternion.Euler (0,0,sq.angle); // first rotate the sprite AND THE PUSH IT DOWN;
		spriteGO.transform.localPosition = new Vector2 (0f, sq.offsetYTOP) - (Vector2)(spriteGO.transform.up * (sq.spriteHeight/2));
		/*TEMPORARY */
		// spriteGO.transform.localPosition = new Vector2 (0f, sq.offsetYTOP);
		spriteGO.transform.localScale = new Vector2 (sq.topLineScale, 1);

		// adding the GO to the map List
		map.Add(temp);


		//declaring initial objects
		// Square sq;
		// GameObject temp;
		// PolygonCollider2D tempCol;

		for (int i = 1; i < genCount; i++) {
			tempValue = GetSoundNumber (ySoundScale, xSoundPos);

			sq = new Square (lastEdgeX, lastEdgeX + 1, tempValue, lastEdgeY, i, sprite.bounds.size.y); // creating new square 1 to the right and adding its height as a random perlin number

			//the next start polygon points are going to be this blocks end points (SO THEY CONNECT TOGETHER)
			lastEdgeX++;
			lastEdgeY = tempValue;

			temp = new GameObject (i.ToString());

			//adding info script that can be accessed
			temp.AddComponent<MapBlockInfo> ();
			temp.GetComponent<MapBlockInfo>().TopPosition = sq.offsetYTOP;

			temp.tag = "Ground";
			temp.layer = LayerMask.NameToLayer ("Ground");
			temp.transform.SetParent (parent);
			//spriteGO is an object inside a temp object that only holds the sprite and aligns it to the line
			spriteGO = new GameObject ();
			spriteGO.transform.SetParent (temp.transform);

			rend = spriteGO.AddComponent<SpriteRenderer> ();
			rend.sprite = sprite;

			spriteGO.transform.localRotation = Quaternion.Euler (0,0,sq.angle); // first rotate the sprite AND THE PUSH IT DOWN;
			spriteGO.transform.localPosition = new Vector2 (0f, sq.offsetYTOP) - (Vector2)(spriteGO.transform.up * (sq.spriteHeight/2));
			/*TEMPORARY */
			// spriteGO.transform.localPosition = new Vector2 (0f, sq.offsetYTOP);
			spriteGO.transform.localScale = new Vector2 (sq.topLineScale, 1);

			//setting the GO position to be in the centre of collider
			temp.transform.localPosition = new Vector2 (i + sq.offsetX, sq.offsetY);

			//adding and setting polygon collider
			tempCol = temp.AddComponent <PolygonCollider2D> ();
			tempCol.points = sq.GetPoints ().ToArray ();
			tempCol.sharedMaterial = groundMaterial; // adding a physics 2d material to the ground

			//adding the GO to the map list
			map.Add (temp);

			//incrementing scale for the perlin noise function
			xSoundPos+=perlinNoiseStep;
		}
		#endregion
	}

	public void GenerateNextMapSection (int count) {
		int currentID = int.Parse(map.Last().name) + 1;
		
		Square sq;
		GameObject temp;
		PolygonCollider2D tempCol;

		bool goalSpawn = false; // trigger to spawn an endgame

		if (currentID + count > mapMaxSize) {
			count = mapMaxSize - currentID;
			goalSpawn = true;
		}

		for (int i = currentID; i <= currentID+count; i++) {
			float tempValue = GetSoundNumber (ySoundScale, xSoundPos);
			
			//making the last square flat so the end game flag can be placed
			if (i == currentID+count && goalSpawn) {
				sq = new Square (lastEdgeX, lastEdgeX + 1, lastEdgeY, lastEdgeY, i, sprite.bounds.size.y); // creating new square 1 to the right and adding its height as a random perlin number
			} else {
				sq = new Square (lastEdgeX, lastEdgeX + 1, tempValue, lastEdgeY, i, sprite.bounds.size.y);
				lastEdgeY = tempValue;
			}

			//the next start polygon points are going to be this blocks end points (SO THEY CONNECT TOGETHER)
			lastEdgeX++;
			

			temp = new GameObject (i.ToString());

			//adding info script that can be accessed
			temp.AddComponent<MapBlockInfo> ();
			temp.GetComponent<MapBlockInfo>().TopPosition = sq.offsetYTOP;

			temp.tag = "Ground";
			temp.layer = LayerMask.NameToLayer ("Ground");
			temp.transform.SetParent (parent);
			//spriteGO is an object inside a temp object that only holds the sprite and aligns it to the line
			GameObject spriteGO = new GameObject ();
			spriteGO.transform.SetParent (temp.transform);

			rend = spriteGO.AddComponent<SpriteRenderer> ();
			rend.sprite = sprite;

			spriteGO.transform.localRotation = Quaternion.Euler (0,0,sq.angle); // first rotate the sprite AND THE PUSH IT DOWN;
			spriteGO.transform.localPosition = new Vector2 (0f, sq.offsetYTOP) - (Vector2)(spriteGO.transform.up * (sq.spriteHeight/2));
			/*TEMPORARY */
			// spriteGO.transform.localPosition = new Vector2 (0f, sq.offsetYTOP);
			spriteGO.transform.localScale = new Vector2 (sq.topLineScale, 1);

			//setting the GO position to be in the centre of collider
			temp.transform.localPosition = new Vector2 (i + sq.offsetX, sq.offsetY);

			//adding and setting polygon collider
			tempCol = temp.AddComponent <PolygonCollider2D> ();
			tempCol.points = sq.GetPoints ().ToArray ();
			tempCol.sharedMaterial = groundMaterial; // adding a physics 2d material to the ground

			//adding the GO to the map list
			map.Add (temp);

			//incrementing scale for the perlin noise function
			xSoundPos+=perlinNoiseStep;
		}

		/* END PART */
		//make the last item flat and place the flag on top
		if (goalSpawn && !_isGoalSpawned ) {
			float endX = map.Last().transform.position.x;
			float endY = map.Last().GetComponent<MapBlockInfo>().TopPosition;

			Instantiate (goalPrefab, new Vector3 (endX, endY + (goalPrefab.GetComponent<SpriteRenderer>().bounds.size.y), 0f), Quaternion.identity);
			_isGoalSpawned = true;

			//when the end goal is set, spawn a section of map for the car to drive to and stop
			print("Generating Ending!");
			GenerateMapEnding (25);
		}
		
	}

	void GenerateMapEnding (int count) {
		int currentID = int.Parse(map.Last().name) + 1;
		
		Square sq;
		GameObject temp;
		PolygonCollider2D tempCol;

		for (int i = currentID; i <= currentID+count; i++) {
			// float tempValue = GetSoundNumber (ySoundScale, xSoundPos);
			
			// if (i == currentID+count) {
			// 	sq = new Square (lastEdgeX, lastEdgeX + 1, lastEdgeY, lastEdgeY, i, sprite.bounds.size.y); // creating new square 1 to the right and adding its height as a random perlin number
			// } else {
			// 	sq = new Square (lastEdgeX, lastEdgeX + 1, tempValue, lastEdgeY, i, sprite.bounds.size.y);
			// 	lastEdgeY = tempValue;
			// }

			sq = new Square (lastEdgeX, lastEdgeX + 1, lastEdgeY, lastEdgeY, i, sprite.bounds.size.y); // creating new square 1 to the right and adding its height as a random perlin number

			//the next start polygon points are going to be this blocks end points (SO THEY CONNECT TOGETHER)
			lastEdgeX++;
			

			temp = new GameObject (i.ToString());

			//adding info script that can be accessed
			temp.AddComponent<MapBlockInfo> ();
			temp.GetComponent<MapBlockInfo>().TopPosition = sq.offsetYTOP;

			temp.tag = "Ground";
			temp.layer = LayerMask.NameToLayer ("Ground");
			temp.transform.SetParent (parent);
			//spriteGO is an object inside a temp object that only holds the sprite and aligns it to the line
			GameObject spriteGO = new GameObject ();
			spriteGO.transform.SetParent (temp.transform);

			rend = spriteGO.AddComponent<SpriteRenderer> ();
			rend.sprite = sprite;

			spriteGO.transform.localRotation = Quaternion.Euler (0,0,sq.angle); // first rotate the sprite AND THE PUSH IT DOWN;
			spriteGO.transform.localPosition = new Vector2 (0f, sq.offsetYTOP) - (Vector2)(spriteGO.transform.up * (sq.spriteHeight/2));
			/*TEMPORARY */
			// spriteGO.transform.localPosition = new Vector2 (0f, sq.offsetYTOP);
			spriteGO.transform.localScale = new Vector2 (sq.topLineScale, 1);

			//setting the GO position to be in the centre of collider
			temp.transform.localPosition = new Vector2 (i + sq.offsetX, sq.offsetY);

			//adding and setting polygon collider
			tempCol = temp.AddComponent <PolygonCollider2D> ();
			tempCol.points = sq.GetPoints ().ToArray ();
			tempCol.sharedMaterial = groundMaterial; // adding a physics 2d material to the ground

			//adding the GO to the map list
			map.Add (temp);

			//incrementing scale for the perlin noise function
			xSoundPos+=perlinNoiseStep;
		}


		//palce another GO with the position of the middle of the ending for the car to drive to it
		carDriveEndTarget = Instantiate (endGoalPrefab, new Vector3 (map[map.Count()-1-(count/2)].transform.position.x,
		map[map.Count()-1-(count/2)].GetComponent<MapBlockInfo>().TopPosition + (goalPrefab.GetComponent<SpriteRenderer>().bounds.size.y), 0f), Quaternion.identity);

	}


	public void EndGameCarMove (GameObject car) {
		
        //    rigidbody.MovePosition(transform.position + direction * movementSpeed * Time.deltaTime);
		carDriveEndTarget.transform.position = new Vector2 (carDriveEndTarget.transform.position.x, car.transform.position.y);
		//works but look cranky
		// StartCoroutine(MoveTheCar (car));
		//works better but results not ideal
		// iTween.MoveTo (car, iTween.Hash ("position", carDriveEndTarget.transform.position, "time", 5f));

		car.GetComponent<CarMover> ().StopInput (); // ends car movement ability
		StartCoroutine(MoveTheCarPhysics(car));
	}


	IEnumerator MoveTheCarPhysics (GameObject car) {
		Vector3 direction = (carDriveEndTarget.transform.position - car.transform.position).normalized;
		// Rigidbody2D carRb = car.GetComponent<Rigidbody2D>();
		float movementFoce = 10f;
		while(true) {
			// if (endGoalPrefab.transform.position.x > car.transform.position.x) {
			// 	car.GetComponent<Rigidbody2D>().AddForce(direction * movementFoce, ForceMode2D.Force);
			// } else {
			// 	car.GetComponent<Rigidbody2D>().velocity = new Vector2(0f,0f);
			// 	break;
			// }
			car.GetComponent<Rigidbody2D>().AddForce(direction * movementFoce, ForceMode2D.Force);
			if ((carDriveEndTarget.transform.position - car.transform.position).magnitude < 0.5f) {
				car.GetComponent<Rigidbody2D>().velocity = new Vector2(0f,0f);
				car.GetComponent<CarMover>().StopMovement();
				break;
			} 
			yield return new WaitForFixedUpdate();
			// yield return new WaitForSeconds(1f);
		}
	}

	//support coroutine for the end game car move
	// IEnumerator MoveTheCar (GameObject car) {
	// 	// Vector3 direction = (carDriveEndTarget.transform.position - car.transform.position).normalized;
	// 	// Rigidbody2D carRb = car.GetComponent<Rigidbody2D>();
	// 	float movementSpeed = 1f;
	// 	while(true) {
	// 		float step = movementSpeed * Time.deltaTime;
	// 		car.transform.position = Vector2.MoveTowards (car.transform.position, carDriveEndTarget.transform.position, step);
	// 		yield return new WaitForFixedUpdate();
	// 		// yield return new WaitForSeconds(1f);
	// 	}
	// }


	void GenerateLeftCollider () {
		GameObject leftCol = new GameObject("LeftCollider");
		leftCol.transform.localPosition = new Vector2(-0.5f, 0.5f); // setting the offset of the GO
		BoxCollider2D boxCol = leftCol.AddComponent <BoxCollider2D> ();
		boxCol.size = new Vector2 (1f, 20f);
	}


	// this function returns a 1 dimensional perlin noise number
	private float GetSoundNumber (float hScale, float xScale) {
		return hScale * Mathf.PerlinNoise (xScale, 0.0f);
	}

	public string GetLastMapID (){
		return map.Last().name;
	}
}
