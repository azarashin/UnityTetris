using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityTetris.Abstract;
using UnityTetris.Interface;

namespace UnityTetris
{
    public class Field : AbstractField
    {
        [SerializeField]
        int _width = 8;

        [SerializeField]
        int _height = 16;

        [SerializeField]
        int _borderLine = 14;

        [SerializeField]
        AudioSource _soundPlaced;

        [SerializeField]
        GameObject _prefabOutsideBlock; 

        private Block[,] _activeParts;
        private ISoundManager _sound;
        private IStatusPanel _statusPanel;
        private List<GameObject> _outsideBlocks = new List<GameObject>(); 

        public override void ResetField(IStatusPanel statusPanel, ISoundManager sound, int width = -1, int height = -1, int borderLine = -1)
        {
            _sound = sound;
            _statusPanel = statusPanel; 
            if (width > 0 && height > 0)
            {
                _width = width;
                _height = height;
            }

            Block[] removings = transform.GetComponentsInChildren<Block>(); 
            foreach(Block b in removings)
            {
                Destroy(b.gameObject); 
            }

            // マップを再生成
            _activeParts = new Block[_width, _height];

            if(borderLine != -1)
            {
                _borderLine = borderLine; 
            }
            foreach(GameObject obj in _outsideBlocks)
            {
                Destroy(obj); 
            }
            for(int y=0;y<= _borderLine; y++)
            {
                GameObject obj = Instantiate(_prefabOutsideBlock);
                obj.transform.parent = transform;
                obj.transform.localPosition = new Vector3(-1, y, 0);
                _outsideBlocks.Add(obj);

                obj = Instantiate(_prefabOutsideBlock);
                obj.transform.parent = transform;
                obj.transform.localPosition = new Vector3(_width, y, 0);
                _outsideBlocks.Add(obj);
            }
            for (int x = -1; x < _width + 1; x++)
            {
                GameObject obj = Instantiate(_prefabOutsideBlock);
                obj.transform.parent = transform;
                obj.transform.localPosition = new Vector3(x, -1, 0);
                _outsideBlocks.Add(obj);
            }

            _statusPanel.ResetScore(); 
        }

        public override int Width()
        {
            return _width;
        }

        public override int Height()
        {
            return _height;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="blocks"></param>
        /// <returns>ブロックが積みあがってしまったらtrue, そうでなければfalse を返す</returns>
        public override bool SetBlocks(Block[] blocks)
        {
            bool ret = false;
            _sound.Play(_soundPlaced); 
            // 設置できるかどうかを確認する
            foreach (Block b in blocks)
            {
                if (b.Py <= _borderLine && b.Px >= 0 && b.Py >= 0 && b.Px < _width && b.Py < _height)
                {
                    if (_activeParts[b.Px, b.Py] != null)
                    {
                        ret = true; 
                    }
                }
                else
                {
                    ret = true;
                }
            }
            if (ret)
            {
                return true;
            }
            // 実際に設置する
            foreach (Block b in blocks)
            {
                _activeParts[b.Px, b.Py] = b;
                b.transform.parent = transform;
            }
            return false;
        }

        public override bool IsHit(Vector2Int[] blocks)
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

        public string DebugField()
        {
            string line = "";
            for (int y=0;y<_height;y++)
            {
                for (int x = 0; x < _width; x++)
                {
                    if(_activeParts[x, _height - 1 - y] != null)
                    {
                        line += "*";
                    } else
                    {
                        line += "o";
                    }

                }
                line += "\r\n"; 
            }
            return line; 
        }

        public override void ReduceLines(IPlayer owner)
        {
            List<int> reducedIndex = new List<int>();
            Dictionary<int, int> reduceMap = new Dictionary<int, int>(); 
            int bottomIndex = 0; 
            // 削除判定をする
            for(int y=0;y<_activeParts.GetLength(1);y++)
            {
                bool reduce = true;
                for (int x = 0; x < _activeParts.GetLength(0); x++)
                {
                    if(_activeParts[x, y] == null)
                    {
                        reduce = false; 
                    }
                }
                if(reduce)
                {
                    reducedIndex.Add(y); 
                } else
                {
                    reduceMap[y] = bottomIndex; 
                    bottomIndex++; 
                }
            }

            if(reducedIndex.Count() == 0)
            {
                // 削除された行がないので、次のブロックを動かし始める
                owner.PullNextBlock();
            }
            else
            {
                StartCoroutine(CoReduceLines(owner, reducedIndex, reduceMap));
            }
        }

        private IEnumerator CoReduceLines(IPlayer owner, List<int> reducedIndex, Dictionary<int, int> reduceMap)
        {
            Dictionary<(int, int), Vector3> toPosition = new Dictionary<(int, int), Vector3>();
            // 削除するブロックの描画を止める
            foreach (int y in reducedIndex)
            {
                for (int x = 0; x < _activeParts.GetLength(0); x++)
                {
                    if (_activeParts[x, y] != null)
                    {
                        _activeParts[x, y].Remove();
                    }
                }
            }
            foreach (int y in reduceMap.Keys)
            {
                for (int x = 0; x < _activeParts.GetLength(0); x++)
                {
                    if (_activeParts[x, y] != null)
                    {
                        toPosition[(x, y)] = Vector3.Scale(_activeParts[x, y].transform.localPosition, Vector3.right + Vector3.forward) + Vector3.up * reduceMap[y];
                    }
                }
            }
            // ブロックを消し始めて少し待つ
            for (int i = 0; i < NumberOfFramesToReduce; i++)
            {
                yield return new WaitForFixedUpdate();
            }

            // ブロックをゆっくり下にスライドさせる
            float speed = 0.2f;
            for (int i=0;i<NumberOfFramesToSlide;i++)
            {
                float weight = ((float)i / (float)NumberOfFramesToSlide);
                foreach((int x, int y ) in toPosition.Keys)
                {
                    Vector3 cur = _activeParts[x, y].transform.localPosition;
                    Vector3 nxt = toPosition[(x, y)];
                    _activeParts[x, y].transform.localPosition = cur + (nxt - cur) * speed;
                }
                yield return new WaitForFixedUpdate(); 
            }
            foreach ((int x, int y) in toPosition.Keys)
            {
                _activeParts[x, y].transform.localPosition = toPosition[(x, y)];
            }

            // マップを更新する
            List<Block> refresh = new List<Block>();
            foreach ((int x, int y) in toPosition.Keys)
            {
                _activeParts[x, y].transform.localPosition = toPosition[(x, y)];
                _activeParts[x, y].Py = reduceMap[y];
                refresh.Add(_activeParts[x, y]); 
            }

            for (int y = 0; y < _activeParts.GetLength(1); y++)
            {
                for (int x = 0; x < _activeParts.GetLength(0); x++)
                {
                    _activeParts[x, y] = null; 
                }
            }

            foreach(Block b in refresh)
            {
                _activeParts[b.Px, b.Py] = b; 
            }

            _statusPanel.AddScore(reducedIndex.Count());

            owner.PullNextBlock();
            yield return null; 
        }

        public override int[,] GetFieldMap()
        {
            int[,] ret = new int[_width, _height]; 
            for(int y=0;y<_height;y++)
            {
                for (int x = 0; x < _width; x++)
                {
                    if(_activeParts[x, y] != null)
                    {
                        ret[x, y] = 1; 
                    }
                    else
                    {
                        ret[x, y] = 0;
                    }
                }
            }
            return ret; 
        }
    }
}