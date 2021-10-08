using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityTetris;
using UnityTetris.Interface;

namespace UnityTetris.Abstract
{
    public abstract class AbstractStateGameMain : MonoBehaviour
    {
        public abstract void Setup(IGameController parent, IPlayer[] players);
        public abstract void PlayerGameOver(IPlayer player);

    }
}