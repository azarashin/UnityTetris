using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityTetris.Interface
{
    public interface IPlayer
    {
        void Setup(BlockSet[] blockSetOptions, ISoundManager sound);
        void StartGame(StateGameMain parent);
        bool IsAlive();
        void Dead();
        void PullNextBlock();
    }
}