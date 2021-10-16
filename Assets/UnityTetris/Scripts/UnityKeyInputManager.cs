using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityTetris.Abstract;

namespace UnityTetris
{
    public class UnityKeyInputManager : InputManager
    {
        private const float _maxKeepTime = 0.20f;
        private const float _maxRepeatTime = 0.05f;
        private float _suspendTime = 0.0f;
        private int _repeat = 0;

        private bool _leftMove;
        private bool _rightMove;
        private bool _leftRotate;
        private bool _rightRotate;


        public override void StartGame()
        {
        }

        public override void UpdateState()
        {
            if (_suspendTime > 0.0f)
            {
                _suspendTime -= Time.deltaTime; 
            }

            _leftMove = Input.GetKey(KeyCode.A)
                || Input.GetKey(KeyCode.LeftArrow)
                || Input.GetKey(KeyCode.Alpha4);

            _rightMove = Input.GetKey(KeyCode.D)
                || Input.GetKey(KeyCode.RightArrow)
                || Input.GetKey(KeyCode.Alpha6);

            _leftRotate = Input.GetKey(KeyCode.Z)
                || Input.GetKey(KeyCode.Space)
                || Input.GetKey(KeyCode.Alpha1);

            _rightRotate = Input.GetKey(KeyCode.C)
                || Input.GetKey(KeyCode.UpArrow)
                || Input.GetKey(KeyCode.Alpha3);

            if (_leftMove || _rightMove)
            {
                if (_suspendTime <= 0.0f)
                {
                    if(_repeat <= 0)
                    {
                        _suspendTime = _maxKeepTime;
                        _repeat++;
                    }
                    else
                    {
                        _suspendTime = _maxRepeatTime;
                    }
                }
                else
                {
                    _leftMove = false;
                    _rightMove = false; 
                }
            }
            else if (_leftRotate || _rightRotate)
            {
                if (_suspendTime > 0.0f)
                {
                    _leftRotate = false;
                    _rightRotate = false;
                }
                _suspendTime = _maxKeepTime; // ボタンが離されるまでは回転させない
            }
            else
            {
                _repeat = 0;
                _suspendTime = 0;
            }
        }

        public override void FinishGame()
        {
        }

        public override bool IsMoveDown()
        {
            return Input.GetKey(KeyCode.S)
                || Input.GetKey(KeyCode.DownArrow)
                || Input.GetKey(KeyCode.Alpha5);
        }

        public override bool IsMoveLeft()
        {
            return _leftMove; 
        }

        public override bool IsMoveRight()
        {
            return _rightMove;
        }

        public override bool IsRotateLeft()
        {
            return _leftRotate;
        }

        public override bool IsRotateRight()
        {

            return _rightRotate;
        }
    }
}