using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityTetris;
using UnityTetris.Abstract;
using UnityTetris.Interface;

public class UnitTestBlockSet
{
    private const string SoundCollide = "null";
    private const string SoundMove = "null";

    [Test]
    public void UnitTestStateGameMainSimplePasses001()
    {
        int fallLevel = 4; 
        BlockSet bs = NewBlockSet("BlockSetA");
        IPlayer player = new StubPlayer();
        AbstractField field = new StubField();
        IInputManager input = new StubInputManager();
        ISoundManager sound = new StubSoundManager(); 
        bs.Setup(player, field, input, sound, fallLevel);
        GameObject.Destroy(bs.gameObject);
    }

    [UnityTest]
    public IEnumerator UnitTestStateGameMainWithEnumeratorPasses001() // 右回転
    {
        int fallLevel = 4;
        BlockSet bs = NewBlockSet("BlockSetA");
        StubPlayer player = new StubPlayer();
        StubField field = new StubField();
        StubInputManager input = new StubInputManager();
        StubSoundManager sound = new StubSoundManager();

        field.ReturnWidth = 10; // テスト用にフィールド幅を設定
        field.ReturnHeight = 10; // テスト用にフィールド高さを設定

        bs.Setup(player, field, input, sound, fallLevel);

        Assert.AreEqual("Width\nHeight\n", field.CallList);
        // ブロックのテンプレートの配置が(0, -1), (0, 0), (0, 1), (0, 2) なので
        // あらゆる回転がなされてもブロックの各パーツの座標値が０以上かつフィールドの高さ未満になるよう補正され、ブロック全体が2つ下にずらされる
        // 個のテストケースではフィールド幅が10 なので、ブロック原点のx座標は中央の5 になるはず。
        Assert.AreEqual(new Vector2Int(5, 7), bs.CenterPos());
        field.ClearCallList();

        // 1回目右回転
        input.SetReturn(false, false, false, false, true);
        yield return new WaitForFixedUpdate();

        // 落下する前に縦棒を右回転させた状態を確認
        Assert.AreEqual(new Vector2Int(5, 7), bs.CenterPos());
        Assert.AreEqual("IsHit((6,7),(5,7),(4,7),(3,7))\n", field.CallList);
        field.ClearCallList();

        // 2回目右回転
        input.SetReturn(false, false, false, false, true);
        yield return new WaitForFixedUpdate();

        // 落下する前に縦棒を右回転させた状態を確認
        Assert.AreEqual(new Vector2Int(5, 7), bs.CenterPos());
        Assert.AreEqual("IsHit((5,8),(5,7),(5,6),(5,5))\n", field.CallList);
        field.ClearCallList();

        // 3回目右回転
        input.SetReturn(false, false, false, false, true);
        yield return new WaitForFixedUpdate();

        // 落下する前に縦棒を右回転させた状態を確認
        Assert.AreEqual(new Vector2Int(5, 7), bs.CenterPos());
        Assert.AreEqual("IsHit((4,7),(5,7),(6,7),(7,7))\n", field.CallList);
        field.ClearCallList();

        // 4回目右回転
        input.SetReturn(false, false, false, false, true);
        yield return new WaitForFixedUpdate();

        // 落下する前に縦棒を右回転させた状態を確認
        Assert.AreEqual(new Vector2Int(5, 7), bs.CenterPos());
        Assert.AreEqual("IsHit((5,6),(5,7),(5,8),(5,9))\n", field.CallList);
        field.ClearCallList();

        GameObject.Destroy(bs.gameObject);

        yield return null;
    }

    [UnityTest]
    public IEnumerator UnitTestStateGameMainWithEnumeratorPasses002() // 左回転
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
        field.ReturnHeight = 10; // テスト用にフィールド高さを設定

        bs.Setup(player, field, input, sound, fallLevel);

        Assert.AreEqual("Width\nHeight\n", field.CallList);
        // ブロックのテンプレートの配置が(0, 0), (1, 0), (-1, 0), (0, 1) なので
        // あらゆる回転がなされてもブロックの各パーツの座標値が０以上かつフィールドの高さ未満になるよう補正され、ブロック全体が1つ下にずらされる
        // 個のテストケースではフィールド幅が10 なので、ブロック原点のx座標は中央の5 になるはず。
        Assert.AreEqual(new Vector2Int(5, 8), bs.CenterPos());
        field.ClearCallList();

        // 1回目左回転
        input.SetReturn(false, false, false, true, false);
        yield return new WaitForFixedUpdate();

        // 落下する前に凸ブロックを左回転させた状態を確認
        Assert.AreEqual(new Vector2Int(5, 8), bs.CenterPos());
        Assert.AreEqual("IsHit((5,8),(5,7),(5,9),(6,8))\n", field.CallList);
        field.ClearCallList();

        // 2回目左回転
        input.SetReturn(false, false, false, true, false);
        yield return new WaitForFixedUpdate();

        // 落下する前に凸ブロックを左回転させた状態を確認
        Assert.AreEqual(new Vector2Int(5, 8), bs.CenterPos());
        Assert.AreEqual("IsHit((5,8),(4,8),(6,8),(5,7))\n", field.CallList);
        field.ClearCallList();

        // 3回目左回転
        input.SetReturn(false, false, false, true, false);
        yield return new WaitForFixedUpdate();

        // 落下する前に凸ブロックを左回転させた状態を確認
        Assert.AreEqual(new Vector2Int(5, 8), bs.CenterPos());
        Assert.AreEqual("IsHit((5,8),(5,9),(5,7),(4,8))\n", field.CallList);
        field.ClearCallList();

        // 4回目左回転
        input.SetReturn(false, false, false, true, false);
        yield return new WaitForFixedUpdate();

        // 落下する前に凸ブロックを左回転させた状態を確認
        Assert.AreEqual(new Vector2Int(5, 8), bs.CenterPos());
        Assert.AreEqual("IsHit((5,8),(6,8),(4,8),(5,9))\n", field.CallList);
        field.ClearCallList();

        GameObject.Destroy(bs.gameObject);

        yield return null;
    }

    [UnityTest]
    public IEnumerator UnitTestStateGameMainWithEnumeratorPasses003() // 左右移動
    {
        int fallLevel = 24; // 落下しないよう落下スピードを遅めにする
        BlockSet bs = NewBlockSet("BlockSetC");
        StubPlayer player = new StubPlayer();
        StubField field = new StubField();
        StubInputManager input = new StubInputManager();
        StubSoundManager sound = new StubSoundManager();

        field.ReturnWidth = 10; // テスト用にフィールド幅を設定
        field.ReturnHeight = 10; // テスト用にフィールド高さを設定

        bs.Setup(player, field, input, sound, fallLevel);

        Assert.AreEqual("Width\nHeight\n", field.CallList);
        // ブロックのテンプレートの配置が(0, 0), (0, 1), (1, 1), (1, 0) なので
        // あらゆる回転がなされてもブロックの各パーツの座標値が０以上かつフィールドの高さ未満になるよう補正され、ブロック全体が1つ下にずらされる
        // 個のテストケースではフィールド幅が10 なので、ブロック原点のx座標は中央の5 になるはず。
        Assert.AreEqual(new Vector2Int(5, 8), bs.CenterPos());
        field.ClearCallList();

        for(int i=6;i<9;i++)
        {
            // 右移動
            input.SetReturn(false, false, true, false, false);
            field.ReturnIsHit = false;
            yield return new WaitForFixedUpdate();

            // 落下する前に四角ブロックを右移動させた状態を確認
            Assert.AreEqual(new Vector2Int(i, 8), bs.CenterPos());
            Assert.AreEqual($"IsHit(({i},8),({i},9),({1+i},9),({1+i},8))\n", field.CallList);
            field.ClearCallList();
            Assert.AreEqual($"Play({SoundMove})\n", sound.CallList); // 移動音
            sound.ClearCallList();
        }
        // 右移動出来ずに止まる
        input.SetReturn(false, false, true, false, false);
        field.ReturnIsHit = true; 
        yield return new WaitForFixedUpdate();

        // 落下する前に四角ブロックを右移動させた状態を確認
        Assert.AreEqual(new Vector2Int(8, 8), bs.CenterPos()); // 進みすぎて衝突し、座標が元の位置に戻る
        Assert.AreEqual($"IsHit((9,8),(9,9),(10,9),(10,8))\n", field.CallList); // 進みすぎた時の座標値が入っている。ブロックの位置はIsHitの呼び出し後に戻される。
        field.ClearCallList();
        Assert.AreEqual($"Play({SoundCollide})\n", sound.CallList); // 衝突音
        sound.ClearCallList();

        for (int i=7;i>=0;i--)
        {
            // 左移動
            input.SetReturn(false, true, false, false, false);
            field.ReturnIsHit = false;
            yield return new WaitForFixedUpdate();

            // 落下する前に四角ブロックを左移動させた状態を確認
            Assert.AreEqual(new Vector2Int(i, 8), bs.CenterPos());
            Assert.AreEqual($"IsHit(({i},8),({i},9),({i+1},9),({i+1},8))\n", field.CallList);
            field.ClearCallList();
            Assert.AreEqual($"Play({SoundMove})\n", sound.CallList); // 移動音
            sound.ClearCallList();
        }
        // 左移動出来ずに止まる
        input.SetReturn(false, true, false, false, false);
        field.ReturnIsHit = true;
        yield return new WaitForFixedUpdate();

        // 落下する前に四角ブロックを右移動させた状態を確認
        Assert.AreEqual(new Vector2Int(0, 8), bs.CenterPos()); // 進みすぎて衝突し、座標が元の位置に戻る
        Assert.AreEqual($"IsHit((-1,8),(-1,9),(0,9),(0,8))\n", field.CallList); // 進みすぎた時の座標値が入っている。ブロックの位置はIsHitの呼び出し後に戻される。
        field.ClearCallList();
        Assert.AreEqual($"Play({SoundCollide})\n", sound.CallList); // 衝突音
        sound.ClearCallList();


        GameObject.Destroy(bs.gameObject);

        yield return null;
    }


    [UnityTest]
    public IEnumerator UnitTestStateGameMainWithEnumeratorPasses004() // 下移動(入力による加速落下)
    {
        int fallLevel = 4;
        BlockSet bs = NewBlockSet("BlockSetC");
        StubPlayer player = new StubPlayer();
        StubField field = new StubField();
        StubInputManager input = new StubInputManager();
        StubSoundManager sound = new StubSoundManager();

        field.ReturnWidth = 10; // テスト用にフィールド幅を設定
        field.ReturnHeight = 10; // テスト用にフィールド高さを設定

        bs.Setup(player, field, input, sound, fallLevel);

        Assert.AreEqual("Width\nHeight\n", field.CallList);
        // ブロックのテンプレートの配置が(0, 0), (0, 1), (1, 1), (1, 0) なので
        // あらゆる回転がなされてもブロックの各パーツの座標値が０以上かつフィールドの高さ未満になるよう補正され、ブロック全体が1つ下にずらされる
        // 個のテストケースではフィールド幅が10 なので、ブロック原点のx座標は中央の5 になるはず。
        Assert.AreEqual(new Vector2Int(5, 8), bs.CenterPos());
        field.ClearCallList();

        // ---
        for (int i=0;i<BlockSet.CountWaitFallingLimit - 1;i++)
        {
            // 下移動
            input.SetReturn(true, false, false, false, false);
            yield return new WaitForFixedUpdate();

            // ボタンを押してもしばらくは移動しない
            Assert.AreEqual(new Vector2Int(5, 8), bs.CenterPos());
            Assert.AreEqual("", field.CallList);
            field.ClearCallList();
        }
        // 下移動
        input.SetReturn(true, false, false, false, false);
        yield return new WaitForFixedUpdate();

        // ボタンを押すとすぐに下に移動する
        Assert.AreEqual(new Vector2Int(5, 7), bs.CenterPos());
        Assert.AreEqual("IsHit((5,7),(5,8),(6,8),(6,7))\n", field.CallList);
        field.ClearCallList();

        // ---
        for (int i = 0; i < BlockSet.CountWaitFallingLimit - 1; i++)
        {
            // 下移動
            input.SetReturn(true, false, false, false, false);
            yield return new WaitForFixedUpdate();

            // ボタンを押してもしばらくは移動しない
            Assert.AreEqual(new Vector2Int(5, 7), bs.CenterPos());
            Assert.AreEqual("", field.CallList);
            field.ClearCallList();
        }
        // 下移動入力をしない（上記のループで下移動入力済みなので下移動するはず）
        input.SetReturn(false, false, false, false, false);
        yield return new WaitForFixedUpdate();

        // ボタンを押すとすぐに下に移動する
        Assert.AreEqual(new Vector2Int(5, 6), bs.CenterPos());
        Assert.AreEqual("IsHit((5,6),(5,7),(6,7),(6,6))\n", field.CallList);
        field.ClearCallList();

        // ---
        for (int i = 0; i < BlockSet.CountWaitFallingLimit - 1; i++)
        {
            // 下移動入力をしない
            input.SetReturn(false, false, false, false, false);
            yield return new WaitForFixedUpdate();

            Assert.AreEqual(new Vector2Int(5, 6), bs.CenterPos());
            Assert.AreEqual("", field.CallList);
            field.ClearCallList();
        }
        // 下移動入力をする（上記のループで下移動入力してないが、ここでの入力でギリギリ間に合うはず）
        input.SetReturn(true, false, false, false, false);
        yield return new WaitForFixedUpdate();

        // ボタンを押すとすぐに下に移動する
        Assert.AreEqual(new Vector2Int(5, 5), bs.CenterPos());
        Assert.AreEqual("IsHit((5,5),(5,6),(6,6),(6,5))\n", field.CallList);
        field.ClearCallList();

        // ---
        for (int i = 0; i < BlockSet.CountWaitFallingLimit - 1; i++)
        {
            // 下移動入力をしない
            input.SetReturn(false, false, false, false, false);
            yield return new WaitForFixedUpdate();

            Assert.AreEqual(new Vector2Int(5, 5), bs.CenterPos());
            Assert.AreEqual("", field.CallList);
            field.ClearCallList();
        }
        // 下移動入力をしない（移動しないはず）
        input.SetReturn(false, false, false, false, false);
        yield return new WaitForFixedUpdate();

        Assert.AreEqual(new Vector2Int(5, 5), bs.CenterPos());
        Assert.AreEqual("", field.CallList);
        field.ClearCallList();

        GameObject.Destroy(bs.gameObject); 

        yield return null;
    }

    [UnityTest]
    public IEnumerator UnitTestStateGameMainWithEnumeratorPasses005() // 下移動(自由落下)
    {
        int fallLevel = 4;
        BlockSet bs = NewBlockSet("BlockSetC");
        StubPlayer player = new StubPlayer();
        StubField field = new StubField();
        StubInputManager input = new StubInputManager();
        StubSoundManager sound = new StubSoundManager();

        field.ReturnWidth = 10; // テスト用にフィールド幅を設定
        field.ReturnHeight = 10; // テスト用にフィールド高さを設定

        bs.Setup(player, field, input, sound, fallLevel);

        Assert.AreEqual("Width\nHeight\n", field.CallList);
        // ブロックのテンプレートの配置が(0, 0), (0, 1), (1, 1), (1, 0) なので
        // あらゆる回転がなされてもブロックの各パーツの座標値が０以上かつフィールドの高さ未満になるよう補正され、ブロック全体が1つ下にずらされる
        // 個のテストケースではフィールド幅が10 なので、ブロック原点のx座標は中央の5 になるはず。
        Assert.AreEqual(new Vector2Int(5, 8), bs.CenterPos());
        field.ClearCallList();

        input.SetReturn(false, false, false, false, false);

        // ---
        for(int k=0;k<3;k++) // ３回自由落下させてみる
        {
            for (int i = 0; i < fallLevel - 1; i++)
            {
                for (int j = 0; j < BlockSet.CountWaitFallingLimit; j++)
                {
                    yield return new WaitForFixedUpdate();

                    // しばらくは移動しない
                    Assert.AreEqual(new Vector2Int(5, 8-k), bs.CenterPos());
                    Assert.AreEqual("", field.CallList);
                    field.ClearCallList();
                }
            }

            for (int j = 0; j < BlockSet.CountWaitFallingLimit - 1; j++)
            {
                yield return new WaitForFixedUpdate();

                // しばらくは移動しない
                Assert.AreEqual(new Vector2Int(5, 8-k), bs.CenterPos());
                Assert.AreEqual("", field.CallList);
                field.ClearCallList();
            }

            // 下移動
            yield return new WaitForFixedUpdate();

            Assert.AreEqual(new Vector2Int(5, 7-k), bs.CenterPos());
            Assert.AreEqual($"IsHit((5,{7-k}),(5,{8-k}),(6,{8-k}),(6,{7-k}))\n", field.CallList);
            field.ClearCallList();
        }

        GameObject.Destroy(bs.gameObject);

        yield return null;
    }

    [UnityTest]
    public IEnumerator UnitTestStateGameMainWithEnumeratorPasses006() // 下移動(一番下まで自由落下)
    {
        int fallLevel = 4;
        BlockSet bs = NewBlockSet("BlockSetC");
        StubPlayer player = new StubPlayer();
        StubField field = NewStubField();
        StubInputManager input = new StubInputManager();
        StubSoundManager sound = new StubSoundManager();

        field.ReturnWidth = 10; // テスト用にフィールド幅を設定
        field.ReturnHeight = 10; // テスト用にフィールド高さを設定

        bs.Setup(player, field, input, sound, fallLevel);

        Assert.AreEqual("Width\nHeight\n", field.CallList);
        // ブロックのテンプレートの配置が(0, 0), (0, 1), (1, 1), (1, 0) なので
        // あらゆる回転がなされてもブロックの各パーツの座標値が０以上かつフィールドの高さ未満になるよう補正され、ブロック全体が1つ下にずらされる
        // 個のテストケースではフィールド幅が10 なので、ブロック原点のx座標は中央の5 になるはず。
        Assert.AreEqual(new Vector2Int(5, 8), bs.CenterPos());
        field.ClearCallList();

        input.SetReturn(false, false, false, false, false);

        // ---
        field.ReturnIsHit = false; //しばらくは衝突しない
        for (int k = 0; k < 3; k++) // ３回自由落下させてみる
        {
            for (int i = 0; i < fallLevel * BlockSet.CountWaitFallingLimit - 1; i++)
            {
                yield return new WaitForFixedUpdate();

                // しばらくは移動しない
                Assert.AreEqual(new Vector2Int(5, 8-k), bs.CenterPos());
                Assert.AreEqual("", field.CallList);
                field.ClearCallList();
                Assert.AreEqual("", sound.CallList); // 落下中音はならない
                sound.ClearCallList();
                Assert.AreEqual("", player.CallList);
                player.ClearCallList();
            }

            // 下移動
            yield return new WaitForFixedUpdate();

            Assert.AreEqual(new Vector2Int(5, 7-k), bs.CenterPos());
            Assert.AreEqual($"IsHit((5,{7-k}),(5,{8-k}),(6,{8-k}),(6,{7-k}))\n", field.CallList);
            field.ClearCallList();
            Assert.AreEqual("", sound.CallList); // 落下中音はならない
            sound.ClearCallList();
            Assert.AreEqual("", player.CallList);
            player.ClearCallList();
        }

        for (int i = 0; i < fallLevel * BlockSet.CountWaitFallingLimit - 1; i++)
        {
            yield return new WaitForFixedUpdate();

            // しばらくは移動しない
            Assert.AreEqual(new Vector2Int(5, 5), bs.CenterPos());
            Assert.AreEqual("", field.CallList);
            field.ClearCallList();
            Assert.AreEqual("", sound.CallList); // 落下中音はならない
            sound.ClearCallList();
            Assert.AreEqual("", player.CallList);
            player.ClearCallList();
        }

        // 下移動しようとするが、衝突してしまい、移動できずにとどまる
        field.ReturnIsHit = true; // ここで衝突させる
        field.ReturnSetBlocks = false; // 上にはつまらない
        yield return new WaitForFixedUpdate();

        Assert.AreEqual(new Vector2Int(5, 5), bs.CenterPos());
        Assert.AreEqual("IsHit((5,4),(5,5),(6,5),(6,4))\n"
            + "SetBlocks((5,5),(5,6),(6,6),(6,5))\n", field.CallList);
        field.ClearCallList();
        sound.ClearCallList(); // サウンドの確認は本番のField インスタンスと結合する必要があり、ここで確認することができない。
        Assert.AreEqual("PullNextBlock\n", player.CallList);
        player.ClearCallList();

        GameObject.Destroy(bs.gameObject);

        yield return null;
    }
    [UnityTest]
    public IEnumerator UnitTestStateGameMainWithEnumeratorPasses007() // 下移動(落下し、設置して上が詰まる)
    {
        int fallLevel = 4;
        BlockSet bs = NewBlockSet("BlockSetC");
        StubPlayer player = new StubPlayer();
        StubField field = NewStubField();
        StubInputManager input = new StubInputManager();
        StubSoundManager sound = new StubSoundManager();

        field.ReturnWidth = 10; // テスト用にフィールド幅を設定
        field.ReturnHeight = 10; // テスト用にフィールド高さを設定

        bs.Setup(player, field, input, sound, fallLevel);

        Assert.AreEqual("Width\nHeight\n", field.CallList);
        // ブロックのテンプレートの配置が(0, 0), (0, 1), (1, 1), (1, 0) なので
        // あらゆる回転がなされてもブロックの各パーツの座標値が０以上かつフィールドの高さ未満になるよう補正され、ブロック全体が1つ下にずらされる
        // 個のテストケースではフィールド幅が10 なので、ブロック原点のx座標は中央の5 になるはず。
        Assert.AreEqual(new Vector2Int(5, 8), bs.CenterPos());
        field.ClearCallList();

        input.SetReturn(false, false, false, false, false);

        // ---
        for (int i = 0; i < fallLevel * BlockSet.CountWaitFallingLimit - 1; i++)
        {
            yield return new WaitForFixedUpdate();

            // しばらくは移動しない
            Assert.AreEqual(new Vector2Int(5, 8), bs.CenterPos());
            Assert.AreEqual("", field.CallList);
            field.ClearCallList();
            Assert.AreEqual("", sound.CallList); // 落下中音はならない
            sound.ClearCallList();
            Assert.AreEqual("", player.CallList);
            player.ClearCallList();
        }

        // 下移動しようとするが、衝突してしまい、移動できずにとどまる
        field.ReturnIsHit = true; // ここで衝突させる
        field.ReturnSetBlocks = true; // 上に詰まる
        yield return new WaitForFixedUpdate();

        Assert.AreEqual(new Vector2Int(5, 8), bs.CenterPos());
        Assert.AreEqual("IsHit((5,7),(5,8),(6,8),(6,7))\n"
            + "SetBlocks((5,8),(5,9),(6,9),(6,8))\n", field.CallList);
        field.ClearCallList();
        sound.ClearCallList(); // サウンドの確認は本番のField インスタンスと結合する必要があり、ここで確認することができない。
        Assert.AreEqual("Dead\n", player.CallList);
        player.ClearCallList();

        GameObject.Destroy(bs.gameObject);

        yield return null;
    }

    private BlockSet NewBlockSet(string blockName)
    {
        BlockSet prefab = Resources.Load<BlockSet>("UnityTetris/Prefabs/BlockSet/" + blockName);
        BlockSet obj = GameObject.Instantiate(prefab);

        return obj;

    }

    private StubField NewStubField()
    {
        StubField prefab = Resources.Load<StubField>("UnityTetris/Prefabs/Test/StubField");
        StubField obj = GameObject.Instantiate(prefab);

        return obj;

    }
}
