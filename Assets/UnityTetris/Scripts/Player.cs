using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityTetris.Abstract;
using UnityTetris.Interface;

namespace UnityTetris
{
    public class Player : MonoBehaviour, IPlayer
    {
        [SerializeField]
        private InputManager _input;

        [SerializeField]
        AudioSource _soundCollide;

        private AbstractBlockSet[] _blockSetPrefabOptions;
        private AbstractStateGameMain _parent;
        private AbstractField _field = null;
        private AbstractBlockSet _currentBlock;
        private ISoundManager _sound;
        private bool _alive;
        private int _fallLevel; 

        public void Setup(AbstractField fieldPrefab, AbstractBlockSet[] blockSetOptions, ISoundManager sound, int fallLevel)
        {
            Debug.Log("Player.Setup");
            _alive = true;
            _blockSetPrefabOptions = blockSetOptions;
            _sound = sound;
            if(_field != null)
            {
                Destroy(_field); 
            }
            _field = Instantiate<AbstractField>(fieldPrefab);
            _field.transform.parent = transform; 
            _field.ResetField(_sound, -1, -1, -1);
            _fallLevel =fallLevel; 
        }

        public void StartGame(AbstractStateGameMain parent)
        {
            Debug.Log("Player.StartGame");
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
            int id = Random.Range(0, _blockSetPrefabOptions.Length);
            Debug.Log($"Player.PullNextBlock: id={id}");
            AbstractBlockSet next = _blockSetPrefabOptions[id];

            _currentBlock = Instantiate(next);
            _currentBlock.Setup(this, _field, _input, _sound, _fallLevel);
        }
    }
}