using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityTetris.Abstract;
using UnityTetris.Interface;

namespace UnityTetris
{
    public class BlockSet : AbstractBlockSet
    {
        [SerializeField]
        private AudioSource _soundCollide;

        [SerializeField]
        private Block _prefabPart;

        [SerializeField]
        private Vector2Int[] _partsPositions;

        private Block[] _activeBlocks;
        private bool _falling;
        private AbstractField _field;
        private IPlayer _owner;
        private IInputManager _input;
        private ISoundManager _sound; 

        private int _rotStep;
        private Vector2Int _centerPos;
        private int _countWaitFalling;
        private int _countFalling;
        private bool _working; 


        public const float CountWaitFallingLimit = 4; // 最速でCountWaitFallingLimit フレームで落下する
        public float CountFallingLimit { get; private set; } // _countFalling がCountFallingLimit に達すると落下する

        private void FixedUpdate()
        {
            if(_working)
            {
                UpdateInput();
                Move();
                ShowBlocks();
            }
        }


        private void UpdateInput()
        {
            _input.UpdateState(); 

            if (_input.IsRotateRight())
            {
                RotateRight();
            }
            if (_input.IsRotateLeft())
            {
                RotateLeft();
            }
            if (_input.IsMoveRight())
            {
                if (MoveRight())
                {
                    _soundCollide.Play();
                }
            }
            if (_input.IsMoveLeft())
            {
                if (MoveLeft())
                {
                    _soundCollide.Play();
                }
            }
            if (_input.IsMoveDown())
            {
                MoveDown();
            }
        }
        private void RotateRight()
        {
            Rotate(1);
        }

        private void RotateLeft()
        {
            Rotate(-1);
        }

        /// <summary>
        /// ブロックを回転させる。
        /// </summary>
        /// <param name="dir">右回転の時は-1, 左回転の時は1</param>
        private void Rotate(int dir)
        {
            Vector2Int[] shift_list = new Vector2Int[]
            {
                new Vector2Int(0, 0),
                new Vector2Int(1, 0),
                new Vector2Int(-1, 0),
                new Vector2Int(2, 0),
                new Vector2Int(-2, 0),
                new Vector2Int(0, 1),
                new Vector2Int(1, 1),
                new Vector2Int(-1, 1),
                new Vector2Int(2, 1),
                new Vector2Int(-2, 1),
                new Vector2Int(0, 2),
                new Vector2Int(1, 2),
                new Vector2Int(-1, 2),
                new Vector2Int(2, 2),
                new Vector2Int(-2, 2),
            };
            int limit = 4;
            Vector2Int org = _centerPos;
            do
            {
                _rotStep = (_rotStep + dir + 4) % 4;
                foreach (Vector2Int shift in shift_list)
                {
                    _centerPos = org + shift;
                    if (!_field.IsHit(ToBlocks()))
                    {
                        return; // 回転成立
                    }
                }
                limit--;
            } while (limit > 0);
            _centerPos = org; // 位置を元に戻す。回転角は上記のループで４回９０度回転して元に戻っているはず。
        }

        /// <summary>
        /// 現在の姿勢における各ブロック要素を2次元座標の配列として返す
        /// </summary>
        /// <returns></returns>
        private Vector2Int[] ToBlocks()
        {
            return _partsPositions.Select(s => _centerPos + RotatePart(s)).ToArray();

        }

        /// <summary>
        /// 現在の回転角情報 _rotStep に合わせて座標p を原点中心に回転させる
        /// </summary>
        /// <param name="p">回転元の座標値</param>
        /// <returns>回転後の座標値</returns>
        private Vector2Int RotatePart(Vector2Int p)
        {
            int[] int_sin = new int[] { 0, 1, 0, -1 };
            int[] int_cos = new int[] { 1, 0, -1, 0 };

            return new Vector2Int(p.x * int_cos[_rotStep] - p.y * int_sin[_rotStep]
                , p.x * int_sin[_rotStep] + p.y * int_cos[_rotStep]);
        }

        private bool MoveRight()
        {
            Vector2Int org = _centerPos;
            _centerPos.x++;
            if (_field.IsHit(ToBlocks()))
            {
                _centerPos = org;
                _sound.Play(_soundCollide);
                return false;
            } else
            {
                _sound.Play(_soundCollide);
            }
            return true;
        }

        private bool MoveLeft()
        {
            Vector2Int org = _centerPos;
            _centerPos.x--;
            if (_field.IsHit(ToBlocks()))
            {
                _centerPos = org;
                _sound.Play(_soundCollide);
                return false;
            } else
            {
                _sound.Play(_soundCollide);
            }
            return true;
        }

        private void MoveDown()
        {
            _falling = true;
        }


        private void Move()
        {
            if (!NeedToFall())
            {
                return;
            }

            Vector2Int org = _centerPos;
            _centerPos.y--;

            Vector2Int[] pos = ToBlocks();
            if (_field.IsHit(pos))
            {
                _centerPos = org;

                pos = ToBlocks();
                for (int i = 0; i < _activeBlocks.Length; i++)
                {
                    _activeBlocks[i].Px = pos[i].x;
                    _activeBlocks[i].Py = pos[i].y;
                }
                _working = false; 
                StartCoroutine(PlaceBlock()); 

            }

        }

        private IEnumerator PlaceBlock()
        {
            for(int i=0;i< AbstractField.NumberOfFramesToStandByNextBlock; i++)
            {
                ShowBlocks();
                yield return new WaitForFixedUpdate();
            }
            // 回転し終わってない場合はここで回転を終わらせる
            transform.localRotation = Quaternion.Euler(0.0f, 0.0f, _rotStep * 90.0f);

            _owner.BlockSetHasBeenPlaced();

            if (_field.SetBlocks(_activeBlocks))
            {
                _owner.Dead();
            }
            else
            {
                _field.ReduceLines(_owner);
            }
            yield return null; 
        }

        private void ShowBlocks()
        {
            float rotSpeed = 0.2f; // これは適当な固定値にしておく
            float moveSpeed = 0.2f; 
            transform.localRotation = Quaternion.Lerp(
                transform.localRotation, Quaternion.Euler(0.0f, 0.0f, _rotStep * 90.0f), rotSpeed);
            transform.localPosition += (new Vector3(_centerPos.x, _centerPos.y) - transform.localPosition) * moveSpeed;
        }

        private bool NeedToFall()
        {
            _countWaitFalling++;
            if (_countWaitFalling >= CountWaitFallingLimit)
            {
                _countWaitFalling = 0;
                _countFalling++;
                if (_countFalling >= CountFallingLimit || _falling)
                {
                    _falling = false;
                    _countFalling = 0;
                    return true;

                }
            }
            return false;
        }

        public override void Setup(IPlayer owner, AbstractField field, IInputManager input, ISoundManager sound, int fallLevel)
        {
            _field = field;
            _owner = owner;
            _input = input;
            _sound = sound; 
            _falling = false;
            _working = true; 
            _rotStep = 0;
            _countWaitFalling = 0;
            _countFalling = 0;
            CountFallingLimit = fallLevel; 
            // _prefabPart を複製してブロックのパーツを構築する
            // localPostion のx, y が_partsPositions の座標と一致するようにインスタンスの座標を指定して生成する
            _activeBlocks = _partsPositions.Select(s =>
                Instantiate(_prefabPart,
                    transform.position + (new Vector3(s.x, s.y)), Quaternion.identity, transform) 
            ).ToArray();

            int miny = _partsPositions.Min(s => s.y);
            int maxy = _partsPositions.Max(s => s.y);
            int minx = _partsPositions.Min(s => s.x);
            int maxx = _partsPositions.Max(s => s.x);
            int y = 0;
            if (miny < 0)
            {
                y = -miny;
            }
            if(y < maxy)
            {
                y = maxy; 
            }
            if(y < -minx)
            {
                y = -minx;
            }
            if (y < maxx)
            {
                y = maxx;
            }
            int width = _field.Width();
            int height = _field.Height(); 
            _centerPos = new Vector2Int(width / 2, height - y - 1);

            if(-minx > width / 2 || maxx > width / 2)
            {
                Debug.LogError($"Size of block is too large. (width: {width}, block: {minx} - {maxx})");
            }
        }

        public override Vector2Int CenterPos()
        {
            return _centerPos;
        }

        public override int RotStep()
        {
            return _rotStep; 
        }
    }
}