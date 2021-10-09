using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityTetris.Interface;

public class StubSoundManager : ISoundManager
{
    public string CallList { get; private set; } = "";

    public void ClearCallList()
    {
        CallList = "";
    }

    public void Play(AudioSource sound)
    {
        if(sound.clip == null)
        {
            CallList += "Play(null)\n";
        } else
        {
            CallList += $"Play({sound.clip.name})\n";
        }
    }

    public void Stop(AudioSource sound)
    {
        if (sound.clip == null)
        {
            CallList += "Stop(null)\n";
        }
        else
        {
            CallList += $"Stop({sound.clip.name})\n";
        }
    }
}
