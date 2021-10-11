using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class UnitTestLevelController
{
    [Test]
    public void UnitTestLevelControllerSimplePasses001()
    {
        int fallLevel = 4; 
        LevelController controller = new LevelController(fallLevel); 
        for(int i=0;i< fallLevel - LevelController.MinimumFallLevel;i++)
        {
            for(int j=0;j<LevelController.NumberOfPhasesToChangeFallLevel;j++)
            {
                Assert.AreEqual(i + 1, controller.CurrentDisplayLevel());
                Assert.AreEqual(controller.CurrentDisplayLevel(), fallLevel - controller.CurrentFallLevel() + 1);
                controller.NextBlockHasBeenPulled(); 
            }
        }
        // ‚±‚êˆÈã—Ž‰º‘¬“x‚Íã‚ª‚ç‚È‚¢
        Assert.AreEqual(LevelController.MinimumFallLevel, controller.CurrentFallLevel());
        Assert.AreEqual(controller.CurrentDisplayLevel(), fallLevel - controller.CurrentFallLevel() + 1);
        controller.NextBlockHasBeenPulled();
        Assert.AreEqual(LevelController.MinimumFallLevel, controller.CurrentFallLevel());
        Assert.AreEqual(controller.CurrentDisplayLevel(), fallLevel - controller.CurrentFallLevel() + 1);
    }

}
