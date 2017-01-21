using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MidiJack;



public class MidiController : MonoBehaviour
{
    private Dictionary<int, int> IndexToPiano;

    private Dictionary<int, int> PianoToIndex;

    private const int NUMOFKEYS = 8;
    private int buttonOffset = 48;
    public GameObject audioParent;
    AudioSource[] audioSources;

    public bool[] buttonPressed;
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

        buttonPressed = new bool[NUMOFKEYS];
    }

    private void Update()
    {
        for (int i = 0 ; i < NUMOFKEYS; i++)
        {
            if(MidiMaster.GetKey(GetIndexToPiano(i) + buttonOffset) > 0)
            {
                buttonPressed[i] = true;
            }
            else
            {
                buttonPressed[i] = false;
            }
        }
    }

    void NoteOn(MidiChannel channel, int note, float velocity)
    {
        audioSources[GetPianoToIndex(note - buttonOffset)].pitch = Mathf.Pow(1.0594631f, note - (buttonOffset + NUMOFKEYS/2));
        audioSources[GetPianoToIndex(note - buttonOffset)].Play();
        pressedButtonCount++;
        //Debug.Log("NoteOn: " + channel + "," + note + "," + velocity);
    }

    void NoteOff(MidiChannel channel, int note)
    {
        pressedButtonCount = (pressedButtonCount - 1) < 0? 0:pressedButtonCount -1;
        audioSources[GetPianoToIndex(note - buttonOffset)].Stop();

        //Debug.Log("NoteOff: " + channel + "," + note);
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
        if (PianoToIndex.ContainsKey(i))
            return PianoToIndex[i];
        return 0;
    }

    int GetIndexToPiano(int i)
    {
        if (IndexToPiano.ContainsKey(i))
            return IndexToPiano[i];
        return 0;
    }
}
