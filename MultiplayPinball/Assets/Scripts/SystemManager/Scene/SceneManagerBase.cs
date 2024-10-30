using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerBase : MonoBehaviour
{
    /// <summary>
    /// �J�ڃ^�C�v    </summary>
    public enum TransitionType
    {
        /// <summary>
        /// ���ɂȂ�    </summary>
        None,
        /// <summary>
        /// �t�F�[�h    </summary>
        Fade,
    }
    /// <summary>
    /// �V�[���̏��    </summary>
    public enum SceneState
    {
        /// <summary>
        /// ���ɂȂ�    </summary>
        None,
        /// <summary>
        /// ������    </summary>
        Init,
        /// <summary>
        /// �J�n    </summary>
        Start,
        /// <summary>
        /// �V�[����������    </summary>
        Running,
        /// <summary>
        /// �I��    </summary>
        End,
        /// <summary>
        /// �㏈��    </summary>
        Uninit,
        /// <summary>
        /// �ꎞ��~��    </summary>
        Pause
    }

    private static GameObject sceneManagerInstance; // �V�[���}�l�[�W���[�̃C���X�^���X
    private static SceneSettings sceneSettings;     // �V�[���ݒ�R���|�[�l���g
    private static SceneManagerBase activeManager; // ���݃A�N�e�B�u�ȃ}�l�[�W���[

    private static bool isChangeScene;
    private static TransitionType transitionType;

    protected static SceneState currentSceneState = SceneState.Init; // ���݂̃V�[�����

    protected static SceneType nextScenetype = SceneType.None;        // �J�ڐ�V�[���^�C�v
    protected static SceneType currentSceneType = SceneType.Title;    // ���݂̃V�[���^�C�v
    protected static SceneType lateSceneType;                         // �O��̃V�[���^�C�v

    public static SceneState CurrentSceneState { get { return currentSceneState; } }
    public static SceneType NextSceneType { get { return nextScenetype; } }
    public static SceneType CurrentSceneType { get { return currentSceneType; } }
    public static SceneType LateSceneType { get { return lateSceneType; } }

    protected void SetSceneTransitionSettings(SceneType _nextSceneType, bool _isChangeScene = false, TransitionType _transType = TransitionType.None)
    {
        // �ꎞ�ۑ�
        nextScenetype = _nextSceneType;
        isChangeScene = _isChangeScene;
        transitionType = _transType;
    }

    protected void Awake()
    {
        // �C���X�^���X�擾
        if (sceneManagerInstance == null)
        {
            DontDestroyOnLoad(gameObject);
            sceneManagerInstance = this.gameObject;
        }
        // �V�[���}�l�[�W���[���������݂���ꍇ�́A�C���X�^���X�擾���Ă��Ȃ��I�u�W�F�N�g���폜
        else if (sceneManagerInstance != gameObject)
        {
            gameObject.SetActive(false);
            GameObject.Destroy(gameObject);
        }

        // �V�[���ݒ�R���|�[�l���g�擾
        if (sceneSettings == null) sceneSettings = GetComponent<SceneSettings>();
        // ���݃A�N�e�B�u�ȃ}�l�[�W���[���擾
        if (activeManager == null && this.enabled) activeManager = this;
    }
    protected virtual void Update()
    {
        // ���݂̃V�[����Ԃɂ���āA��������
        switch (currentSceneState)
        {
            // ��������
            case SceneState.Init:
                StateInit(); break;
            // �J�n��
            case SceneState.Start:
                StateStart(); break;
            // ���s����
            case SceneState.Running:
                StateRunning(); break;
            // �I����
            case SceneState.End:
                StateEnd(); break;
            // �㏈����
            case SceneState.Uninit:
                StateUninit(); break;
        }
    }
    /// <summary>
    /// ��������Ԃ̏���(base.StateInit()���s���ۂ͍Ō���ōs�����ƁI)    </summary>
    protected virtual void StateInit() { currentSceneState = SceneState.Start; }
    /// <summary>
    /// �J�n��Ԃ̏���(base.StateStart()���s���ۂ͍Ō���ōs�����ƁI)    </summary>
    protected virtual void StateStart() { currentSceneState = SceneState.Running; }
    /// <summary>
    /// ���s����Ԃ̏���(base.StateRunning()���s���ۂ͍Ō���ōs�����ƁI)    </summary>
    protected virtual void StateRunning() { currentSceneState = SceneState.End; }
    /// <summary>
    /// �I����Ԃ̏���(base.StateEnd()���s���ۂ͍Ō���ōs�����ƁI)    </summary>
    protected virtual void StateEnd() { currentSceneState = SceneState.Uninit; }
    /// <summary>
    /// �㏈����Ԃ̏���(base.StateUninit()���s���ۂ͍Ō���ōs�����ƁI)    </summary>
    protected virtual void StateUninit() { SceneTransition(nextScenetype, isChangeScene, transitionType); }


    /// <summary>
    /// �V�[���J��    </summary>
    /// <param name="_nextSceneType">
    /// ���ɑJ�ڂ���V�[���^�C�v    </param>
    /// <param name="isChangeScene">
    /// �V�[����؂�ւ��邩�ǂ���    </param>
    /// <param name="_transType">
    /// �J�ڃ^�C�v    </param>
    public void SceneTransition(SceneType _nextSceneType, bool _isChangeScene = false, TransitionType _transType = TransitionType.None)
    {
        // �㏈����ԂŖ�����΁A��Ԃ��ЂƂi�߂�
        if (currentSceneState != SceneState.Uninit)
        {
            SetSceneTransitionSettings(_nextSceneType, _isChangeScene, _transType);
            currentSceneState++;
        }
        else
        {
            // �J�ڃ^�C�v�ɂ���āA��������
            switch (_transType)
            {
                // �ʏ�
                default:
                    ChangeSceneManager(_nextSceneType);
                    if (_isChangeScene) ChangeScene(_nextSceneType);

                    // ���݁E�O��̃V�[���^�C�v��ۑ�
                    lateSceneType = currentSceneType;
                    currentSceneType = _nextSceneType;
                    nextScenetype = SceneType.None;
                    
                    currentSceneState = SceneState.Init;    // �V�[���̏�ԑJ��
                    break;
            }
        }
    }
    /// <summary>
    /// �V�[���}�l�[�W���[�̐؂�ւ�    </summary>
    /// <param name="_nextSceneType">
    /// ���ɑJ�ڂ���V�[���^�C�v    </param>
    private void ChangeSceneManager(SceneType _nextSceneType)
    {
        // ���݂̃}�l�[�W���[�͔�A�N�e�B�u��
        activeManager.enabled = false;

        // �J�ڂ���V�[���^�C�v�ɑΉ������}�l�[�W���[���Z�b�g
        switch (_nextSceneType)
        {
            // �^�C�g��
            case SceneType.Title:
                activeManager = GetComponent<TitleManager>(); break;
            // �V���O���Q�[��
            case SceneType.SingleGame:
                activeManager = GetComponent<SingleGameManager>(); break;
            // ���U���g
            case SceneType.Result:
                activeManager = GetComponent<ResultManager>(); break;
            // �Q�[���I��
            case SceneType.Quit:
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#else
                Application.Quit();
#endif
                break;
        }
        activeManager.enabled = true;   // �A�N�e�B�u��
    }
    /// <summary>
    /// �V�[���̐؂�ւ�    </summary>
    /// <param name="_nextSceneType">
    /// ���ɑJ�ڂ���V�[���^�C�v    </param>
    private void ChangeScene(SceneType _nextSceneType)
    {
        // �e�V�[���ݒ���擾
        foreach (SceneSettings.SceneSetting sceneSetting in sceneSettings.GetSceneSetting())
        {
            // �J�ڂ���V�[���^�C�v�ƈ�v������A�ݒ肳�ꂽ�V�[�����̃V�[���ɐ؂�ւ���
            if (sceneSetting.GetSceneType() == _nextSceneType)
            {
                SceneManager.LoadScene(sceneSetting.GetSceneName());
            }
        }
    }
}
