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
        p.Setup(field, bs, sound, fallLevel);
    }

    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    [UnityTest]
    public IEnumerator UnityTest001()
    {
        ObjectPicker<StubField> fieldPicker = new ObjectPicker<StubField>(); 
        int fallLevel = 4;
        Player p = NewPlayer();
        ISoundManager sound = new StubSoundManager();
        StubBlockSet[] bs = new StubBlockSet[] { StubBlockSet() };
        StubField fieldPrefab = StubFieldPrefab();
        fieldPicker.Pick(); 
        p.Setup(fieldPrefab, bs, sound, fallLevel);
        StubField[] fields = fieldPicker.Pick();
        Assert.AreEqual(1, fields.Length);
        Assert.AreEqual("ResetField(-1,-1,-1)\n", fields[0].CallList); 
        yield return null;
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
        GameObject prefab = Resources.Load<GameObject>("UnityTetris/Prefabs/Test/StubBlockSet");
        GameObject obj = GameObject.Instantiate(prefab);

        StubBlockSet ret = obj.GetComponent<StubBlockSet>();
        return ret;

    }

    private StubField StubFieldPrefab()
    {
        GameObject prefab = Resources.Load<GameObject>("UnityTetris/Prefabs/Test/StubField");
        StubField ret = prefab.GetComponent<StubField>();
        return ret;

    }
}
