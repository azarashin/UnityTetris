using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityTetris.Abstract;

namespace UnityTetris
{
    public class GameController : MonoBehaviour
    {
        [SerializeField]
        private Player[] _players;

        [SerializeField]
        private AbstractBlockSet[] _blockSetOptions;

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
            _boot.gameObject.SetActive(true);
            _main.gameObject.SetActive(false);
            _finish.gameObject.SetActive(false);
            _boot.Setup(this, _players, _blockSetOptions, _sound, _fallLevel);
        }

        public void RunGame()
        {
            Debug.Log("GameController.RunGame");
            _boot.gameObject.SetActive(false);
            _main.gameObject.SetActive(true);
            _finish.gameObject.SetActive(false);
            _main.Setup(this, _players);
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