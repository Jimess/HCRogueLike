using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SuspensionInfo : MonoBehaviour {

	public Text backFreqText;
	public Slider backFreqSlider;
	public Text backDampText;
	public Slider backDampSlider;
	public GameObject showCar;
	private ShowCarScript _carScript;

	public Text frontFreqText;
	public Slider frontFreqSlider;
	public Text frontDampText;
	public Slider frontDampSlider;

	// Use this for initialization
	void Start () {
		_carScript = showCar.GetComponent<ShowCarScript>();

		//*Back SUSPENSION */
		backFreqSlider.value = GameController.controller.carInfo.back_freq;
		backFreqSlider.onValueChanged.AddListener (delegate {OnFreqSliderChange(0); });

		backDampSlider.value = GameController.controller.carInfo.back_damp;
		backDampSlider.onValueChanged.AddListener (delegate {OnDampSliderChange(0); });

		backFreqText.text = "Back Freq: " + backFreqSlider.value;
		backDampText.text = "Back Damping: " + backDampSlider.value;
		/*END Back SUSPENSION */

		/*Front SUSPENSION */
		frontFreqSlider.value = GameController.controller.carInfo.front_freq;
		frontFreqSlider.onValueChanged.AddListener (delegate {OnFreqSliderChange(1); });

		frontDampSlider.value = GameController.controller.carInfo.front_damp;
		frontDampSlider.onValueChanged.AddListener (delegate {OnDampSliderChange(1);});

		frontFreqText.text = "Front Freq: " + frontFreqSlider.value;
		frontDampText.text = "Front Damping: " + frontDampSlider.value;

		/*END Front SUSPENSION */



	}
	
	// Update is called once per frame
	void Update () {
		
	}


	public void OnFreqSliderChange(int id) {
		if (id == 0) { // back suspension
			//change the values for the main game
			backFreqText.text = "Back Freq: " + backFreqSlider.value;
			GameController.controller.carInfo.back_freq = backFreqSlider.value;

			//change the values for the showCar
			_carScript.ChangeBackSuspension (backFreqSlider.value, backDampSlider.value);
		} else if (id == 1) {
			frontFreqText.text = "Front Freq: " + frontFreqSlider.value;
			GameController.controller.carInfo.front_freq = frontFreqSlider.value;

			_carScript.ChangeFrontSuspension (frontFreqSlider.value, frontDampSlider.value);
		}
		

		

	}

	public void OnDampSliderChange(int id) {
		if (id == 0) { // back suspension
			//change the values for the main game
			backDampText.text = "Back Damping: " + backDampSlider.value;
			GameController.controller.carInfo.back_damp = backDampSlider.value;

			//change the values for the showCar
			_carScript.ChangeBackSuspension (backFreqSlider.value, backDampSlider.value);
		} else if (id == 1) {
			frontDampText.text = "Front Damping: " + frontDampSlider.value;
			GameController.controller.carInfo.front_damp = frontDampSlider.value;

			_carScript.ChangeFrontSuspension (frontFreqSlider.value, frontDampSlider.value);
		}
	}
}
