using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityTetris.Interface
{
    abstract public class ISoundManager
    {
        public abstract void Play(AudioSource sound);
        public abstract void Stop(AudioSource sound);
    }
}
