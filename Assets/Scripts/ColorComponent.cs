using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class ColorComponent : MonoBehaviour {

	ColorModelScript colorModel;
    MeshRenderer meshrenderer;
    Collider2D colliderThis;
    Light lightThis;

	public ColorModelScript.Color ownColor;

	Color currentColor;
	Color destColor;

	float lerpTime;
	float lerpDuration = 1f;

	bool fadingOut = false;

    private void Awake()
    {
        meshrenderer = GetComponent<MeshRenderer>();
        colliderThis = GetComponentInChildren<Collider2D>();
        lightThis = GetComponentInChildren<Light>();
    }

	// Use this for initialization
	void Start () {
		ColorDeactivated();
        
        lightThis.color = Color.HSVToRGB((float)ownColor / 360.0f, 1f, 1f); ;

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

	void Update()
	{
		if (lerpTime == 0f)
			return;
		if (!fadingOut) 
		{
			lerpTime -= Time.deltaTime;
			Mathf.Clamp (lerpTime, 0f, lerpDuration);
		}
		else 
		{
			lerpTime -= 5 * Time.deltaTime;
			if (lerpTime < 0.1f) 
			{
				meshrenderer.enabled = false;
				lightThis.enabled = false;
				colliderThis.gameObject.layer = 8;
                lerpTime = 0;
                fadingOut = false;
            }
		}

		currentColor = Color.Lerp (currentColor, destColor, 1f - lerpTime / lerpDuration);
        lightThis.color = Color.Lerp(currentColor, destColor, 1f - lerpTime / lerpDuration);
        meshrenderer.material.color = currentColor;
	}

	protected void ColorActivated()
    {
		currentColor = new Color(0, 0, 0);
		destColor = Color.HSVToRGB((float)ownColor/360.0f, 1f, 1f);
		lerpTime = lerpDuration;
		colliderThis.gameObject.layer = 0;
        meshrenderer.enabled = true;
        lightThis.enabled = true;
    }

    protected void ColorDeactivated()
    {
		fadingOut = true;
		destColor= new Color(0, 0, 0);
		lerpTime = lerpDuration;
    }
}
