using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityTetris.Interface
{
    public interface IGameController
    {
        void BootGame();
        void RunGame();
        void FinishGame();
    }
}