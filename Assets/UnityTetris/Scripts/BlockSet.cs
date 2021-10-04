using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BlockSet : MonoBehaviour
{
    [SerializeField]
    private AudioSource _soundCollide;

    [SerializeField]
    private Block _prefabPart;

    [SerializeField]
    private Vector2Int[] _partsPositions;

    private Block[] _activeBlocks;
    private bool _falling; 
    private Field _field;
    private Player _owner;
    private InputManager _input;

    private int _rotStep;
    private Vector2Int _centerPos; 

    private void FixedUpdate()
    {
        UpdateInput(); 
        Move();
        ShowBlocks(); 
    }


    private void UpdateInput()
    {
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
    /// �u���b�N����]������B
    /// </summary>
    /// <param name="dir">�E��]�̎���-1, ����]�̎���1</param>
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
            _rotStep = (_rotStep + dir) % 4;
            foreach (Vector2Int shift in shift_list)
            {
                _centerPos = org + shift; 
                if(!_field.IsHit(ToBlocks()))
                {
                    return; // ��]����
                }
            }
            limit--; 
        } while (limit > 0);
        _centerPos = org; // �ʒu�����ɖ߂��B��]�p�͏�L�̃��[�v�łS��X�O�x��]���Č��ɖ߂��Ă���͂��B
    }

    /// <summary>
    /// ���݂̎p���ɂ�����e�u���b�N�v�f��2�������W�̔z��Ƃ��ĕԂ�
    /// </summary>
    /// <returns></returns>
    private Vector2Int[] ToBlocks()
    {
        return _partsPositions.Select(s => _centerPos + RotatePart(s)).ToArray(); 

    }

    /// <summary>
    /// ���݂̉�]�p��� _rotStep �ɍ��킹�č��Wp �����_���S�ɉ�]������
    /// </summary>
    /// <param name="p">��]���̍��W�l</param>
    /// <returns>��]��̍��W�l</returns>
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
        if(_field.IsHit(ToBlocks()))
        {
            _centerPos = org;
            return false; 
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
            return false;
        }
        return true;
    }

    private void MoveDown()
    {
        _falling = true; 
    }


    private void Move()
    {
        if(!NeedToFall())
        {
            return; 
        }

        Vector2Int org = _centerPos;
        _centerPos.y++;

        Vector2Int[] pos = ToBlocks();
        if (_field.IsHit(pos))
        {
            _centerPos = org;

            pos = ToBlocks();
            for (int i=0;i<_activeBlocks.Length;i++)
            {
                _activeBlocks[i].Px = pos[i].x;
                _activeBlocks[i].Py = pos[i].y;
                _activeBlocks[i].transform.parent = _field.transform; 
            }

            if (_field.SetBlocks(_activeBlocks))
            {
                _owner.Dead(); 
            } else
            {
                _owner.PullNextBlock(); 
            }
        }

    }

    private void ShowBlocks()
    {
        float rotSpeed = 0.2f; // ����͓K���ȌŒ�l�ɂ��Ă���
        transform.localRotation = Quaternion.Lerp(
            transform.localRotation, Quaternion.Euler(0.0f, _rotStep * 90.0f, 0.0f), rotSpeed);
        transform.localPosition = new Vector3(_centerPos.x, -_centerPos.y); 
    }

    private int _countWaitFalling;
    private int _countFalling;

    private bool NeedToFall()
    {
        float countWaitFallingLimit = 4; // �ő���countWaitFallingLimit �t���[���ŗ�������
        float countFallingLimit = 4; // _countFalling ��countFallingLimit �ɒB����Ɨ�������
        _countWaitFalling++;
        if(_countWaitFalling >= countWaitFallingLimit)
        {
            _countWaitFalling = 0;
            _countFalling++;
            if (_countFalling >= countFallingLimit)
            {
                _falling = false;
                _countFalling = 0;
                Debug.Log("Fall");
                return true; 

            }
        }
        return false; 
    }

    public void Setup(Player owner, Field field, InputManager input)
    {
        _field = field;
        _owner = owner;
        _input = input;
        _falling = false;
        _rotStep = 0;
        // _prefabPart �𕡐����ău���b�N�̃p�[�c���\�z����
        // localPostion ��x, y ��_partsPositions �̍��W�ƈ�v����悤�ɃC���X�^���X�̍��W���w�肵�Đ�������
        _activeBlocks = _partsPositions.Select(s => 
            Instantiate(_prefabPart, 
                transform.position + (new Vector3(s.x, s.y)), Quaternion.identity, transform)
        ).ToArray();

        int miny = _partsPositions.Min(s => s.y);
        int y = 0;
        if (miny < 0)
        {
            y = -miny;
        }
        _centerPos = new Vector2Int(_field.Width() / 2, y);
    }
}
