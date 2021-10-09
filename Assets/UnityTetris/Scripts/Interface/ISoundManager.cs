using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityTetris.Interface
{
    public interface ISoundManager
    {
        void Play(AudioSource sound);
        void Stop(AudioSource sound);
    }
}
