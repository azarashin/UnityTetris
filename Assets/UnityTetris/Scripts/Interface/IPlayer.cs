using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityTetris.Abstract;

namespace UnityTetris.Interface
{
    public interface IPlayer
    {
        void Setup(AbstractField fieldPrefab, AbstractBlockSet[] blockSetPrefabOptions, ISoundManager sound, LevelController levelController);
        void StartGame(AbstractStateGameMain parent);
        bool IsAlive();
        void Dead();
        void PullNextBlock();
        void BlockSetHasBeenPlaced(); 
    }
}