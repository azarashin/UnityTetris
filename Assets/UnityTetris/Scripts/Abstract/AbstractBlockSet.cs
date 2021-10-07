using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityTetris.Abstract;
using UnityTetris.Interface;

namespace UnityTetris.Abstract
{
    public abstract class AbstractBlockSet : MonoBehaviour
    {
        public abstract void Setup(IPlayer owner, AbstractField field, IInputManager input, ISoundManager sound, int fallLevel);
    }
}