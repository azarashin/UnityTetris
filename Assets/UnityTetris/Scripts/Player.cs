using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityTetris.Interface;

namespace UnityTetris
{
    public class Player : MonoBehaviour, IPlayer
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
        private ISoundManager _sound;
        private bool _alive;
        private int _fallLevel; 

        public void Setup(BlockSet[] blockSetOptions, ISoundManager sound, int fallLevel)
        {
            Debug.Log("Player.RunGame");
            _alive = true;
            _blockSetOptions = blockSetOptions;
            _sound = sound; 
            _field.ResetField(_sound);
            _fallLevel =fallLevel; 
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
            _currentBlock.Setup(this, _field, _input, _sound, _fallLevel);
        }
    }
}