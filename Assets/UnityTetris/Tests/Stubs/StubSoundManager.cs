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

    public override void Play(AudioSource sound)
    {
        CallList += "Play\n";
    }

    public override void Stop(AudioSource sound)
    {
        CallList += "Stop\n";
    }
}
