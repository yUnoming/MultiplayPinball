using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �V�[���^�C�v    </summary>
public enum SceneType
{
    /// <summary>
    /// ���ɂȂ�    </summary>
    None,
    /// <summary>
    /// �^�C�g��    </summary>
    Title,
    /// <summary>
    /// �Z���N�g    </summary>
    Select,
    /// <summary>
    /// �Q�[���̃}�b�`���O��    </summary>
    GameMatching,
    /// <summary>
    /// �V���O���Q�[��    </summary>
    SingleGame,
    /// <summary>
    /// �}���`�Q�[��    </summary>
    MultiGame,
    /// <summary>
    /// ���U���g    </summary>
    Result,
    /// <summary>
    /// �Q�[���I��    </summary>
    Quit,
};

public class SceneSettings : MonoBehaviour
{
    [Serializable]
    public class SceneSetting
    {
        [SerializeField]
        [Header("�ݒ肷��V�[���^�C�v")]
        private SceneType sceneType;

        [SerializeField]
        [Header("�V�[����")]
        [Tooltip("��L�̃V�[���^�C�v�ɑJ�ڂ���ۃV�[�����ς��Ȃ�A�V�[���������")]
        private string sceneName = "tmp";

        public SceneType GetSceneType() { return sceneType; }
        public string GetSceneName() { return sceneName; }
    }

    [SerializeField]
    [Header("�e�V�[���̐ݒ�")]
    private SceneSetting[] sceneSetting;

    public SceneSetting[] GetSceneSetting() { return sceneSetting;}
}
