using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityTetris.Abstract;

namespace UnityTetris.Interface
{
    public interface IInputManager
    {
        void StartGame();
        void UpdateState();
        void FinishGame(); 

        bool IsRotateRight();
        bool IsRotateLeft();
        bool IsMoveRight();
        bool IsMoveLeft();
        bool IsMoveDown();
    }
}