using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityTetris.Abstract;
using UnityTetris.Interface;

namespace UnityTetris.AI
{
    public class MLInput : InputManager
    {
        [SerializeField]
        UnityKeyInputManager _heulisticInput;

        private bool _moveLeft;
        private bool _moveRight;
        private bool _moveDown;
        private bool _rotLeft;
        private bool _rotRight;

        public override void StartGame()
        {
            _heulisticInput.StartGame(); 
        }

        public override void UpdateState()
        {
            _heulisticInput.UpdateState(); 
            GetComponent<MLInputAgent>().RequestDecision();
        }

        public override void FinishGame()
        {
            _heulisticInput.FinishGame(); 
            GetComponent<MLInputAgent>().EndEpisode();
        }

        public override bool IsMoveDown()
        {
            return _moveDown;
        }

        public override bool IsMoveLeft()
        {
            return _moveLeft;
        }

        public override bool IsMoveRight()
        {
            return _moveRight;
        }

        public override bool IsRotateLeft()
        {
            return _rotLeft;
        }

        public override bool IsRotateRight()
        {
            return _rotRight;
        }

        internal void UpdateActions(int[] array)
        {
            _moveLeft = (array[0] == 1);
            _moveRight = (array[0] == 2);
            _moveDown = (array[0] == 3);
            _rotLeft = (array[0] == 4);
            _rotRight = (array[0] == 5);
        }
    }

}
