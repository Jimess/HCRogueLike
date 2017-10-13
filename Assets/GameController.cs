using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class GameController : MonoBehaviour {

	public static GameController controller;

	public float health;

	public PlayerCarInfo carInfo;


	void Awake () {
		if (controller == null) {
			DontDestroyOnLoad(gameObject);
			controller = this;
			Load();
		} else if (controller != this) {
			Destroy (gameObject);
		}
	}

	public void Save() {
		BinaryFormatter bf = new BinaryFormatter();
		FileStream file = File.Open(Application.persistentDataPath + "/playerSave.data", FileMode.Open);

		PlayerSave save = new PlayerSave ();
		save.health = health;
		
		//Car Save info
		save.carInfo = new PlayerCarInfo (carInfo.back_freq, carInfo.back_damp, carInfo.front_freq, carInfo.front_damp);

		bf.Serialize (file, save);

		file.Close();
	}

	public void Load() {
		if (File.Exists(Application.persistentDataPath + "/playerSave.data")) {
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = File.Open(Application.persistentDataPath + "/playerSave.data", FileMode.Open);

			PlayerSave load = (PlayerSave)bf.Deserialize (file);

			file.Close();

			health = load.health;
			carInfo = new PlayerCarInfo (load.carInfo.back_freq, load.carInfo.back_damp, load.carInfo.front_freq, load.carInfo.front_damp);
		} else { // if not load file just create with default values
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = File.Open(Application.persistentDataPath + "/playerSave.data", FileMode.OpenOrCreate);

			PlayerSave save = new PlayerSave ();
			save.health = health;
			
			//Car Save info
			save.carInfo = new PlayerCarInfo (2f, 1f, 2f, 1f);

			bf.Serialize (file, save);

			file.Close();

			Load();
		}
	}

}
[Serializable]
class PlayerSave {
	public float health;
	public PlayerCarInfo carInfo;
}

[Serializable]
public class PlayerCarInfo {
	public float back_freq;
	public float front_freq;
	public float back_damp;
	public float front_damp;

	public PlayerCarInfo (float back_freq, float back_damp, float front_freq, float front_damp){
		this.back_freq = back_freq;
		this.front_freq = front_freq;
		this.back_damp = back_damp;
		this.front_damp = front_damp;
	}
}