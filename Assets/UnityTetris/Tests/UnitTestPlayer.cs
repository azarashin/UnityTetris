using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityTetris;
using UnityTetris.Abstract;
using UnityTetris.Interface;

public class UnitTestPlayer
{
    [Test]
    public void UnitTestPlayerSimplePasses001()
    {
        ObjectPicker<StubField> fieldPicker = new ObjectPicker<StubField>();
        ObjectPicker<StubBlockSet> blockSetPicker = new ObjectPicker<StubBlockSet>();
        int fallLevel = 4;
        Player p = NewPlayer();
        ISoundManager sound = new StubSoundManager();
        StubBlockSet[] bs = new StubBlockSet[] { StubBlockSetPrefab() };
        StubField fieldPrefab = StubFieldPrefab();
        StubStateGameMain gameMain = StubStateGameMain(); 
        fieldPicker.Pick();
        //Player のインスタンスが問題なく生成されてることを確認。
        p.Setup(fieldPrefab, bs, sound, fallLevel);

        // Setup の呼び出しにより、内部でStubField(fieldPrefab) のインスタンスができているはず。
        StubField[] fields = fieldPicker.Pick();
        Assert.AreEqual(1, fields.Length);

        // StubField のResetField が呼び出されていることを確認。
        Assert.AreEqual("ResetField(-1,-1,-1)\n", fields[0].CallList);

        StubBlockSet[] blocks = blockSetPicker.Pick();
        Assert.AreEqual(0, blocks.Length); // この時点でブロックはまだできていない

        p.StartGame(gameMain);

        blocks = blockSetPicker.Pick();
        Assert.AreEqual(1, blocks.Length); // この時点でブロックが１つで来ているはず
        Assert.AreEqual($"Setup({fallLevel})\n", blocks[0].CallList);
        GameObject.Destroy(p); 

    }

    [Test]
    public void UnitTestPlayerSimplePasses002()
    {
        int fallLevel = 4;
        Player p = NewPlayer();
        ISoundManager sound = new StubSoundManager();
        StubBlockSet[] bs = new StubBlockSet[] { StubBlockSetPrefab() };
        StubField fieldPrefab = StubFieldPrefab();
        StubStateGameMain gameMain = StubStateGameMain();

        p.Setup(fieldPrefab, bs, sound, fallLevel);
        p.StartGame(gameMain);

        Assert.AreEqual(true, p.IsAlive()); // まだ生きている
        Assert.AreEqual("", gameMain.CallList);

        p.Dead();

        Assert.AreEqual(false, p.IsAlive()); // 死んだ
        Assert.AreEqual($"PlayerGameOver({p.GetHashCode()})\n", gameMain.CallList); // 通知が来た
        GameObject.Destroy(p);

    }

    [UnityTest]
    public IEnumerator UnitTestPlayerWithEnumeratorPasses001()
    {
        int fallLevel = 4;
        StubField[] fields; 
        Player p = NewPlayer();
        ISoundManager sound = new StubSoundManager();
        StubBlockSet[] bs = new StubBlockSet[] { StubBlockSetPrefab() };
        StubField fieldPrefab = StubFieldPrefab();
        StubStateGameMain gameMain = StubStateGameMain();

        fields = GameObject.FindObjectsOfType<StubField>();
        Assert.AreEqual(0, fields.Length); 
        p.Setup(fieldPrefab, bs, sound, fallLevel);
        yield return new WaitForFixedUpdate(); 

        fields = GameObject.FindObjectsOfType<StubField>();
        Assert.AreEqual(1, fields.Length);
        p.Setup(fieldPrefab, bs, sound, fallLevel);
        yield return new WaitForFixedUpdate();

        fields = GameObject.FindObjectsOfType<StubField>();
        Assert.AreEqual(1, fields.Length); // プレイヤーが一人に対してフィールドは二つ以上存在してはいけない

        GameObject.Destroy(p);

        yield return null; 
    }


    private StubStateGameMain StubStateGameMain()
    {
        StubStateGameMain prefab = Resources.Load<StubStateGameMain>("UnityTetris/Prefabs/Test/StubStateGameMain");
        StubStateGameMain obj = GameObject.Instantiate(prefab);

        return obj;

    }

    private Player NewPlayer()
    {
        Player prefab = Resources.Load<Player>("UnityTetris/Prefabs/Player");
        Player obj = GameObject.Instantiate(prefab);

        return obj;

    }

    private StubBlockSet StubBlockSetPrefab()
    {
        StubBlockSet prefab = Resources.Load<StubBlockSet>("UnityTetris/Prefabs/Test/StubBlockSet");
        return prefab;

    }

    private StubField StubFieldPrefab()
    {
        StubField prefab = Resources.Load<StubField>("UnityTetris/Prefabs/Test/StubField");
        return prefab;

    }
}
