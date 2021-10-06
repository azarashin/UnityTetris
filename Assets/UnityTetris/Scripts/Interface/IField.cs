using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityTetris.Interface
{
    public interface IField
    {

        void ResetField(int width, int height, int borderLine);

        int Width();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="blocks"></param>
        /// <returns>�u���b�N���ς݂������Ă��܂�����true, �����łȂ����false ��Ԃ�</returns>
        bool SetBlocks(Block[] blocks);

        bool IsHit(Vector2Int[] blocks);
        Transform RefTransform(); 
    }
}
