using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityTetris.Interface;

namespace UnityTetris.Abstract
{
    public abstract class AbstractField : MonoBehaviour
    {

        public abstract void ResetField(ISoundManager sound, int width, int height, int borderLine);

        public abstract int Width();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="blocks"></param>
        /// <returns>ブロックが積みあがってしまったらtrue, そうでなければfalse を返す</returns>
        public abstract bool SetBlocks(Block[] blocks);

        public abstract bool IsHit(Vector2Int[] blocks);
    }
}
