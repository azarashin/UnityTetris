using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityTetris
{
    [Serializable]
    public class PlayerAndStatusPanel
    {
        [SerializeField]
        Player _player;

        [SerializeField]
        StatusPanel _statusPanel;

        public Player PlayerInstance
        {
            get
            {
                return _player; 
            }
        }

        public void Pack()
        {
            _player.SetStatusPanel(_statusPanel); 
        }
    }
}
