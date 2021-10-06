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
        BlockSet bs = NewBlockSet("BlockSetA");
        IPlayer player = new StubPlayer();
        IField field = new StubField();
        IInputManager input = new StubInputManager();
        ISoundManager sound = new StubSoundManager(); 
        bs.Setup(player, field, input, sound); 
    }

    [UnityTest]
    public IEnumerator UnityTest001()
    {
        BlockSet bs = NewBlockSet("BlockSetA");
        StubPlayer player = new StubPlayer();
        StubField field = new StubField();
        StubInputManager input = new StubInputManager();
        StubSoundManager sound = new StubSoundManager();

        field.ReturnWidth = 10; // テスト用にフィールド幅を設定

        bs.Setup(player, field, input, sound);

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

        yield return null;
    }

    [UnityTest]
    public IEnumerator UnityTest002()
    {
        //    -1 0 1
        //  0   ooo
        //  1    o
        BlockSet bs = NewBlockSet("BlockSetB");
        StubPlayer player = new StubPlayer();
        StubField field = new StubField();
        StubInputManager input = new StubInputManager();
        StubSoundManager sound = new StubSoundManager();

        field.ReturnWidth = 10; // テスト用にフィールド幅を設定

        bs.Setup(player, field, input, sound);

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
