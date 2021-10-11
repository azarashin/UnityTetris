using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityTetris.Abstract;
using UnityTetris.Interface;

namespace UnityTetris
{
    public class GameController : MonoBehaviour, IGameController
    {
        [SerializeField]
        private PlayerAndStatusPanel[] _players;

        [SerializeField]
        AbstractField _fieldPrefab;

        [SerializeField]
        private AbstractBlockSet[] _blockSetPrefabOptions;

        [SerializeField]
        private UnitySoundManager _sound; 

        [SerializeField]
        private StateGameBoot _boot;

        [SerializeField]
        private StateGameMain _main;

        [SerializeField]
        private StateGameFinish _finish;

        [Range(1, 32)]
        [SerializeField]
        private int _fallLevel = 16;

        private void Start()
        {
            BootGame();
        }

        public void BootGame()
        {
            Debug.Log("GameController.BootGame");
            foreach(PlayerAndStatusPanel psp in _players)
            {
                psp.Pack(); 
            }
            _boot.gameObject.SetActive(true);
            _main.gameObject.SetActive(false);
            _finish.gameObject.SetActive(false);
            Player[] players = _players.Select(s => s.PlayerInstance).ToArray(); 
            _boot.Setup(this, _fieldPrefab, players, _blockSetPrefabOptions, _sound, _fallLevel);
        }

        public void RunGame()
        {
            Debug.Log("GameController.RunGame");
            _boot.gameObject.SetActive(false);
            _main.gameObject.SetActive(true);
            _finish.gameObject.SetActive(false);
            Player[] players = _players.Select(s => s.PlayerInstance).ToArray();
            _main.Setup(this, players);
        }

        public void FinishGame()
        {
            Debug.Log("GameController.FinishGame");
            _boot.gameObject.SetActive(false);
            _main.gameObject.SetActive(false);
            _finish.gameObject.SetActive(true);
            _finish.Setup(this);
        }
    }
}