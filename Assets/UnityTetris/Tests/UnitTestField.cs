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
        StubSoundManager sound = new StubSoundManager();
        Field f = NewField();
        f.ResetField(sound, 4, 5);
        Assert.AreEqual(f.Width(), 4);
        f.ResetField(sound, 8, 9, 3);
        Assert.AreEqual(f.Width(), 8);
        GameObject.Destroy(f.gameObject); 
    }

    [Test]
    public void UnitTest002()
    {
        StubSoundManager sound = new StubSoundManager();
        Field f = NewField();
        f.ResetField(sound, 8, 9, 3);
        Block[] blocks = new Block[]
        {
            NewBlock(4, 4)
        };
        Assert.IsFalse(f.SetBlocks(blocks));

        // 同じところにブロックを重ねたら積み重なり判定される
        Block[] blocks2 = new Block[]
        {
            NewBlock(4, 4)
        };
        Assert.IsTrue(f.SetBlocks(blocks2));

        Block[] blocks3 = new Block[]
        {
            NewBlock(4, 3)
        };
        // ボーダーライン上の設置はセーフ
        Assert.IsFalse(f.SetBlocks(blocks3));

        Block[] blocks4 = new Block[]
        {
            NewBlock(4, 2)
        };
        // ボーダーラインより上の設置はアウト
        Assert.IsTrue(f.SetBlocks(blocks4));

        Block[] blocks5 = new Block[]
        {
            NewBlock(4, 3),
            NewBlock(4, 2)
        };
        // ボーダーラインより上の設置はアウト
        Assert.IsTrue(f.SetBlocks(blocks5));

        foreach (Block block in blocks)
        {
            Assert.AreNotEqual(null, block);
            GameObject.Destroy(block.gameObject);
        }
        foreach (Block block in blocks2)
        {
            Assert.AreNotEqual(null, block);
            GameObject.Destroy(block.gameObject);
        }
        foreach (Block block in blocks3)
        {
            Assert.AreNotEqual(null, block);
            GameObject.Destroy(block.gameObject);
        }
        foreach (Block block in blocks4)
        {
            Assert.AreNotEqual(null, block);
            GameObject.Destroy(block.gameObject);
        }
        foreach (Block block in blocks5)
        {
            Assert.AreNotEqual(null, block);
            GameObject.Destroy(block.gameObject);
        }

        GameObject.Destroy(f.gameObject);
    }

    [Test]
    public void UnitTest003()
    {
        StubSoundManager sound = new StubSoundManager();
        Field f = NewField();
        f.ResetField(sound, 8, 8, 3);
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

        Block[] blocks2 = new Block[]
        {
            NewBlock(2, 3),
            NewBlock(3, 3),
            NewBlock(2, 4),
            NewBlock(3, 4),
        };

        // 設置済みのブロックにこれから置こうとするブロックが重なるので置けないはず
        Assert.IsTrue(f.SetBlocks(blocks2));

        Debug.Log(f.DebugField());
        // 設置済みのブロックの配置は変わっていないはず。
        Assert.AreEqual(expected, f.DebugField());

        foreach (Block block in blocks)
        {
            Assert.AreNotEqual(null, block);
            GameObject.Destroy(block.gameObject);
        }
        foreach (Block block in blocks2)
        {
            Assert.AreNotEqual(null, block);
            GameObject.Destroy(block.gameObject);
        }
        GameObject.Destroy(f.gameObject);
    }


    [Test]
    public void UnitTest004()
    {
        Field f = NewField();
        StubSoundManager sound = new StubSoundManager();
        f.ResetField(sound, 8, 8, 3);
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

        Vector2Int[] nexts = new Vector2Int[]
        {
            new Vector2Int(2, 3),
            new Vector2Int(3, 3),
            new Vector2Int(2, 4),
            new Vector2Int(3, 4),
        };

        // 設置済みのブロックと衝突
        Assert.IsTrue(f.IsHit(nexts));

        nexts = new Vector2Int[]
                {
            new Vector2Int(2, 5),
            new Vector2Int(3, 5),
            new Vector2Int(2, 6),
            new Vector2Int(3, 6),
                };

        // 設置済みのブロックと衝突しない
        Assert.IsFalse(f.IsHit(nexts));

        nexts = new Vector2Int[]
                {
            new Vector2Int(2, 2),
            new Vector2Int(3, 2),
            new Vector2Int(2, 3),
            new Vector2Int(3, 3),
                };

        // 設置済みのブロックと衝突しない（ボーダーラインは超えている）
        Assert.IsFalse(f.IsHit(nexts));

        nexts = new Vector2Int[]
        {
            new Vector2Int(-1, 3),
            new Vector2Int(-1, 3),
            new Vector2Int(0, 4),
            new Vector2Int(0, 4),
        };

        // 外壁と衝突
        Assert.IsTrue(f.IsHit(nexts));

        nexts = new Vector2Int[]
        {
            new Vector2Int(7, 3),
            new Vector2Int(7, 3),
            new Vector2Int(8, 4),
            new Vector2Int(8, 4),
        };

        // 外壁と衝突
        Assert.IsTrue(f.IsHit(nexts));

        nexts = new Vector2Int[]
        {
            new Vector2Int(1, -1),
            new Vector2Int(1, -1),
            new Vector2Int(2, 0),
            new Vector2Int(2, 0),
        };

        // 外壁と衝突
        Assert.IsTrue(f.IsHit(nexts));

        nexts = new Vector2Int[]
        {
            new Vector2Int(1, 7),
            new Vector2Int(1, 7),
            new Vector2Int(2, 8),
            new Vector2Int(2, 8),
        };

        // 外壁と衝突
        Assert.IsTrue(f.IsHit(nexts));

        foreach(Block block in blocks)
        {
            Assert.AreNotEqual(null, block);
            GameObject.Destroy(block.gameObject); 
        }

        GameObject.Destroy(f.gameObject);
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

    private Field NewField()
    {
        GameObject prefab = Resources.Load<GameObject>("Prefabs/Field");
        GameObject obj = GameObject.Instantiate(prefab);

        Field ret = obj.GetComponent<Field>();
        return ret;

    }
}
