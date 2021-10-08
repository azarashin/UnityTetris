using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityTetris.Abstract;
using UnityTetris.Interface;

namespace UnityTetris
{
    public class StateGameMain : AbstractStateGameMain
    {
        private IGameController _parent;
        private IPlayer[] _players;

        public override void Setup(IGameController parent, IPlayer[] players)
        {
            Debug.Log("StateGameMain.Setup");
            _parent = parent;
            _players = players;
            foreach (var p in _players)
            {
                p.StartGame(this);
            }
        }

        public override void PlayerGameOver(IPlayer player)
        {
            if (GetNumberOfAlivingPlayer() <= 1)
            {
                _parent.FinishGame();
            }
        }

        private int GetNumberOfAlivingPlayer()
        {
            return _players.Where(s => s.IsAlive()).Count();
        }
    }
}