using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityTetris; 


    public class UnitTestField
{
    [Test]
    public void UnitTest001()
    {
        Field f = new Field();
        f.ResetField(4, 5);
        Assert.AreEqual(f.Width(), 4);
        f.ResetField(8, 9, 3);
        Assert.AreEqual(f.Width(), 8);
    }

    [Test]
    public void UnitTest002()
    {
        Field f = new Field();
        f.ResetField(8, 9, 3);
        Block[] blocks = new Block[]
        {
            NewBlock(4, 4)
        };
        Assert.IsFalse(f.SetBlocks(blocks));

        // �����Ƃ���Ƀu���b�N���d�˂���ςݏd�Ȃ蔻�肳���
        blocks = new Block[]
        {
            NewBlock(4, 4)
        };
        Assert.IsTrue(f.SetBlocks(blocks));

        blocks = new Block[]
        {
            NewBlock(4, 3)
        };
        // �{�[�_�[���C����̐ݒu�̓Z�[�t
        Assert.IsFalse(f.SetBlocks(blocks));

        blocks = new Block[]
        {
            NewBlock(4, 2)
        };
        // �{�[�_�[���C������̐ݒu�̓A�E�g
        Assert.IsTrue(f.SetBlocks(blocks));
    }

    private Block NewBlock(int x, int y)
    {
        GameObject prefab = Resources.Load<GameObject>("Prefabs/Block");
        GameObject obj = GameObject.Instantiate(prefab); 

        Block ret = obj.GetComponent<Block>();
        ret.Px = x;
        ret.Py = y;
        return ret; 

    }
}
