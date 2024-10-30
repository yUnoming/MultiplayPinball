using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerBase : MonoBehaviour
{
    /// <summary>
    /// 遷移タイプ    </summary>
    public enum TransitionType
    {
        /// <summary>
        /// 特になし    </summary>
        None,
        /// <summary>
        /// フェード    </summary>
        Fade,
    }
    /// <summary>
    /// シーンの状態    </summary>
    public enum SceneState
    {
        /// <summary>
        /// 特になし    </summary>
        None,
        /// <summary>
        /// 初期化    </summary>
        Init,
        /// <summary>
        /// 開始    </summary>
        Start,
        /// <summary>
        /// シーン内処理中    </summary>
        Running,
        /// <summary>
        /// 終了    </summary>
        End,
        /// <summary>
        /// 後処理    </summary>
        Uninit,
        /// <summary>
        /// 一時停止中    </summary>
        Pause
    }

    private static GameObject sceneManagerInstance; // シーンマネージャーのインスタンス
    private static SceneSettings sceneSettings;     // シーン設定コンポーネント
    private static SceneManagerBase activeManager; // 現在アクティブなマネージャー

    private static bool isChangeScene;
    private static TransitionType transitionType;

    protected static SceneState currentSceneState = SceneState.Init; // 現在のシーン状態

    protected static SceneType nextScenetype = SceneType.None;        // 遷移先シーンタイプ
    protected static SceneType currentSceneType = SceneType.Title;    // 現在のシーンタイプ
    protected static SceneType lateSceneType;                         // 前回のシーンタイプ

    public static SceneState CurrentSceneState { get { return currentSceneState; } }
    public static SceneType NextSceneType { get { return nextScenetype; } }
    public static SceneType CurrentSceneType { get { return currentSceneType; } }
    public static SceneType LateSceneType { get { return lateSceneType; } }

    protected void SetSceneTransitionSettings(SceneType _nextSceneType, bool _isChangeScene = false, TransitionType _transType = TransitionType.None)
    {
        // 一時保存
        nextScenetype = _nextSceneType;
        isChangeScene = _isChangeScene;
        transitionType = _transType;
    }

    protected void Awake()
    {
        // インスタンス取得
        if (sceneManagerInstance == null)
        {
            DontDestroyOnLoad(gameObject);
            sceneManagerInstance = this.gameObject;
        }
        // シーンマネージャーが複数存在する場合は、インスタンス取得していないオブジェクトを削除
        else if (sceneManagerInstance != gameObject)
        {
            gameObject.SetActive(false);
            GameObject.Destroy(gameObject);
        }

        // シーン設定コンポーネント取得
        if (sceneSettings == null) sceneSettings = GetComponent<SceneSettings>();
        // 現在アクティブなマネージャーを取得
        if (activeManager == null && this.enabled) activeManager = this;
    }
    protected virtual void Update()
    {
        // 現在のシーン状態によって、処理分岐
        switch (currentSceneState)
        {
            // 初期化時
            case SceneState.Init:
                StateInit(); break;
            // 開始時
            case SceneState.Start:
                StateStart(); break;
            // 実行中時
            case SceneState.Running:
                StateRunning(); break;
            // 終了時
            case SceneState.End:
                StateEnd(); break;
            // 後処理時
            case SceneState.Uninit:
                StateUninit(); break;
        }
    }
    /// <summary>
    /// 初期化状態の処理(base.StateInit()を行う際は最後尾で行うこと！)    </summary>
    protected virtual void StateInit() { currentSceneState = SceneState.Start; }
    /// <summary>
    /// 開始状態の処理(base.StateStart()を行う際は最後尾で行うこと！)    </summary>
    protected virtual void StateStart() { currentSceneState = SceneState.Running; }
    /// <summary>
    /// 実行中状態の処理(base.StateRunning()を行う際は最後尾で行うこと！)    </summary>
    protected virtual void StateRunning() { currentSceneState = SceneState.End; }
    /// <summary>
    /// 終了状態の処理(base.StateEnd()を行う際は最後尾で行うこと！)    </summary>
    protected virtual void StateEnd() { currentSceneState = SceneState.Uninit; }
    /// <summary>
    /// 後処理状態の処理(base.StateUninit()を行う際は最後尾で行うこと！)    </summary>
    protected virtual void StateUninit() { SceneTransition(nextScenetype, isChangeScene, transitionType); }


    /// <summary>
    /// シーン遷移    </summary>
    /// <param name="_nextSceneType">
    /// 次に遷移するシーンタイプ    </param>
    /// <param name="isChangeScene">
    /// シーンを切り替えるかどうか    </param>
    /// <param name="_transType">
    /// 遷移タイプ    </param>
    public void SceneTransition(SceneType _nextSceneType, bool _isChangeScene = false, TransitionType _transType = TransitionType.None)
    {
        // 後処理状態で無ければ、状態をひとつ進める
        if (currentSceneState != SceneState.Uninit)
        {
            SetSceneTransitionSettings(_nextSceneType, _isChangeScene, _transType);
            currentSceneState++;
        }
        else
        {
            // 遷移タイプによって、処理分岐
            switch (_transType)
            {
                // 通常
                default:
                    ChangeSceneManager(_nextSceneType);
                    if (_isChangeScene) ChangeScene(_nextSceneType);

                    // 現在・前回のシーンタイプを保存
                    lateSceneType = currentSceneType;
                    currentSceneType = _nextSceneType;
                    nextScenetype = SceneType.None;
                    
                    currentSceneState = SceneState.Init;    // シーンの状態遷移
                    break;
            }
        }
    }
    /// <summary>
    /// シーンマネージャーの切り替え    </summary>
    /// <param name="_nextSceneType">
    /// 次に遷移するシーンタイプ    </param>
    private void ChangeSceneManager(SceneType _nextSceneType)
    {
        // 現在のマネージャーは非アクティブ化
        activeManager.enabled = false;

        // 遷移するシーンタイプに対応したマネージャーをセット
        switch (_nextSceneType)
        {
            // タイトル
            case SceneType.Title:
                activeManager = GetComponent<TitleManager>(); break;
            // シングルゲーム
            case SceneType.SingleGame:
                activeManager = GetComponent<SingleGameManager>(); break;
            // リザルト
            case SceneType.Result:
                activeManager = GetComponent<ResultManager>(); break;
            // ゲーム終了
            case SceneType.Quit:
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#else
                Application.Quit();
#endif
                break;
        }
        activeManager.enabled = true;   // アクティブ化
    }
    /// <summary>
    /// シーンの切り替え    </summary>
    /// <param name="_nextSceneType">
    /// 次に遷移するシーンタイプ    </param>
    private void ChangeScene(SceneType _nextSceneType)
    {
        // 各シーン設定を取得
        foreach (SceneSettings.SceneSetting sceneSetting in sceneSettings.GetSceneSetting())
        {
            // 遷移するシーンタイプと一致したら、設定されたシーン名のシーンに切り替える
            if (sceneSetting.GetSceneType() == _nextSceneType)
            {
                SceneManager.LoadScene(sceneSetting.GetSceneName());
            }
        }
    }
}
