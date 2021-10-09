using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityTetris.Interface;

namespace UnityTetris
{
    public class UnitySoundManager : MonoBehaviour, ISoundManager
    {
        public void Play(AudioSource sound)
        {
            sound.Play(); 
        }

        public void Stop(AudioSource sound)
        {
            sound.Stop(); 
        }
    }
}
