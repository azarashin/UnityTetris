using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityTetris.Interface;

namespace UnityTetris
{
    public class UnitySoundManager : ISoundManager
    {
        public override void Play(AudioSource sound)
        {
            sound.Play(); 
        }

        public override void Stop(AudioSource sound)
        {
            sound.Stop(); 
        }
    }
}
