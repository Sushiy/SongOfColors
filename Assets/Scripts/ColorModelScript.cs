using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class ColorModelScript : MonoBehaviour {

	public enum Color : int //int values are Hue values of the hsv colors
	{
		NONE = -1, PURPLE = 303, BLUE = 236, CYAN = 187, GREEN = 102, YELLOW = 60, ORANGE = 39, RED = 0
	}
		
	//for global access
	public static ColorModelScript instance;

	public ReactiveProperty<Color> activeColor;

	public ReactiveProperty<Color> oldColor;

	float frequence;

	public void addFrequence(float f) {
		print (frequence + f);
		setFrequence (frequence + f);
	}

	public void setFrequence (float newFrequence) {
		frequence = newFrequence;

		Color newColor;

		if (newFrequence < 0f) {
			newColor = Color.NONE;
		} else if (newFrequence < 10f) {
			newColor = Color.PURPLE;
		} else if (newFrequence < 20f) {
			newColor = Color.BLUE;
		} else if (newFrequence < 30f) {
			newColor = Color.CYAN;
		} else if (newFrequence < 40f) {
			newColor = Color.GREEN;
		} else if (newFrequence < 50f) {
			newColor = Color.YELLOW;
		} else if (newFrequence < 60f) {
			newColor = Color.ORANGE;
		} else {
			newColor = Color.RED;
		}

		if (newColor != activeColor.Value) {
			oldColor.Value = activeColor.Value;
			activeColor.Value = newColor;
		}
	}

	void Awake() {
		instance = this;

		activeColor = new ReactiveProperty<Color>(Color.NONE);
		oldColor = new ReactiveProperty<Color>(Color.NONE);
	}

	// Use this for initialization
	void Start () {
		frequence = 0;
	}

	void OnColorChanged(){
		
	}


	// Update is called once per frame
	void Update () {
		
	}
}
