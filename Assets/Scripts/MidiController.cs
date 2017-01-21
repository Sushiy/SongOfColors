using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MidiJack;

public class MidiController : MonoBehaviour
{
    public bool[] buttonPressed;

    private void Start()
    {
        buttonPressed = new bool[30];
    }

    private void Update()
    {
        if (MidiMaster.GetKeyDown(0))
        {
            buttonPressed[0] = true;
        }
        /*
        for (int i = 0 ; i < 19; i++)
        {
            if(MidiMaster.GetKey(i + 30) > 0)
            {
                buttonPressed[i] = true;
            }
            else
            {
                buttonPressed[i] = false;
            }
        }*/
    }

    void NoteOn(MidiChannel channel, int note, float velocity)
    {
        Debug.Log("NoteOn: " + channel + "," + note + "," + velocity);
    }

    void NoteOff(MidiChannel channel, int note)
    {
       Debug.Log("NoteOff: " + channel + "," + note);
    }

    void Knob(MidiChannel channel, int knobNumber, float knobValue)
    {
        Debug.Log("Knob: " + knobNumber + "," + knobValue);
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
}
