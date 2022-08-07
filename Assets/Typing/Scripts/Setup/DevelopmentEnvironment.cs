using UnityEngine.SceneManagement;

namespace Arsene.Typing.Setup
{
    public class DevelopmentEnvironment
    {
        public static void SetUp()
        {
            // Debugシーンをロードする
#if UNITY_EDITOR || DEVELOPMENT_BUILD
            if (!SceneManager.GetSceneByBuildIndex(1).IsValid())
            {
                SceneManager.LoadSceneAsync("Debug", LoadSceneMode.Additive);
            }
#endif
        }
    }
}