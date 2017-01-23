using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class KeyBoardController : MonoBehaviour
{

    public static KeyBoardController instance;

    public ReactiveProperty<bool> cPlaying;
    public ReactiveProperty<bool> dPlaying;
    public ReactiveProperty<bool> ePlaying;
    public ReactiveProperty<bool> fPlaying;
    public ReactiveProperty<bool> gPlaying;
    public ReactiveProperty<bool> aPlaying;
    public ReactiveProperty<bool> bPlaying;
    public ReactiveProperty<bool> c2Playing;


    // Use this for initialization
    void Awake ()
    {
        instance = this;
        cPlaying = new ReactiveProperty<bool>(false);
        dPlaying = new ReactiveProperty<bool>(false);
        ePlaying = new ReactiveProperty<bool>(false);
        fPlaying = new ReactiveProperty<bool>(false);
        gPlaying = new ReactiveProperty<bool>(false);
        aPlaying = new ReactiveProperty<bool>(false);
        bPlaying = new ReactiveProperty<bool>(false);
        c2Playing = new ReactiveProperty<bool>(false);
    }
	
	// Update is called once per frame
	void Update ()
    {
        cPlaying.Value = GetAnyKey(new KeyCode[] { KeyCode.Q, KeyCode.A, KeyCode.Y });
        dPlaying.Value = GetAnyKey(new KeyCode[] { KeyCode.W, KeyCode.S, KeyCode.X });
        ePlaying.Value = GetAnyKey(new KeyCode[] { KeyCode.E, KeyCode.D, KeyCode.C });
        fPlaying.Value = GetAnyKey(new KeyCode[] { KeyCode.R, KeyCode.F, KeyCode.V });
        gPlaying.Value = GetAnyKey(new KeyCode[] { KeyCode.T, KeyCode.G, KeyCode.B });
        aPlaying.Value = GetAnyKey(new KeyCode[] { KeyCode.Z, KeyCode.H, KeyCode.N });
        bPlaying.Value = GetAnyKey(new KeyCode[] { KeyCode.U, KeyCode.J, KeyCode.M });
        c2Playing.Value = GetAnyKey(new KeyCode[] { KeyCode.I, KeyCode.K, KeyCode.Comma });

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            QuitGame();
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    bool GetAnyKey(KeyCode[] keys)
    {
        foreach (KeyCode key in keys)
        {
            if (Input.GetKey(key))
                return true;
        }
        return false;
    }
}
