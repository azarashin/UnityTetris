using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityTetris;

public class UnitTestStateGameBoot
{
    [UnityTest]
    public IEnumerator UnitTestStateGameBootWithEnumeratorPasses()
    {
        int fallLevel = 4;
        StateGameBoot boot = NewStateGameBoot();
        StubGameController controller = new StubGameController();
        StubPlayer[] players = new StubPlayer[] { new StubPlayer(), new StubPlayer()};
        StubField field = new StubField();
        BlockSet[] bss = new BlockSet[] { PrefabBlockSet("BlockSetA"), PrefabBlockSet("BlockSetC") };
        StubSoundManager sound = new StubSoundManager();

        Assert.AreEqual("", players[0].CallList);
        Assert.AreEqual("", players[1].CallList);

        boot.Setup(controller, field, players, bss, sound, fallLevel);

        Assert.AreEqual("Setup\n", players[0].CallList);
        Assert.AreEqual("Setup\n", players[1].CallList);

        // この次のフレームでコルーチンが起動するはず
        yield return new WaitForFixedUpdate();

        float limit = boot.CountMax + 0.1f; // 少なくともStateGameBoot に指定されたカウント秒数+100msec以内には状態遷移をすること
        float start = Time.time;
        float end;
        Assert.AreEqual("", controller.CallList); 
        while((end = Time.time) - start <= limit)
        {
            if(controller.CallList == "RunGame\n") {
                break; 
            }
            yield return new WaitForFixedUpdate();
        }
        Assert.IsTrue(end - start <= limit); 


        yield return null;
    }

    private BlockSet PrefabBlockSet(string blockName)
    {
        BlockSet prefab = Resources.Load<BlockSet>("UnityTetris/Prefabs/BlockSet/" + blockName);


        return prefab;

    }

    private StateGameBoot NewStateGameBoot()
    {
        StateGameBoot prefab = Resources.Load<StateGameBoot>("UnityTetris/Prefabs/State/StateGameBoot");
        StateGameBoot obj = GameObject.Instantiate(prefab);

        return obj;

    }

}
