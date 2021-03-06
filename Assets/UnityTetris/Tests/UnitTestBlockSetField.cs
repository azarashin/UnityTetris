using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityTetris;
using UnityTetris.Abstract;

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
        StubStatusPanel statusPanel = new StubStatusPanel();

        field.ResetField(statusPanel, sound, width, height, border);
        Assert.AreEqual("ResetScore\n", statusPanel.CallList);
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
        StubStatusPanel statusPanel = new StubStatusPanel(); 

        field.ResetField(statusPanel, sound, width, height, border);
        Assert.AreEqual("ResetScore\n", statusPanel.CallList);

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
        for (int i = 0; i < AbstractField.NumberOfFramesToStandByNextBlock; i++)
        {
            yield return new WaitForFixedUpdate();
        }

        Assert.AreEqual(new Vector2Int(width / 2, 0), bs.CenterPos());
        Assert.AreEqual($"Play({SoundPlaced})\n", sound.CallList); // 移動音
        sound.ClearCallList();
        Assert.AreEqual("BlockSetHasBeenPlaced\nPullNextBlock\n", player.CallList); // この時点でPullNextBlock は呼び出されない。
        player.ClearCallList();

        GameObject.Destroy(bs.gameObject); // この時点でBlockSet のインスタンスは破壊される
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
        StubPlayer player = new StubPlayer();
        Field field = NewField();
        StubInputManager input = new StubInputManager();
        StubSoundManager sound = new StubSoundManager();
        StubStatusPanel statusPanel = new StubStatusPanel();
        string expected; 

        field.ResetField(statusPanel, sound, width, height, border);
        Assert.AreEqual("ResetScore\n", statusPanel.CallList);

        yield return MoveBlock("BlockSetD", true, new (int, int)[] {
            (1, 8),
        }, field, sound, player, input, fallLevel);
        expected = @"oooooooooo
oooooooooo
oooooooooo
oooooooooo
oooooooooo
oooooooooo
oooooooooo
oooooooooo
oooooo**oo
ooooo**ooo
";
        Debug.Log(field.DebugField());
        Assert.AreEqual(expected, field.DebugField());

        yield return MoveBlock("BlockSetD", true, new (int, int)[] {
            (0, 8),
        }, field, sound, player, input, fallLevel);
        expected = @"oooooooooo
oooooooooo
oooooooooo
oooooooooo
oooooooooo
oooooooooo
oooooooooo
ooooo**ooo
oooo****oo
ooooo**ooo
";
        Debug.Log(field.DebugField());
        Assert.AreEqual(expected, field.DebugField());

        yield return MoveBlock("BlockSetD", true, new (int, int)[] {
            (-1, 8),
        }, field, sound, player, input, fallLevel);
        expected = @"oooooooooo
oooooooooo
oooooooooo
oooooooooo
oooooooooo
oooooooooo
oooo**oooo
ooo****ooo
oooo****oo
ooooo**ooo
";
        Debug.Log(field.DebugField());
        Assert.AreEqual(expected, field.DebugField());

        GameObject.Destroy(field.gameObject);

        yield return null;
    }

    [UnityTest]
    public IEnumerator UnitTestBlockSetFieldWithEnumeratorPasses004() // ブロックを消す
    {
        int fallLevel = 24;
        int width = 6;
        int height = 8;
        int border = 5;
        StubPlayer player = new StubPlayer();
        Field field = NewField();
        StubInputManager input = new StubInputManager();
        StubSoundManager sound = new StubSoundManager();
        StubStatusPanel statusPanel = new StubStatusPanel();
        string expected;

        field.ResetField(statusPanel, sound, width, height, border);
        Assert.AreEqual("ResetScore\n", statusPanel.CallList);
        statusPanel.ClearCallList();

        yield return MoveBlock("BlockSetC", true, new (int, int)[] {
            (1, 8),
        }, field, sound, player, input, fallLevel);
        expected = @"oooooo
oooooo
oooooo
oooooo
oooooo
oooooo
oooo**
oooo**
";
        Debug.Log(field.DebugField());
        Assert.AreEqual(expected, field.DebugField());

        yield return MoveBlock("BlockSetC", true, new (int, int)[] {
            (-3, 8),
        }, field, sound, player, input, fallLevel);
        expected = @"oooooo
oooooo
oooooo
oooooo
oooooo
oooooo
**oo**
**oo**
";
        Debug.Log(field.DebugField());
        Assert.AreEqual(expected, field.DebugField());

        yield return MoveBlock("BlockSetC", true, new (int, int)[] {
            (-3, 8),
        }, field, sound, player, input, fallLevel);
        expected = @"oooooo
oooooo
oooooo
oooooo
**oooo
**oooo
**oo**
**oo**
";
        Debug.Log(field.DebugField());
        Assert.AreEqual(expected, field.DebugField());

        Assert.AreEqual("", statusPanel.CallList); // この時点でステータスパネルに更新なし。
        yield return MoveBlock("BlockSetC", true, new (int, int)[] {
            (-1, 8),
        }, field, sound, player, input, fallLevel);
        expected = @"oooooo
oooooo
oooooo
oooooo
oooooo
oooooo
**oooo
**oooo
";
        Debug.Log(field.DebugField());
        Assert.AreEqual(expected, field.DebugField());
        Assert.AreEqual("AddScore(2)\n", statusPanel.CallList);

        GameObject.Destroy(field.gameObject);

        yield return null;
    }


    /// <summary>
    /// ブロックを生成し、落としてフィールドに設置させる
    /// </summary>
    /// <param name="blockName">ブロックのprefab名</param>
    /// <param name="pull">このブロックが設置されるべきであればtrue, 設置されずに積みあがったと判定されるべきであればfalse</param>
    /// <param name="field"></param>
    /// <param name="sound"></param>
    /// <param name="player"></param>
    /// <param name="input"></param>
    /// <param name="fallLevel"></param>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    private IEnumerator MoveBlock(string blockName, bool pull, (int, int)[] tasks, Field field, StubSoundManager sound, StubPlayer player, StubInputManager input, int fallLevel)
    {
        // 横移動の量はBlockSet.CountWaitFallingLimit 未満でなければ移動中に下に落下してしまう。
        // このことを想定してテストシナリオを記述すること

        BlockSet bs = NewBlockSet(blockName);
        bs.Setup(player, field, input, sound, fallLevel);

        bool placed = false; 
        foreach ((int x, int y) in tasks)
        {
            if (x < 0)
            {
                for (int i = 0; i < -x; i++)
                {
                    input.SetReturn(false, true, false, false, false);
                    yield return new WaitForFixedUpdate();
                    if(player.CallList != "")
                    {
                        placed = true;
                        break; 
                    }
                }
            }
            else if (x > 0)
            {
                for (int i = 0; i < x; i++)
                {
                    input.SetReturn(false, false, true, false, false);
                    yield return new WaitForFixedUpdate();
                    if (player.CallList != "")
                    {
                        placed = true;
                        break;
                    }
                }
            }
            if(placed)
            {
                break; 
            }

            for (int i = 0; i < y * (BlockSet.CountWaitFallingLimit + 1); i++)
            {
                input.SetReturn(true, false, false, false, false);
                yield return new WaitForFixedUpdate();

                if (player.CallList != "")
                {
                    placed = true;
                    break;
                }

            }
            if (placed)
            {
                break;
            }

        }
        Assert.AreEqual("", player.CallList); // この時点では何もイベントは発生していないはず


        if(!placed)
        {
            for (int i = 0; i < AbstractField.NumberOfFramesToStandByNextBlock; i++) // 落下してイベントが発生するまで十分な時間待つ
            {
                yield return new WaitForFixedUpdate();
                if (player.CallList != "")
                {
                    Debug.Log(i); 
                    placed = true;
                    break;
                }

            }
        }

        input.SetReturn(false, false, false, false, false);

        if (player.CallList != "")
        {
            Debug.Log(player.CallList);
        }
        if (bs != null && player.CallList == "BlockSetHasBeenPlaced\n") // ブロックが消されてしばらく待っている場合
        {
            player.ClearCallList();

            for (int i = 0; i < AbstractField.NumberOfFramesToReduce + AbstractField.NumberOfFramesToSlide; i++)
            {
                yield return new WaitForFixedUpdate();
                if (player.CallList == "PullNextBlock\n")
                {
                    Debug.Log(i);
                    player.ClearCallList();
                    GameObject.Destroy(bs.gameObject);
                    Assert.IsTrue(pull);
                    yield break;
                }

            }
        }

        if (bs != null && player.CallList == "BlockSetHasBeenPlaced\nPullNextBlock\n")
        {
            player.ClearCallList();
            GameObject.Destroy(bs.gameObject);
            Assert.IsTrue(pull);
            yield break;
        }
        if (bs != null && player.CallList == "BlockSetHasBeenPlaced\nDead\n")
        {
            player.ClearCallList();
            GameObject.Destroy(bs.gameObject);
            Assert.IsFalse(pull);
            yield break;
        }


        Assert.Fail(); // イベントが発生していなかったらこのテストケースは失敗

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
