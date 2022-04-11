using UnityEngine;
using UnityEngine.SceneManagement;

public class RootEntryPoint
{
    /// <summary>
    /// 起動時の処理
    /// </summary>
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void Setup()
    {
        // if (!SceneManager.GetSceneByBuildIndex((int)SceneID.MANAGER).IsValid())
        // {
        //     SceneManager.LoadSceneAsync((int)SceneID.MANAGER, LoadSceneMode.Additive);
        // }
#if UNITY_EDITOR || DEVELOPMENT_BUILD
        if (!SceneManager.GetSceneByBuildIndex(1).IsValid())
        {
            SceneManager.LoadSceneAsync("Debug", LoadSceneMode.Additive);
        }
#endif
#if !UNITY_EDITOR && UNITY_WEBGL
        // disable WebGLInput.captureAllKeyboardInput so elements in web page can handle keyboard inputs
        WebGLInput.captureAllKeyboardInput = false;
#endif
    }
}
