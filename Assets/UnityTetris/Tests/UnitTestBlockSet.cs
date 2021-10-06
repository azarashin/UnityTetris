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

    private BlockSet NewBlockSet(string blockName)
    {
        GameObject prefab = Resources.Load<GameObject>("Prefabs/" + blockName);
        GameObject obj = GameObject.Instantiate(prefab);

        BlockSet ret = obj.GetComponent<BlockSet>();
        return ret;

    }

    [UnityTest]
    public IEnumerator UnityTest001()
    {
        // Use the Assert class to test conditions.
        // Use yield to skip a frame.
        yield return null;
    }
}
