using UnityEngine;
using UnityEngine.SceneManagement;

namespace Arsene.Typing
{
    public static class SetupBeforeSceneLoad
    {
        /// <summary>
        /// 起動時の処理
        /// はじめのシーンがロードされる前に処理される
        /// </summary>
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Setup()
        {
            // 開発時はDebugシーンをロードする
#if UNITY_EDITOR || DEVELOPMENT_BUILD
            if (!SceneManager.GetSceneByBuildIndex(1).IsValid())
            {
                SceneManager.LoadSceneAsync("Debug", LoadSceneMode.Additive);
            }
#endif
            
            // WebGLの設定
#if !UNITY_EDITOR && UNITY_WEBGL
        // disable WebGLInput.captureAllKeyboardInput so elements in web page can handle keyboard inputs
        WebGLInput.captureAllKeyboardInput = false;
#endif
        }
    }
}
