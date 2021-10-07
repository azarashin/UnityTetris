using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityTetris.Abstract;

namespace UnityTetris.Interface
{
    public interface IPlayer
    {
        void Setup(AbstractBlockSet[] blockSetOptions, ISoundManager sound, int fallLevel);
        void StartGame(StateGameMain parent);
        bool IsAlive();
        void Dead();
        void PullNextBlock();
    }
}