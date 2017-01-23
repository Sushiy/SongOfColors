using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MidiJack;
using UniRx;


public class MidiController : MonoBehaviour
{
    private Dictionary<int, int> IndexToPiano;

    private Dictionary<int, int> PianoToIndex;

    private const int NUMOFKEYS = 8;
    private int buttonOffset = 48;
    public GameObject audioParent;
    AudioSource[] audioSources;
    private int pressedButtonCount = 0;

    private void Start()
    {
        if (audioParent == null)
        {
            Debug.LogError("MidiController is lacking references");
            return;
        }
        audioSources = new AudioSource[NUMOFKEYS];
        IndexToPiano = new Dictionary<int, int>() { { 0, 0 }, { 1, 2 }, { 2, 4 }, { 3, 5 }, { 4, 7 }, { 5, 9 }, { 6, 11 }, { 7, 12 } };
        PianoToIndex = new Dictionary<int, int>() { { 0, 0 }, { 2, 1 }, { 4, 2 }, { 5, 3 }, { 7, 4 }, { 9, 5 }, { 11, 6 }, { 12, 7 } };
        for (int i = 0; i < NUMOFKEYS; i++)
        {
            audioSources[i] = audioParent.transform.GetChild(i).GetComponent<AudioSource>();
        }

        PlayerScript.instance.cPlaying.Subscribe(keyPlaying => OtherInput(keyPlaying, buttonOffset + 0));
        PlayerScript.instance.dPlaying.Subscribe(keyPlaying => OtherInput(keyPlaying, buttonOffset + 2));
        PlayerScript.instance.ePlaying.Subscribe(keyPlaying => OtherInput(keyPlaying, buttonOffset + 4));
        PlayerScript.instance.fPlaying.Subscribe(keyPlaying => OtherInput(keyPlaying, buttonOffset + 5));
        PlayerScript.instance.gPlaying.Subscribe(keyPlaying => OtherInput(keyPlaying, buttonOffset + 7));
        PlayerScript.instance.aPlaying.Subscribe(keyPlaying => OtherInput(keyPlaying, buttonOffset + 9));
        PlayerScript.instance.bPlaying.Subscribe(keyPlaying => OtherInput(keyPlaying, buttonOffset + 11));
        PlayerScript.instance.c2Playing.Subscribe(keyPlaying => OtherInput(keyPlaying, buttonOffset + 12));
    }

    void OtherInput(bool playing, int note)
    {
        if (playing)
            NoteOn(0, note, 1.0f);
        else
            NoteOff(0, note);
    }

    void NoteOn(MidiChannel channel, int note, float velocity)
    {
        AudioSource chosenaudio = audioSources[GetPianoToIndex(note - buttonOffset)];
        if (chosenaudio.isPlaying)
        {
            StopCoroutine(FadeOut(chosenaudio));
        }
        chosenaudio.pitch = Mathf.Pow(1.0594631f, note - (buttonOffset + NUMOFKEYS/2));
        chosenaudio.volume = 0.5f;
        chosenaudio.Play();
        ColorModelScript.instance.ActiveColor = (ColorModelScript.instance.getColorFromIndex(GetPianoToIndex(note - buttonOffset)));
        pressedButtonCount++;
    }

    void NoteOff(MidiChannel channel, int note)
    {
        pressedButtonCount = (pressedButtonCount - 1) < 0? 0:pressedButtonCount -1;
        StartCoroutine(FadeOut(audioSources[GetPianoToIndex(note - buttonOffset)]));
        if(pressedButtonCount <= 0)
            ColorModelScript.instance.ActiveColor = (ColorModelScript.Color.NONE);
    }

    void Knob(MidiChannel channel, int knobNumber, float knobValue)
    {
        //Debug.Log("Knob: " + knobNumber + "," + knobValue);
    }

    void OnEnable()
    {
        MidiMaster.noteOnDelegate += NoteOn;
        MidiMaster.noteOffDelegate += NoteOff;
        MidiMaster.knobDelegate += Knob;
    }

    void OnDisable()
    {
        MidiMaster.noteOnDelegate -= NoteOn;
        MidiMaster.noteOffDelegate -= NoteOff;
        MidiMaster.knobDelegate -= Knob;
    }

    int GetPianoToIndex(int i)
    {
        int im = i % 12;
        if (PianoToIndex.ContainsKey(im))
            return PianoToIndex[im];
        return 0;
    }

    int GetIndexToPiano(int i)
    {
        if (IndexToPiano.ContainsKey(i))
            return IndexToPiano[i];
        return 0;
    }
    
    IEnumerator FadeOut(AudioSource source)
    {
        float volume = source.volume;
        while(volume > 0)
        {
            volume -= 4.0f * Time.deltaTime;
            source.volume = volume;
            yield return null;
        }
    }
}
