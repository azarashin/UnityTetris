using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityTetris;

public class UnitTestStateGameMain
{
    [Test]
    public void UnitTestStateGameMainSimplePasses001()
    {
        StateGameMain main = NewStateGameMain();
        StubGameController controller = new StubGameController(); 
        StubPlayer[] players = new StubPlayer[] { new StubPlayer(), new StubPlayer() };

        main.Setup(controller, players);

        Assert.AreEqual("StartGame\n", players[0].CallList); // 各プレイヤーに対してゲーム開始していることを確認。
        Assert.AreEqual("StartGame\n", players[1].CallList);
    }

    [Test]
    public void UnitTestStateGameMainSimplePasses002()
    {
        StateGameMain main = NewStateGameMain();
        StubGameController controller = new StubGameController();
        StubPlayer[] players = new StubPlayer[] { new StubPlayer(), new StubPlayer(), new StubPlayer() };

        main.Setup(controller, players);

        players[0].RetIsAlive = false;
        players[1].RetIsAlive = true;
        players[2].RetIsAlive = true;
        main.PlayerGameOver(players[0]);

        Assert.AreEqual("", controller.CallList); // この時点ではまだ2人残っているため、イベント呼び出しされない

        players[0].RetIsAlive = false;
        players[1].RetIsAlive = false;
        players[2].RetIsAlive = true;
        main.PlayerGameOver(players[1]);

        Assert.AreEqual($"FinishGame\n", controller.CallList); // 残り一人になったのでイベントが呼び出される。

    }


    private StateGameMain NewStateGameMain()
    {
        StateGameMain prefab = Resources.Load<StateGameMain>("UnityTetris/Prefabs/State/StateGameMain");
        StateGameMain obj = GameObject.Instantiate(prefab);

        return obj;

    }

}
