using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleGameManager : SceneManagerBase
{
    protected override void StateRunning()
    {
        // Tmp: Enter�L�[�Ń��U���g��
        if (Input.GetKeyDown(KeyCode.Return))
        {
            SceneTransition(SceneType.Result, true);
        }
    }
}
