using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimatorView : MonoBehaviour
{
    PlayerScript playerScript;
    Animator animThis;
	// Use this for initialization
	void Start ()
    {
        ColorModelScript.instance.frequence
        .Subscribe(frequence => )
	}
	
    void SetFrequency(float f)
    {
        if (f > 0)
            f = f / 65;
        else
            f = 0;

        animThis.SetFloat("fNote", f);
    }
}
