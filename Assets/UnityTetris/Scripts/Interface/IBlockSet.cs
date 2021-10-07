using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityTetris.Abstract;

namespace UnityTetris.Interface
{
    public interface IBlockSet
    {
        void Setup(IPlayer owner, AbstractField field, IInputManager input, ISoundManager sound, int fallLevel);
    }
}