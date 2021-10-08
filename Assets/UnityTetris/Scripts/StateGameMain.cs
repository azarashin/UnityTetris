using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityTetris.Abstract;

namespace UnityTetris
{
    public class StateGameMain : AbstractStateGameMain
    {
        private GameController _parent;
        private Player[] _players;

        public override void Setup(GameController parent, Player[] players)
        {
            Debug.Log("StateGameMain.Setup");
            _parent = parent;
            _players = players;
            foreach (var p in _players)
            {
                p.StartGame(this);
            }
        }

        public override void PlayerGameOver(Player player)
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