using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class ColorComponent : MonoBehaviour {

	ColorModelScript colorModel;

	public ColorModelScript.Color ownColor;

	// Use this for initialization
	void Start () {
		ColorDeactivated();

		colorModel = ColorModelScript.instance;

		colorModel.activeColor
			.Where(x => ownColor == x)
			.Subscribe (x => ColorActivated())
			.AddTo(gameObject); 

		colorModel.oldColor
			.Where(x => ownColor == x)
			.Subscribe (x => ColorDeactivated())
			.AddTo(gameObject);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	protected void ColorActivated(){
		SpriteRenderer renderer = GetComponentInParent<SpriteRenderer>();
		renderer.color = Color.HSVToRGB ((int)ownColor / 360f, 1f, 1f);
	}

	protected void ColorDeactivated(){
		SpriteRenderer renderer = GetComponentInParent<SpriteRenderer>();
		renderer.color = new Color(0,0,0,0);
	}
}
