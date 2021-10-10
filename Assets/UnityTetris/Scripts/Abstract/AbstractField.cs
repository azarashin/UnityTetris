using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityTetris.Interface;

namespace UnityTetris.Abstract
{
    public abstract class AbstractField : MonoBehaviour
    {
        // ブロック設置後、ブロックが削除されない場合に待機するフレーム数
        public const int NumberOfFramesToStandByNextBlock = 30;

        // ブロックの削除後、残ったブロックがスライドするのに要するフレーム数
        public const int NumberOfFramesToSlide = 30;
        // フィールド上のブロックが削除されることにより次のブロックの発生が猶予されるフレーム数
        public const int NumberOfFramesToReduce = 30;

        public abstract void ResetField(IStatusPanel statusPanel, ISoundManager sound, int width, int height, int borderLine);

        public abstract int Width();
        public abstract int Height();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="blocks"></param>
        /// <returns>ブロックが積みあがってしまったらtrue, そうでなければfalse を返す</returns>
        public abstract bool SetBlocks(Block[] blocks);

        public abstract bool IsHit(Vector2Int[] blocks);

        public abstract void ReduceLines(IPlayer owner);
    }
}
