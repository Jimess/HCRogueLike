using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomizationEventController : MonoBehaviour {

	public GameObject suspensionPanel;
	public GameObject modifyPanel;

	void OnEnable() {
		if (modifyPanel.activeInHierarchy) {
			modifyPanel.SetActive(false);
		}
		suspensionPanel.SetActive(true);
	}

	public void SwitchToModify() {
		suspensionPanel.SetActive(false);
		modifyPanel.SetActive(true);
	}

	public void SwitchToSuspension() {
		modifyPanel.SetActive(false);
		suspensionPanel.SetActive(true);
	}
}
