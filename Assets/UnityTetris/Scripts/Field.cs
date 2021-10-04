using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityTetris
{
    public class Field : MonoBehaviour
    {
        [SerializeField]
        int _width = 8;

        [SerializeField]
        int _height = 16;

        private Block[,] _activeParts;

        public void ResetField(int width = -1, int height = -1)
        {
            if (width > 0 && height > 0)
            {
                _width = width;
                _height = height;
            }
            _activeParts = new Block[_width, _height];
        }

        public int Width()
        {
            return _width;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="blocks"></param>
        /// <returns>ブロックが積みあがってしまったらtrue, そうでなければfalse を返す</returns>
        public bool SetBlocks(Block[] blocks)
        {
            bool ret = false;
            foreach (Block b in blocks)
            {
                if (b.Px >= 0 && b.Py >= 0 && b.Px < _width && b.Py < _height)
                {
                    _activeParts[b.Px, b.Py] = b;
                }
                else
                {
                    ret = true;
                }
            }
            return ret;
        }

        public bool IsHit(Vector2Int[] blocks)
        {
            foreach (Vector2Int b in blocks)
            {
                if (b.x >= 0 && b.y >= 0 && b.x < _width && b.y < _height)
                {
                    if (_activeParts[b.x, b.y] != null)
                    {
                        // フィールドの内部である。既にその場所にブロックが存在していたら衝突したと判定する
                        return true;
                    }
                }
                else
                {
                    // フィールドの外にはみ出た部分なので衝突したものとみなす
                    return true;
                }
            }
            return false;
        }
    }
}