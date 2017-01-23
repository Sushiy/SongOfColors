using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MidiJack;
using UnityEngine.SceneManagement;

public class TukanController : MidiController
{
    Image targetImage;

    public Sprite idle, purple, cyan, yellow;

    public ParticleSystem ps_Purple, ps_Cyan, ps_Yellow;

    public Image button1, button2, button3;

    float timeSinceLastInput = 0.0f;
    float randomTukanDelay = 1.33333f;
    public float randomTukanTime = 2.0f;

    int notePlaying = -1;

    private void Awake()
    {
        targetImage = GetComponent<Image>();
        StartCoroutine(PlayRandomTukan());
    }

    private void Update()
    {
        if (pressedButtonCount <= 0)
            timeSinceLastInput += Time.deltaTime;
    }

    IEnumerator PlayRandomTukan()
    {
        while(enabled)
        {
            yield return new WaitForSecondsRealtime(randomTukanDelay);

            Debug.Log("Looped at." + Time.time);
            if (notePlaying != -1)
            {
                NoteOff(0, notePlaying);
                notePlaying = -1;
            }
            else if (timeSinceLastInput >= randomTukanDelay)
            {
                int i = Random.Range(0, 9);
                if (i != 0 && i != 4 && i != 7)
                    yield return null;
                else
                {
                    NoteOn(0, i + buttonOffset, 1.0f);
                    notePlaying = i + buttonOffset;
                }
            }
        }
    }

    public void ChangeToScene(int i)
    {
        SceneManager.LoadScene(i);
    } 

    protected override void NoteOn(MidiChannel channel, int note, float velocity)
    {
        timeSinceLastInput = 0;
        StopCoroutine(PlayRandomTukan());
        AudioSource chosenaudio = audioSources[GetPianoToIndex(note - buttonOffset)];
        if (chosenaudio.isPlaying)
        {
            StopCoroutine(FadeOut(chosenaudio));
        }
        chosenaudio.pitch = Mathf.Pow(1.0594631f, note - (buttonOffset + NUMOFKEYS / 2));
        chosenaudio.volume = 0.5f;
        chosenaudio.Play();
        int actualNote = note - buttonOffset;
        if (actualNote == 0)
        {
            ps_Purple.Play();
            targetImage.sprite = purple;
            button1.color = new Color(1,0,1);
        }
        else if (actualNote == 4)
        {
            targetImage.sprite = cyan;
            ps_Cyan.Play();
            button2.color = new Color(0, 1, 1);
        }
        else if (actualNote == 7)
        {
            targetImage.sprite = yellow;
            ps_Yellow.Play();
            button3.color = new Color(1, 1, 0);
        }
        else
        {
            targetImage.sprite = idle;
            ps_Purple.Stop();
            ps_Cyan.Stop();
            ps_Yellow.Stop();
            button1.color = Color.gray;
            button2.color = Color.gray;
            button3.color = Color.gray;
        }
        pressedButtonCount++;
    }

    protected override void NoteOff(MidiChannel channel, int note)
    {
        pressedButtonCount = (pressedButtonCount - 1) < 0 ? 0 : pressedButtonCount - 1;
        StartCoroutine(FadeOut(audioSources[GetPianoToIndex(note - buttonOffset)]));

        int actualNote = note - buttonOffset;
        if (actualNote == 0)
        {
            ps_Purple.Stop();
            button1.color = Color.gray;
        }
        else if (actualNote == 4)
        {
            ps_Cyan.Stop();
            button2.color = Color.gray;
        }
        else if (actualNote == 7)
        {
            ps_Yellow.Stop();
            button3.color = Color.gray;
        }

        if (pressedButtonCount <= 0)
            targetImage.sprite = idle;
    }
}
