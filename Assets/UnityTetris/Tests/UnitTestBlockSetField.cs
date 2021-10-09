using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityTetris;

public class UnitTestBlockSetField
{
    private const string SoundCollide = "null";
    private const string SoundMove = "null";
    private const string SoundPlaced = "null";

    [UnityTest]
    public IEnumerator UnitTestBlockSetFieldWithEnumeratorPasses001() // 左右移動
    {
        int fallLevel = 24; // 落下しないよう落下スピードを遅めにする
        int width = 10;
        int height = 10;
        int border = 7;
        BlockSet bs = NewBlockSet("BlockSetC");
        StubPlayer player = new StubPlayer();
        Field field = NewField(); 
        StubInputManager input = new StubInputManager();
        StubSoundManager sound = new StubSoundManager();

        field.ResetField(sound, width, height, border); 
        bs.Setup(player, field, input, sound, fallLevel);

        Assert.AreEqual(new Vector2Int(width / 2, 8), bs.CenterPos());

        for (int i = width / 2 + 1; i < width - 1; i++)
        {
            // 右移動
            input.SetReturn(false, false, true, false, false);
            yield return new WaitForFixedUpdate();

            // 落下する前に四角ブロックを右移動させた状態を確認
            Assert.AreEqual(new Vector2Int(i, 8), bs.CenterPos());
            Assert.AreEqual($"Play({SoundMove})\n", sound.CallList); // 移動音
            sound.ClearCallList();
            Assert.AreEqual("", player.CallList);
            player.ClearCallList();
        }

        // 右移動
        input.SetReturn(false, false, true, false, false);
        yield return new WaitForFixedUpdate();

        // ブロックを移動させようとして外壁にぶつかる
        Assert.AreEqual(new Vector2Int(8, 8), bs.CenterPos());
        Assert.AreEqual($"Play({SoundCollide})\n", sound.CallList); // 衝突音
        sound.ClearCallList();
        Assert.AreEqual("", player.CallList);
        player.ClearCallList();

        for (int i = width - 2 - 1; i >= 0; i--)
        {
            // 左移動
            input.SetReturn(false, true, false, false, false);
            yield return new WaitForFixedUpdate();

            // 落下する前に四角ブロックを左移動させた状態を確認
            Assert.AreEqual(new Vector2Int(i, 8), bs.CenterPos());
            Assert.AreEqual($"Play({SoundMove})\n", sound.CallList); // 移動音
            sound.ClearCallList();
            Assert.AreEqual("", player.CallList);
            player.ClearCallList();
        }

        // 左移動
        input.SetReturn(false, true, false, false, false);
        yield return new WaitForFixedUpdate();

        // ブロックを移動させようとして外壁にぶつかる
        Assert.AreEqual(new Vector2Int(0, 8), bs.CenterPos());
        Assert.AreEqual($"Play({SoundCollide})\n", sound.CallList); // 衝突音
        sound.ClearCallList();
        Assert.AreEqual("", player.CallList);
        player.ClearCallList();

        GameObject.Destroy(bs.gameObject);
        GameObject.Destroy(field.gameObject);

        yield return null;
    }

    [UnityTest]
    public IEnumerator UnitTestBlockSetFieldWithEnumeratorPasses002() // 下移動(一番下まで自由落下)
    {
        int fallLevel = 4;
        int width = 10;
        int height = 10;
        int border = 7;
        BlockSet bs = NewBlockSet("BlockSetC");
        StubPlayer player = new StubPlayer();
        Field field = NewField();
        StubInputManager input = new StubInputManager();
        StubSoundManager sound = new StubSoundManager();

        field.ResetField(sound, width, height, border);

        bs.Setup(player, field, input, sound, fallLevel);

        // ブロックのテンプレートの配置が(0, 0), (0, 1), (1, 1), (1, 0) なので
        // あらゆる回転がなされてもブロックの各パーツの座標値が０以上かつフィールドの高さ未満になるよう補正され、ブロック全体が1つ下にずらされる
        // 個のテストケースではフィールド幅が10 なので、ブロック原点のx座標は中央の5 になるはず。
        Assert.AreEqual(new Vector2Int(width / 2, 8), bs.CenterPos());

        input.SetReturn(false, false, false, false, false);

        // ---
        for (int k = 1; k < height - 1; k++) // 設置直前まで自由落下させてみる
        {
            for (int i = 0; i < fallLevel * BlockSet.CountWaitFallingLimit - 1; i++)
            {
                yield return new WaitForFixedUpdate();

                // しばらくは移動しない
                Assert.AreEqual(new Vector2Int(width / 2, height - 1 - k), bs.CenterPos());
                Assert.AreEqual("", sound.CallList); // 落下中音はならない
                sound.ClearCallList();
                Assert.AreEqual("", player.CallList);
                player.ClearCallList();
            }

            // 下移動
            yield return new WaitForFixedUpdate();

            Assert.AreEqual(new Vector2Int(width / 2, height - 2 - k), bs.CenterPos());
            Assert.AreEqual("", sound.CallList); // 落下中音はならない
            sound.ClearCallList();
            Assert.AreEqual("", player.CallList);
            player.ClearCallList();
        }

        for (int i = 0; i < fallLevel * BlockSet.CountWaitFallingLimit - 1; i++)
        {
            yield return new WaitForFixedUpdate();

            // しばらくは移動しない
            Assert.AreEqual(new Vector2Int(width / 2, 0), bs.CenterPos());
            Assert.AreEqual("", sound.CallList); // 落下中音はならない
            sound.ClearCallList();
            Assert.AreEqual("", player.CallList);
            player.ClearCallList();
        }

        // 下移動しようとするが、衝突してしまい、移動できずにとどまる
        yield return new WaitForFixedUpdate();

        Assert.AreEqual(new Vector2Int(width / 2, 0), bs.CenterPos());
        Assert.AreEqual($"Play({SoundPlaced})\n", sound.CallList); // 移動音
        sound.ClearCallList();
        Assert.AreEqual("PullNextBlock\n", player.CallList);
        player.ClearCallList();

        GameObject.Destroy(bs.gameObject);
        GameObject.Destroy(field.gameObject); 

        yield return null;
    }

    [UnityTest]
    public IEnumerator UnitTestBlockSetFieldWithEnumeratorPasses003() // 下移動(一番下まで自由落下)
    {
        int fallLevel = 24;
        int width = 10;
        int height = 10;
        int border = 7;
        BlockSet bs = NewBlockSet("BlockSetD");
        StubPlayer player = new StubPlayer();
        Field field = NewField();
        StubInputManager input = new StubInputManager();
        StubSoundManager sound = new StubSoundManager();

        field.ResetField(sound, width, height, border);

        bs.Setup(player, field, input, sound, fallLevel);
        yield return MoveBlock(bs, player, input, fallLevel, 1, 8);
        int limit = 60 * 5;
        bool pulled = false; 
        for(int i=0;i<limit;i++)
        {
            if(player.CallList == "PullNextBlock\n")
            {
                pulled = true;
                break; 
            }
            yield return new WaitForFixedUpdate(); 
        }
        GameObject.Destroy(bs.gameObject);
        Assert.AreEqual("PullNextBlock\n", player.CallList);
        Assert.IsTrue(pulled);
        player.ClearCallList();
        Debug.Log(field.DebugField());

        bs = NewBlockSet("BlockSetD");
        bs.Setup(player, field, input, sound, fallLevel);
        yield return MoveBlock(bs, player, input, fallLevel, 0, 8);
        limit = 60 * 5;
        pulled = false;
        for (int i = 0; i < limit; i++)
        {
            if (player.CallList == "PullNextBlock\n")
            {
                pulled = true;
                break;
            }
            yield return new WaitForFixedUpdate();
        }
        GameObject.Destroy(bs.gameObject);
        Assert.IsTrue(pulled);
        player.ClearCallList();
        Debug.Log(field.DebugField());

        bs = NewBlockSet("BlockSetD");
        bs.Setup(player, field, input, sound, fallLevel);
        yield return MoveBlock(bs, player, input, fallLevel, -1, 8);
        limit = 60 * 5;
        pulled = false;
        for (int i = 0; i < limit; i++)
        {
            if (player.CallList == "PullNextBlock\n")
            {
                pulled = true;
                break;
            }
            yield return new WaitForFixedUpdate();
        }
        Assert.IsTrue(pulled);
        player.ClearCallList();
        Debug.Log(field.DebugField());

        GameObject.Destroy(bs.gameObject);
        GameObject.Destroy(field.gameObject);

        yield return null;
    }

    private IEnumerator MoveBlock(BlockSet block, StubPlayer player, StubInputManager input, int fallLevel, int x, int y)
    {
        if(x < 0)
        {
            for(int i=0;i<-x;i++)
            {
                input.SetReturn(false, true, false, false, false); 
                yield return new WaitForFixedUpdate();
            }
        } else if(x > 0)
        {
            for (int i = 0; i < x; i++)
            {
                input.SetReturn(false, false, true, false, false);
                yield return new WaitForFixedUpdate();
            }
        }
        for (int i = 0; i < fallLevel * BlockSet.CountWaitFallingLimit - 1; i++)
        {
            input.SetReturn(true, false, false, false, false);
            yield return new WaitForFixedUpdate();
            if ("PullNextBlock\n" == player.CallList || "Dead\n" == player.CallList) {
                break;
            }
        }
        input.SetReturn(false, false, false, false, false);
    }

    private BlockSet NewBlockSet(string blockName)
    {
        BlockSet prefab = Resources.Load<BlockSet>("UnityTetris/Prefabs/BlockSet/" + blockName);
        BlockSet obj = GameObject.Instantiate(prefab);

        return obj; 

    }

    private Field NewField()
    {
        Field prefab = Resources.Load<Field>("UnityTetris/Prefabs/Field");
        Field obj = GameObject.Instantiate(prefab);

        return obj;

    }
}
