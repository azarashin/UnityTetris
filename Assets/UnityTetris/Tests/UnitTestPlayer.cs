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
    public void UnitTest001()
    {
        int fallLevel = 4; 
        Player p = NewPlayer();
        ISoundManager sound = new StubSoundManager();
        StubBlockSet[] bs = new StubBlockSet[] { StubBlockSet() };
        StubField field = StubFieldPrefab();
        //Player のインスタンスが問題なく生成されてることを確認。
        p.Setup(field, bs, sound, fallLevel);
    }

    [UnityTest]
    public IEnumerator UnityTest001()
    {
        ObjectPicker<StubField> fieldPicker = new ObjectPicker<StubField>();
        ObjectPicker<StubBlockSet> blockSetPicker = new ObjectPicker<StubBlockSet>();
        int fallLevel = 4;
        Player p = NewPlayer();
        ISoundManager sound = new StubSoundManager();
        StubBlockSet[] bs = new StubBlockSet[] { StubBlockSet() };
        StubField fieldPrefab = StubFieldPrefab();
        StubStateGameMain gameMain = StubStateGameMain(); 
        fieldPicker.Pick(); 
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

        yield return null;
    }


    private StubStateGameMain StubStateGameMain()
    {
        GameObject prefab = Resources.Load<GameObject>("UnityTetris/Prefabs/Test/StubStateGameMain");
        GameObject obj = GameObject.Instantiate(prefab);

        StubStateGameMain ret = obj.GetComponent<StubStateGameMain>();
        return ret;

    }

    private Player NewPlayer()
    {
        GameObject prefab = Resources.Load<GameObject>("UnityTetris/Prefabs/Player");
        GameObject obj = GameObject.Instantiate(prefab);

        Player ret = obj.GetComponent<Player>();
        return ret;

    }

    private StubBlockSet StubBlockSet()
    {
        StubBlockSet prefab = Resources.Load<StubBlockSet>("UnityTetris/Prefabs/Test/StubBlockSet");
        return prefab;

    }

    private StubField StubFieldPrefab()
    {
        GameObject prefab = Resources.Load<GameObject>("UnityTetris/Prefabs/Test/StubField");
        StubField ret = prefab.GetComponent<StubField>();
        return ret;

    }
}
