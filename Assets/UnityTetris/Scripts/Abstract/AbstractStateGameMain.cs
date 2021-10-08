using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityTetris;

namespace UnityTetris.Abstract
{
    public abstract class AbstractStateGameMain : MonoBehaviour
    {
        public abstract void Setup(GameController parent, Player[] players);
        public abstract void PlayerGameOver(Player player);

    }
}