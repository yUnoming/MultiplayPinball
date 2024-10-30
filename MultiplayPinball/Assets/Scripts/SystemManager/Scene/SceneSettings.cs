using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// シーンタイプ    </summary>
public enum SceneType
{
    /// <summary>
    /// 特になし    </summary>
    None,
    /// <summary>
    /// タイトル    </summary>
    Title,
    /// <summary>
    /// セレクト    </summary>
    Select,
    /// <summary>
    /// ゲームのマッチング中    </summary>
    GameMatching,
    /// <summary>
    /// シングルゲーム    </summary>
    SingleGame,
    /// <summary>
    /// マルチゲーム    </summary>
    MultiGame,
    /// <summary>
    /// リザルト    </summary>
    Result,
    /// <summary>
    /// ゲーム終了    </summary>
    Quit,
};

public class SceneSettings : MonoBehaviour
{
    [Serializable]
    public class SceneSetting
    {
        [SerializeField]
        [Header("設定するシーンタイプ")]
        private SceneType sceneType;

        [SerializeField]
        [Header("シーン名")]
        [Tooltip("上記のシーンタイプに遷移する際シーンが変わるなら、シーン名を入力")]
        private string sceneName = "tmp";

        public SceneType GetSceneType() { return sceneType; }
        public string GetSceneName() { return sceneName; }
    }

    [SerializeField]
    [Header("各シーンの設定")]
    private SceneSetting[] sceneSetting;

    public SceneSetting[] GetSceneSetting() { return sceneSetting;}
}
