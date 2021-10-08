using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityTetris;

public class UnitTestStateGameFinish
{

    [UnityTest]
    public IEnumerator UnitTestStateGameFinishWithEnumeratorPasses()
    {
        int fallLevel = 4;
        StateGameFinish finish = NewStateGameFinish();
        StubGameController controller = new StubGameController();

        finish.Setup(controller);

        // ���̎��̃t���[���ŃR���[�`�����N������͂�
        yield return new WaitForFixedUpdate();

        float limit = finish.WaitMax + 0.1f; // ���Ȃ��Ƃ�StateGameBoot �Ɏw�肳�ꂽ�J�E���g�b��+100msec�ȓ��ɂ͏�ԑJ�ڂ����邱��
        float start = Time.time;
        float end;
        Assert.AreEqual("", controller.CallList);
        while ((end = Time.time) - start <= limit)
        {
            if (controller.CallList == "BootGame\n")
            {
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

    private StateGameFinish NewStateGameFinish()
    {
        StateGameFinish prefab = Resources.Load<StateGameFinish>("UnityTetris/Prefabs/State/StateGameFinish");
        StateGameFinish obj = GameObject.Instantiate(prefab);

        return obj;

    }

}
