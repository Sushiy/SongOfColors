using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class ParticleColor : MonoBehaviour
{
    PlayerScript player;
    ParticleSystem particleSystem;
	// Use this for initialization
	void Start ()
    {
        player = GetComponentInParent<PlayerScript>();
        particleSystem = GetComponent<ParticleSystem>();
        player.currentColor
            .Subscribe(currentColor => ChangeColor(currentColor));

        ColorModelScript.instance.activeColor
        .Subscribe(x => OnColorChanged(x));
	}
	
    void OnColorChanged(ColorModelScript.Color c)
    {
        if (c == ColorModelScript.Color.NONE && particleSystem.isPlaying)
            particleSystem.Stop();
        if (c != ColorModelScript.Color.NONE/* && (!particleSystem.isPlaying || !particleSystem.IsAlive())*/)
            particleSystem.Play();
    }

    void ChangeColor(Color c)
    {
        particleSystem.startColor = c;
    }
}
