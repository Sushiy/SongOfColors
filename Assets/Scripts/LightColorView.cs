using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class LightColorView : MonoBehaviour
{
    public PlayerScript player;
    Light lightThis;
	// Use this for initialization
	void Start ()
    {
        lightThis = GetComponent<Light>();

        player.currentColor
        .Subscribe(currentColor => lightThis.color = currentColor);
	}
}
