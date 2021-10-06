using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityTetris.Interface
{
    public interface IBlockSet
    {
        void Setup(IPlayer owner, IField field, IInputManager input, ISoundManager sound);
    }
}