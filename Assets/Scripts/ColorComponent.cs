using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class ColorComponent : MonoBehaviour {

	ColorModelScript colorModel;

	public ColorModelScript.Color ownColor;

	// Use this for initialization
	void Start () {
		colorModel = ColorModelScript.instance;

		colorModel.activeColor
			/*.Where(x => ownColor == x)*/
			.Subscribe (x => OnColorChanged(x)); 
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnColorChanged(ColorModelScript.Color newColor){
		if (newColor == ownColor) {
			//show
		} else {
			//hide
		}
	}
}
