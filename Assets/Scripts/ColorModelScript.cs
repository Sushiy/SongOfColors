using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class ColorModelScript : MonoBehaviour {

	public enum Color
	{
		PURPLE, BLUE, GREEN, YELLOW, ORANGE, RED
	}
		
	public static ColorModelScript instance;

	public ReactiveProperty<Color> activeColor;

	public void setFrequence (float newFrequence) {
		if (newFrequence < 10f)
			activeColor = Color.PURPLE;
		else if (newFrequence < 20f)
			activeColor = Color.BLUE;
		else if (newFrequence < 30f)
			activeColor = Color.GREEN;
		else if (newFrequence < 40f)
			activeColor = Color.YELLOW;
		else if (newFrequence < 50f)
			activeColor = Color.ORANGE;
		else
			activeColor = Color.RED;
	}

	// Use this for initialization
	void Start () {
		instance = this;
		activeColor = new ReactiveProperty<Color> ();

		activeColor
			.Where(activeColor => ownColor == activeColor)
			.Subscribe (activeColor => OnColorChanged); 
	}

	OnColorChanged(Color)


	// Update is called once per frame
	void Update () {
		
	}
}
