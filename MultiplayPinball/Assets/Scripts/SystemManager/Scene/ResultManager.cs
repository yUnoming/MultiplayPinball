using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultManager : SceneManagerBase
{
    protected override void StateRunning()
    {
        // Tmp: Enter�L�[�Ń^�C�g����
        if (Input.GetKeyDown(KeyCode.Return))
        {
            SceneTransition(SceneType.Title, true);
        }
    }
}
