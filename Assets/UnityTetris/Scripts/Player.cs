using System;
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
        private int _currentBlockId; 
        private ISoundManager _sound;
        private bool _alive;
        private List<int> _reservation;
        private LevelController _levelController;
        private IStatusPanel _statusPanel;

        public AbstractField Field { get { return _field; } }
        public IStatusPanel StatusPanel { get { return _statusPanel; } }

        public void Setup(AbstractField fieldPrefab, AbstractBlockSet[] blockSetOptions, ISoundManager sound, LevelController levelController)
        {
            Debug.Log("Player.Setup");
            if (_field != null)
            {
                Destroy(_field.gameObject);
            }

            _alive = true;
            _blockSetPrefabOptions = blockSetOptions;
            _sound = sound;
            _field = Instantiate<AbstractField>(fieldPrefab);
            _field.transform.parent = transform;
            _field.transform.localPosition = Vector3.zero; 
            _field.ResetField(_statusPanel, _sound, -1, -1, -1);
            _levelController = levelController; 

            _reservation = new List<int>();
            _reservation.Add(UnityEngine.Random.Range(0, _blockSetPrefabOptions.Length));
            _reservation.Add(UnityEngine.Random.Range(0, _blockSetPrefabOptions.Length));
            _statusPanel.UpdateReservation(_reservation);
            _statusPanel.UpdateLevel(_levelController.CurrentDisplayLevel());
        }

        public void SetStatusPanel(IStatusPanel statusPanel)
        {
            _statusPanel = statusPanel;
        }

        public void StartGame(AbstractStateGameMain parent)
        {
            Debug.Log("Player.StartGame");
            _parent = parent;
            _currentBlock = null;
            _input.StartGame(); 
            PullNextBlock();
        }

        public bool IsAlive()
        {
            return _alive;
        }

        public void Dead()
        {
            _input.FinishGame(); 
            _alive = false;
            _parent.PlayerGameOver(this);
        }

        public void BlockSetHasBeenPlaced()
        {
            if (_currentBlock != null)
            {
                Destroy(_currentBlock.gameObject);
            }
        }

        public void PullNextBlock()
        {
            _levelController.NextBlockHasBeenPulled();

            _reservation.Add(UnityEngine.Random.Range(0, _blockSetPrefabOptions.Length));
            _currentBlockId = _reservation[0];
            _reservation.RemoveAt(0);

            _statusPanel.UpdateReservation(_reservation);
            _statusPanel.UpdateLevel(_levelController.CurrentDisplayLevel()); 

            Debug.Log($"Player.PullNextBlock: id={_currentBlockId}");
            AbstractBlockSet next = _blockSetPrefabOptions[_currentBlockId];

            _currentBlock = Instantiate(next);
            _currentBlock.transform.parent = transform; 
            _currentBlock.Setup(this, _field, _input, _sound, _levelController.CurrentFallLevel());

        }

        public (int, int)[] Observe()
        {
            Vector2Int pos = _currentBlock.CenterPos();
            int rot = _currentBlock.RotStep();
            return new (int, int)[] {
                (pos.x, _field.Width()),
                (pos.y, _field.Height()),
                (rot, 4),
                (_currentBlockId, _blockSetPrefabOptions.Length)
            }; 
        }
    }
}