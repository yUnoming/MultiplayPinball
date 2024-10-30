using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleManager : SceneManagerBase
{
    protected override void StateRunning()
    {
        // Tmp: Enterキーでゲームシーンへ
        if(Input.GetKeyDown(KeyCode.Return))
        {
            SceneTransition(SceneType.SingleGame, true);
        }
    }
}
