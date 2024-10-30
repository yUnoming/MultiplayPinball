using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultManager : SceneManagerBase
{
    protected override void StateRunning()
    {
        // Tmp: Enterキーでタイトルへ
        if (Input.GetKeyDown(KeyCode.Return))
        {
            SceneTransition(SceneType.Title, true);
        }
    }
}
