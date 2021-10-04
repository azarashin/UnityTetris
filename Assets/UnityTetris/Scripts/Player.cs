using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityTetris
{
    public class Player : MonoBehaviour
    {
        [SerializeField]
        private Field _field;

        [SerializeField]
        private InputManager _input;

        [SerializeField]
        AudioSource _soundCollide;

        private BlockSet[] _blockSetOptions;
        private StateGameMain _parent;
        private BlockSet _currentBlock;
        private bool _alive;

        public void Setup(BlockSet[] blockSetOptions)
        {
            Debug.Log("Player.RunGame");
            _alive = true;
            _blockSetOptions = blockSetOptions;
            _field.ResetField();
        }

        public void StartGame(StateGameMain parent)
        {
            _parent = parent;
            _currentBlock = null;
            PullNextBlock();
        }

        public bool IsAlive()
        {
            return _alive;
        }

        public void Dead()
        {
            _alive = false;
            _parent.PlayerGameOver(this);
        }

        public void PullNextBlock()
        {
            if (_currentBlock != null)
            {
                Destroy(_currentBlock.gameObject);
            }
            int id = Random.Range(0, _blockSetOptions.Length);
            Debug.Log($"Player.PullNextBlock: id={id}");
            BlockSet next = _blockSetOptions[id];

            _currentBlock = Instantiate(next);
            _currentBlock.Setup(this, _field, _input);
        }
    }
}