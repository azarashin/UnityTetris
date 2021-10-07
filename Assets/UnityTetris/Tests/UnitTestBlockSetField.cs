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
    public IEnumerator UnityTest001() // 左右移動
    {
        int fallLevel = 24; // 落下しないよう落下スピードを遅めにする
        BlockSet bs = NewBlockSet("BlockSetC");
        StubPlayer player = new StubPlayer();
        Field field = NewField(); 
        StubInputManager input = new StubInputManager();
        StubSoundManager sound = new StubSoundManager();

        field.ResetField(sound, 10, 10, 2); 
        bs.Setup(player, field, input, sound, fallLevel);

        // Field のWidth が10 なので、ブロックの初期x 座標値は5
        Assert.AreEqual(new Vector2Int(5, 1), bs.CenterPos());

        for (int i = 6; i < 9; i++)
        {
            // 右移動
            input.SetReturn(false, false, true, false, false);
            yield return new WaitForFixedUpdate();

            // 落下する前に四角ブロックを右移動させた状態を確認
            Assert.AreEqual(new Vector2Int(i, 1), bs.CenterPos());
            Assert.AreEqual($"Play({SoundMove})\n", sound.CallList); // 移動音
            sound.ClearCallList();
            Assert.AreEqual("", player.CallList);
            player.ClearCallList();
        }

        // 右移動
        input.SetReturn(false, false, true, false, false);
        yield return new WaitForFixedUpdate();

        // ブロックを移動させようとして外壁にぶつかる
        Assert.AreEqual(new Vector2Int(8, 1), bs.CenterPos());
        Assert.AreEqual($"Play({SoundCollide})\n", sound.CallList); // 衝突音
        sound.ClearCallList();
        Assert.AreEqual("", player.CallList);
        player.ClearCallList();

        for (int i = 7; i >= 0; i--)
        {
            // 左移動
            input.SetReturn(false, true, false, false, false);
            yield return new WaitForFixedUpdate();

            // 落下する前に四角ブロックを左移動させた状態を確認
            Assert.AreEqual(new Vector2Int(i, 1), bs.CenterPos());
            Assert.AreEqual($"Play({SoundMove})\n", sound.CallList); // 移動音
            sound.ClearCallList();
            Assert.AreEqual("", player.CallList);
            player.ClearCallList();
        }

        // 左移動
        input.SetReturn(false, true, false, false, false);
        yield return new WaitForFixedUpdate();

        // ブロックを移動させようとして外壁にぶつかる
        Assert.AreEqual(new Vector2Int(0, 1), bs.CenterPos());
        Assert.AreEqual($"Play({SoundCollide})\n", sound.CallList); // 衝突音
        sound.ClearCallList();
        Assert.AreEqual("", player.CallList);
        player.ClearCallList();

        GameObject.Destroy(bs.gameObject);
        GameObject.Destroy(field.gameObject);

        yield return null;
    }

    [UnityTest]
    public IEnumerator UnityTest002() // 下移動(一番下まで自由落下)
    {
        int fallLevel = 4;
        BlockSet bs = NewBlockSet("BlockSetC");
        StubPlayer player = new StubPlayer();
        Field field = NewField();
        StubInputManager input = new StubInputManager();
        StubSoundManager sound = new StubSoundManager();

        field.ResetField(sound, 10, 10, 2);

        bs.Setup(player, field, input, sound, fallLevel);

        // ブロックのテンプレートの配置が(0, 0), (0, 1), (1, 1), (1, 0) なので
        // あらゆる回転がなされてもブロックの各パーツの座標値が０以上になるよう補正され、ブロック全体が1つ下にずらされる
        // 個のテストケースではフィールド幅が10 なので、ブロック原点のx座標は中央の5 になるはず。
        Assert.AreEqual(new Vector2Int(5, 1), bs.CenterPos());

        input.SetReturn(false, false, false, false, false);

        // ---
        for (int k = 1; k < 8; k++) // 設置直前まで自由落下させてみる
        {
            for (int i = 0; i < fallLevel * BlockSet.CountWaitFallingLimit - 1; i++)
            {
                yield return new WaitForFixedUpdate();

                // しばらくは移動しない
                Assert.AreEqual(new Vector2Int(5, k), bs.CenterPos());
                Assert.AreEqual("", sound.CallList); // 落下中音はならない
                sound.ClearCallList();
                Assert.AreEqual("", player.CallList);
                player.ClearCallList();
            }

            // 下移動
            yield return new WaitForFixedUpdate();

            Assert.AreEqual(new Vector2Int(5, 1 + k), bs.CenterPos());
            Assert.AreEqual("", sound.CallList); // 落下中音はならない
            sound.ClearCallList();
            Assert.AreEqual("", player.CallList);
            player.ClearCallList();
        }

        for (int i = 0; i < fallLevel * BlockSet.CountWaitFallingLimit - 1; i++)
        {
            yield return new WaitForFixedUpdate();

            // しばらくは移動しない
            Assert.AreEqual(new Vector2Int(5, 8), bs.CenterPos());
            Assert.AreEqual("", sound.CallList); // 落下中音はならない
            sound.ClearCallList();
            Assert.AreEqual("", player.CallList);
            player.ClearCallList();
        }

        // 下移動しようとするが、衝突してしまい、移動できずにとどまる
        yield return new WaitForFixedUpdate();

        Assert.AreEqual(new Vector2Int(5, 8), bs.CenterPos());
        Assert.AreEqual($"Play({SoundPlaced})\n", sound.CallList); // 移動音
        sound.ClearCallList();
        Assert.AreEqual("PullNextBlock\n", player.CallList);
        player.ClearCallList();

        GameObject.Destroy(bs.gameObject);

        yield return null;
    }


    private BlockSet NewBlockSet(string blockName)
    {
        GameObject prefab = Resources.Load<GameObject>("UnityTetris/Prefabs/" + blockName);
        GameObject obj = GameObject.Instantiate(prefab);

        BlockSet ret = obj.GetComponent<BlockSet>();
        return ret;

    }

    private Field NewField()
    {
        GameObject prefab = Resources.Load<GameObject>("UnityTetris/Prefabs/Field");
        GameObject obj = GameObject.Instantiate(prefab);

        Field ret = obj.GetComponent<Field>();
        return ret;

    }
}
