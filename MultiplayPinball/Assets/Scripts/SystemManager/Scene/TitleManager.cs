using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleManager : SceneManagerBase
{
    protected override void StateRunning()
    {
        // Tmp: Enter�L�[�ŃQ�[���V�[����
        if(Input.GetKeyDown(KeyCode.Return))
        {
            SceneTransition(SceneType.SingleGame, true);
        }
    }
}
