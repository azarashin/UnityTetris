using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityTetris
{
    public class UnityKeyInputManager : InputManager
    {
        public override bool IsMoveDown()
        {
            return Input.GetKey(KeyCode.S)
                || Input.GetKey(KeyCode.DownArrow)
                || Input.GetKey(KeyCode.Alpha5);
        }

        public override bool IsMoveLeft()
        {
            return Input.GetKey(KeyCode.A)
                || Input.GetKey(KeyCode.LeftArrow)
                || Input.GetKey(KeyCode.Alpha4);
        }

        public override bool IsMoveRight()
        {
            return Input.GetKey(KeyCode.D)
                || Input.GetKey(KeyCode.RightArrow)
                || Input.GetKey(KeyCode.Alpha6);
        }

        public override bool IsRotateLeft()
        {
            return Input.GetKey(KeyCode.Z)
                || Input.GetKey(KeyCode.Space)
                || Input.GetKey(KeyCode.Alpha1);
        }

        public override bool IsRotateRight()
        {
            return Input.GetKey(KeyCode.C)
                || Input.GetKey(KeyCode.UpArrow)
                || Input.GetKey(KeyCode.Alpha3);
        }
    }
}