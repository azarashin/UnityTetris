using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityTetris.Interface;

namespace UnityTetris
{
    public class StateGameBoot : MonoBehaviour
    {
        [SerializeField]
        int _countMax;

        [SerializeField]
        private Text _counter;

        private GameController _parent;

        public void Setup(GameController parent, IPlayer[] players, BlockSet[] blockSetOptions, ISoundManager sound)
        {
            Debug.Log("StateGameBoot.Setup");
            _parent = parent;
            foreach (var p in players)
            {
                p.Setup(blockSetOptions, sound);
            }
            StartCoroutine(CoRun());
        }

        private IEnumerator CoRun()
        {
            _counter.gameObject.SetActive(true);
            for (int i = 0; i < _countMax; i++)
            {
                _counter.text = (_countMax - i).ToString();
                yield return new WaitForSeconds(1.0f);
            }
            _counter.gameObject.SetActive(false);
            _parent.RunGame();
            yield return null;
        }

    }
}