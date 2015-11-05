﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using BibaFramework.BibaMenu;

[RequireComponent (typeof(Toggle))]

public class PlaySFXOnToggle : MonoBehaviour {
	
	public SceneMenuStateView view;
	public string sfxString;
	
	void Awake() {
		GetComponent<Toggle> ().onValueChanged.AddListener (Toggled);
	}
	
	void Toggled(bool value) {
		view.AudioServices.PlaySFX (sfxString);
	}
}
