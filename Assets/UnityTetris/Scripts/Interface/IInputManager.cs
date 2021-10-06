using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityTetris.Interface
{
    public interface IInputManager
    {
        bool IsRotateRight();
        bool IsRotateLeft();
        bool IsMoveRight();
        bool IsMoveLeft();
        bool IsMoveDown();
    }
}