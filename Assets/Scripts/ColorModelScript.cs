﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class ColorModelScript : MonoBehaviour {

	public enum Color : int //int values are Hue values of the hsv colors
	{
		NONE = 360, PURPLE = 303, BLUE = 236, CYAN = 187, GREEN = 102, YELLOW = 60, ORANGE = 39, RED = 0
	}
		
	//for global access
	public static ColorModelScript instance;

	public ReactiveProperty<Color> activeColor;

	public ReactiveProperty<Color> oldColor;

	public ReactiveProperty<float> frequence;

	Dictionary<Color,float> frequences;

	public void addFrequence(float f) {
		print (frequence.Value + f);
		setFrequence (frequence.Value + f);
	}

	public void setFrequence (float newFrequence) {
		frequence.Value = newFrequence;

		Color newColor = getColorOfFrequence(frequence.Value);

		if (newColor != activeColor.Value) {
			oldColor.Value = activeColor.Value;
			activeColor.Value = newColor;
		}
	}

	Color getColorOfFrequence(float f) {
		if (f < 0f) {
			return Color.NONE;
		} else if (f < (frequences[Color.PURPLE] + frequences[Color.BLUE]) / 2f) {
			return Color.PURPLE;
		} else if (f < (frequences[Color.BLUE] + frequences[Color.CYAN]) / 2f) {
			return Color.BLUE;
		} else if (f < (frequences[Color.CYAN] + frequences[Color.GREEN]) / 2f) {
			return Color.CYAN;
		} else if (f < (frequences[Color.GREEN] + frequences[Color.YELLOW]) / 2f) {
			return Color.GREEN;
		} else if (f < (frequences[Color.YELLOW] + frequences[Color.ORANGE]) / 2f) {
			return Color.YELLOW;
		} else if (f < (frequences[Color.ORANGE] + frequences[Color.RED]) / 2f) {
			return Color.ORANGE;
		} else {
			return Color.RED;
		}
	}

	public float getHueOfFrequence(float f) {
		float lowerBound = 0f, upperBound = 1024f;
		Color lowerColor = Color.NONE, upperColor = Color.NONE;

		//find the two colors/frequencies to interpolate
		foreach (KeyValuePair<Color, float> pair in frequences) {
			if (pair.Value > lowerBound && pair.Value <= f) {
				lowerBound = pair.Value;
				lowerColor = pair.Key;
			} 
			if (pair.Value < upperBound && pair.Value > lowerBound && pair.Value >= f) {
				upperBound = pair.Value;
				upperColor = pair.Key;
			}
		}

		//interpolate
		float a = (f - lowerBound) / (upperBound - lowerBound);
		float result = Mathf.Clamp(a * (float)upperColor + (1f - a) * (float)lowerColor, 0f, 360f);

		print (lowerColor + "," + upperColor + "," + result);

		return result;	
	}

	void Awake() {
		instance = this;

		activeColor = new ReactiveProperty<Color>(Color.NONE);
		oldColor = new ReactiveProperty<Color>(Color.NONE);
		frequence = new ReactiveProperty<float>(0f);
	}

	// Use this for initialization
	void Start () {
		frequences = new Dictionary<Color, float> ();
		frequences[Color.NONE] 		= -1f;
		frequences[Color.PURPLE] 	= 5f;
		frequences[Color.BLUE] 		= 15f;
		frequences[Color.CYAN] 		= 25f;
		frequences[Color.GREEN] 	= 35f;
		frequences[Color.YELLOW] 	= 45f;
		frequences[Color.ORANGE] 	= 55f;
		frequences[Color.RED] 		= 65f;
	}

	void OnColorChanged(){
		
	}


	// Update is called once per frame
	void Update () {
		
	}
}
