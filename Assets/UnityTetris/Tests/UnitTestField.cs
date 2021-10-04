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

        // 同じところにブロックを重ねたら積み重なり判定される
        blocks = new Block[]
        {
            NewBlock(4, 4)
        };
        Assert.IsTrue(f.SetBlocks(blocks));

        blocks = new Block[]
        {
            NewBlock(4, 3)
        };
        // ボーダーライン上の設置はセーフ
        Assert.IsFalse(f.SetBlocks(blocks));

        blocks = new Block[]
        {
            NewBlock(4, 2)
        };
        // ボーダーラインより上の設置はアウト
        Assert.IsTrue(f.SetBlocks(blocks));

        blocks = new Block[]
        {
            NewBlock(4, 3),
            NewBlock(4, 2)
        };
        // ボーダーラインより上の設置はアウト
        Assert.IsTrue(f.SetBlocks(blocks));
    }

    [Test]
    public void UnitTest003()
    {
        Field f = new Field();
        f.ResetField(8, 8, 3);
        Block[] blocks = new Block[]
        {
            NewBlock(0, 4),
            NewBlock(1, 4),
            NewBlock(2, 4),
            NewBlock(3, 4),
            NewBlock(7, 7),
            NewBlock(6, 7),
            NewBlock(5, 7),
            NewBlock(4, 7),
        };

        Assert.IsFalse(f.SetBlocks(blocks)); 

        string expected = @"oooooooo
oooooooo
oooooooo
oooooooo
****oooo
oooooooo
oooooooo
oooo****
";
        Debug.Log(f.DebugField()); 
        // 設置済みのブロックの配置を確認。
        Assert.AreEqual(expected, f.DebugField());

        blocks = new Block[]
        {
            NewBlock(2, 3),
            NewBlock(3, 3),
            NewBlock(2, 4),
            NewBlock(3, 4),
        };

        // 設置済みのブロックにこれから置こうとするブロックが重なるので置けないはず
        Assert.IsTrue(f.SetBlocks(blocks));

        Debug.Log(f.DebugField());
        // 設置済みのブロックの配置は変わっていないはず。
        Assert.AreEqual(expected, f.DebugField()); 
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
