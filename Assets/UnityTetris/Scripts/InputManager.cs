using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityTetris
{
    public abstract class InputManager : MonoBehaviour
    {
        public abstract bool IsRotateRight();
        public abstract bool IsRotateLeft();
        public abstract bool IsMoveRight();
        public abstract bool IsMoveLeft();
        public abstract bool IsMoveDown();
    }
}