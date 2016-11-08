﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using BibaFramework.BibaGame;

public class WinScreen : MonoBehaviour {

    public Text victoryText;
    public Image image;
    public BibaDevice bibaDevice;
    public GameController controller; 

    private Color green;
    private Color purple;
 
    void OnEnable(){
        green = new Color(113, 183, 68);
        purple = new Color(61, 50, 146);
    }
    	
	// Update is called once per frame
	void Update () {
        if (GameManager.Instance.victor == "Survivors") { victoryText.text = "SURVIVORS WIN!"; image.color = purple; } //LOCALIZATION controller.LocalizationService.GetText("copWin");
        else if (GameManager.Instance.victor == "Zombies") { victoryText.text = "ZOMBIES WIN!"; image.color = green; } //controller.LocalizationService.GetText("robberWin");
	}
}