using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityTetris.Abstract;

namespace UnityTetris.Interface
{
    public abstract class AbstractBlockSet : MonoBehaviour
    {
        public abstract void Setup(IPlayer owner, AbstractField field, IInputManager input, ISoundManager sound, int fallLevel);
    }
}