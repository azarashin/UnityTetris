using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityTetris;
using UnityTetris.Interface;

public class UnitTestBlockSet
{
    [Test]
    public void Test001()
    {
        int fallLevel = 4; 
        BlockSet bs = NewBlockSet("BlockSetA");
        IPlayer player = new StubPlayer();
        IField field = new StubField();
        IInputManager input = new StubInputManager();
        ISoundManager sound = new StubSoundManager(); 
        bs.Setup(player, field, input, sound, fallLevel);
        GameObject.Destroy(bs.gameObject);
    }

    [UnityTest]
    public IEnumerator UnityTest001() // 右回転
    {
        int fallLevel = 4;
        BlockSet bs = NewBlockSet("BlockSetA");
        StubPlayer player = new StubPlayer();
        StubField field = new StubField();
        StubInputManager input = new StubInputManager();
        StubSoundManager sound = new StubSoundManager();

        field.ReturnWidth = 10; // テスト用にフィールド幅を設定

        bs.Setup(player, field, input, sound, fallLevel);

        Assert.AreEqual("Width\n", field.CallList);
        // ブロックのテンプレートの配置が(0, -1), (0, 0), (0, 1), (0, 2) なので
        // あらゆる回転がなされてもブロックの各パーツの座標値が０以上になるよう補正され、ブロック全体が2つ下にずらされる
        // 個のテストケースではフィールド幅が10 なので、ブロック原点のx座標は中央の5 になるはず。
        Assert.AreEqual(new Vector2Int(5, 2), bs.CenterPos());
        field.ClearCallList();

        // 1回目右回転
        input.SetReturn(false, false, false, false, true);
        yield return new WaitForFixedUpdate();

        // 落下する前に縦棒を右回転させた状態を確認
        Assert.AreEqual(new Vector2Int(5, 2), bs.CenterPos());
        Assert.AreEqual("IsHit((6,2),(5,2),(4,2),(3,2))\n", field.CallList);
        field.ClearCallList();

        // 2回目右回転
        input.SetReturn(false, false, false, false, true);
        yield return new WaitForFixedUpdate();

        // 落下する前に縦棒を右回転させた状態を確認
        Assert.AreEqual(new Vector2Int(5, 2), bs.CenterPos());
        Assert.AreEqual("IsHit((5,3),(5,2),(5,1),(5,0))\n", field.CallList);
        field.ClearCallList();

        // 3回目右回転
        input.SetReturn(false, false, false, false, true);
        yield return new WaitForFixedUpdate();

        // 落下する前に縦棒を右回転させた状態を確認
        Assert.AreEqual(new Vector2Int(5, 2), bs.CenterPos());
        Assert.AreEqual("IsHit((4,2),(5,2),(6,2),(7,2))\n", field.CallList);
        field.ClearCallList();

        // 4回目右回転
        input.SetReturn(false, false, false, false, true);
        yield return new WaitForFixedUpdate();

        // 落下する前に縦棒を右回転させた状態を確認
        Assert.AreEqual(new Vector2Int(5, 2), bs.CenterPos());
        Assert.AreEqual("IsHit((5,1),(5,2),(5,3),(5,4))\n", field.CallList);
        field.ClearCallList();

        GameObject.Destroy(bs.gameObject);

        yield return null;
    }

    [UnityTest]
    public IEnumerator UnityTest002() // 左回転
    {
        //    -1 0 1
        //  0   ooo
        //  1    o
        int fallLevel = 4;
        BlockSet bs = NewBlockSet("BlockSetB");
        StubPlayer player = new StubPlayer();
        StubField field = new StubField();
        StubInputManager input = new StubInputManager();
        StubSoundManager sound = new StubSoundManager();

        field.ReturnWidth = 10; // テスト用にフィールド幅を設定

        bs.Setup(player, field, input, sound, fallLevel);

        Assert.AreEqual("Width\n", field.CallList);
        // ブロックのテンプレートの配置が(0, 0), (1, 0), (-1, 0), (0, 1) なので
        // あらゆる回転がなされてもブロックの各パーツの座標値が０以上になるよう補正され、ブロック全体が1つ下にずらされる
        // 個のテストケースではフィールド幅が10 なので、ブロック原点のx座標は中央の5 になるはず。
        Assert.AreEqual(new Vector2Int(5, 1), bs.CenterPos());
        field.ClearCallList();

        // 1回目左回転
        input.SetReturn(false, false, false, true, false);
        yield return new WaitForFixedUpdate();

        // 落下する前に凸ブロックを左回転させた状態を確認
        Assert.AreEqual(new Vector2Int(5, 1), bs.CenterPos());
        Assert.AreEqual("IsHit((5,1),(5,0),(5,2),(6,1))\n", field.CallList);
        field.ClearCallList();

        // 2回目左回転
        input.SetReturn(false, false, false, true, false);
        yield return new WaitForFixedUpdate();

        // 落下する前に凸ブロックを左回転させた状態を確認
        Assert.AreEqual(new Vector2Int(5, 1), bs.CenterPos());
        Assert.AreEqual("IsHit((5,1),(4,1),(6,1),(5,0))\n", field.CallList);
        field.ClearCallList();

        // 3回目左回転
        input.SetReturn(false, false, false, true, false);
        yield return new WaitForFixedUpdate();

        // 落下する前に凸ブロックを左回転させた状態を確認
        Assert.AreEqual(new Vector2Int(5, 1), bs.CenterPos());
        Assert.AreEqual("IsHit((5,1),(5,2),(5,0),(4,1))\n", field.CallList);
        field.ClearCallList();

        // 4回目左回転
        input.SetReturn(false, false, false, true, false);
        yield return new WaitForFixedUpdate();

        // 落下する前に凸ブロックを左回転させた状態を確認
        Assert.AreEqual(new Vector2Int(5, 1), bs.CenterPos());
        Assert.AreEqual("IsHit((5,1),(6,1),(4,1),(5,2))\n", field.CallList);
        field.ClearCallList();

        GameObject.Destroy(bs.gameObject);

        yield return null;
    }

    [UnityTest]
    public IEnumerator UnityTest003() // 左右移動
    {
        int fallLevel = 4;
        BlockSet bs = NewBlockSet("BlockSetC");
        StubPlayer player = new StubPlayer();
        StubField field = new StubField();
        StubInputManager input = new StubInputManager();
        StubSoundManager sound = new StubSoundManager();

        field.ReturnWidth = 10; // テスト用にフィールド幅を設定

        bs.Setup(player, field, input, sound, fallLevel);

        Assert.AreEqual("Width\n", field.CallList);
        // ブロックのテンプレートの配置が(0, 0), (0, 1), (1, 1), (1, 0) なので
        // あらゆる回転がなされてもブロックの各パーツの座標値が０以上になるよう補正され、ブロック全体が1つ下にずらされる
        // 個のテストケースではフィールド幅が10 なので、ブロック原点のx座標は中央の5 になるはず。
        Assert.AreEqual(new Vector2Int(5, 1), bs.CenterPos());
        field.ClearCallList();

        // 右移動
        input.SetReturn(false, false, true, false, false);
        yield return new WaitForFixedUpdate();

        // 落下する前に四角ブロックを右移動させた状態を確認
        Assert.AreEqual(new Vector2Int(6, 1), bs.CenterPos());
        Assert.AreEqual("IsHit((6,1),(6,2),(7,2),(7,1))\n", field.CallList);
        field.ClearCallList();

        // 右移動
        input.SetReturn(false, false, true, false, false);
        yield return new WaitForFixedUpdate();

        // 落下する前に四角ブロックを右移動させた状態を確認
        Assert.AreEqual(new Vector2Int(7, 1), bs.CenterPos());
        Assert.AreEqual("IsHit((7,1),(7,2),(8,2),(8,1))\n", field.CallList);
        field.ClearCallList();

        // 左移動
        input.SetReturn(false, true, false, false, false);
        yield return new WaitForFixedUpdate();

        // 落下する前に四角ブロックを左移動させた状態を確認
        Assert.AreEqual(new Vector2Int(6, 1), bs.CenterPos());
        Assert.AreEqual("IsHit((6,1),(6,2),(7,2),(7,1))\n", field.CallList);
        field.ClearCallList();

        // 左移動
        input.SetReturn(false, true, false, false, false);
        yield return new WaitForFixedUpdate();

        // 落下する前に四角ブロックを左移動させた状態を確認
        Assert.AreEqual(new Vector2Int(5, 1), bs.CenterPos());
        Assert.AreEqual("IsHit((5,1),(5,2),(6,2),(6,1))\n", field.CallList);
        field.ClearCallList();

        GameObject.Destroy(bs.gameObject);

        yield return null;
    }


    [UnityTest]
    public IEnumerator UnityTest004() // 下移動
    {
        int fallLevel = 4;
        BlockSet bs = NewBlockSet("BlockSetC");
        StubPlayer player = new StubPlayer();
        StubField field = new StubField();
        StubInputManager input = new StubInputManager();
        StubSoundManager sound = new StubSoundManager();

        field.ReturnWidth = 10; // テスト用にフィールド幅を設定

        bs.Setup(player, field, input, sound, fallLevel);

        Assert.AreEqual("Width\n", field.CallList);
        // ブロックのテンプレートの配置が(0, 0), (0, 1), (1, 1), (1, 0) なので
        // あらゆる回転がなされてもブロックの各パーツの座標値が０以上になるよう補正され、ブロック全体が1つ下にずらされる
        // 個のテストケースではフィールド幅が10 なので、ブロック原点のx座標は中央の5 になるはず。
        Assert.AreEqual(new Vector2Int(5, 1), bs.CenterPos());
        field.ClearCallList();

        // ---
        for (int i=0;i<BlockSet.CountWaitFallingLimit - 1;i++)
        {
            // 下移動
            input.SetReturn(true, false, false, false, false);
            yield return new WaitForFixedUpdate();

            // ボタンを押してもしばらくは移動しない
            Assert.AreEqual(new Vector2Int(5, 1), bs.CenterPos());
            Assert.AreEqual("", field.CallList);
            field.ClearCallList();
        }
        // 下移動
        input.SetReturn(true, false, false, false, false);
        yield return new WaitForFixedUpdate();

        // ボタンを押すとすぐに下に移動する
        Assert.AreEqual(new Vector2Int(5, 2), bs.CenterPos());
        Assert.AreEqual("IsHit((5,2),(5,3),(6,3),(6,2))\n", field.CallList);
        field.ClearCallList();

        // ---
        for (int i = 0; i < BlockSet.CountWaitFallingLimit - 1; i++)
        {
            // 下移動
            input.SetReturn(true, false, false, false, false);
            yield return new WaitForFixedUpdate();

            // ボタンを押してもしばらくは移動しない
            Assert.AreEqual(new Vector2Int(5, 2), bs.CenterPos());
            Assert.AreEqual("", field.CallList);
            field.ClearCallList();
        }
        // 下移動入力をしない（上記のループで下移動入力済みなので下移動するはず）
        input.SetReturn(false, false, false, false, false);
        yield return new WaitForFixedUpdate();

        // ボタンを押すとすぐに下に移動する
        Assert.AreEqual(new Vector2Int(5, 3), bs.CenterPos());
        Assert.AreEqual("IsHit((5,3),(5,4),(6,4),(6,3))\n", field.CallList);
        field.ClearCallList();

        // ---
        for (int i = 0; i < BlockSet.CountWaitFallingLimit - 1; i++)
        {
            // 下移動入力をしない
            input.SetReturn(false, false, false, false, false);
            yield return new WaitForFixedUpdate();

            Assert.AreEqual(new Vector2Int(5, 3), bs.CenterPos());
            Assert.AreEqual("", field.CallList);
            field.ClearCallList();
        }
        // 下移動入力をする（上記のループで下移動入力してないが、ここでの入力でギリギリ間に合うはず）
        input.SetReturn(true, false, false, false, false);
        yield return new WaitForFixedUpdate();

        // ボタンを押すとすぐに下に移動する
        Assert.AreEqual(new Vector2Int(5, 4), bs.CenterPos());
        Assert.AreEqual("IsHit((5,4),(5,5),(6,5),(6,4))\n", field.CallList);
        field.ClearCallList();

        // ---
        for (int i = 0; i < BlockSet.CountWaitFallingLimit - 1; i++)
        {
            // 下移動入力をしない
            input.SetReturn(false, false, false, false, false);
            yield return new WaitForFixedUpdate();

            Assert.AreEqual(new Vector2Int(5, 4), bs.CenterPos());
            Assert.AreEqual("", field.CallList);
            field.ClearCallList();
        }
        // 下移動入力をしない（移動しないはず）
        input.SetReturn(false, false, false, false, false);
        yield return new WaitForFixedUpdate();

        Assert.AreEqual(new Vector2Int(5, 4), bs.CenterPos());
        Assert.AreEqual("", field.CallList);
        field.ClearCallList();

        GameObject.Destroy(bs.gameObject); 

        yield return null;
    }

    private BlockSet NewBlockSet(string blockName)
    {
        GameObject prefab = Resources.Load<GameObject>("Prefabs/" + blockName);
        GameObject obj = GameObject.Instantiate(prefab);

        BlockSet ret = obj.GetComponent<BlockSet>();
        return ret;

    }
}
