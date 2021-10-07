using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityTetris;

public class UnitTestPlayer
{
    [Test]
    public void UnitTest001()
    {
        Player p = NewPlayer(); 
    }

    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    [UnityTest]
    public IEnumerator UnitTestPlayerWithEnumeratorPasses()
    {
        // Use the Assert class to test conditions.
        // Use yield to skip a frame.
        yield return null;
    }

    private Player NewPlayer()
    {
        GameObject prefab = Resources.Load<GameObject>("UnityTetris/Prefabs/Player");
        GameObject obj = GameObject.Instantiate(prefab);

        Player ret = obj.GetComponent<Player>();
        return ret;

    }
}
