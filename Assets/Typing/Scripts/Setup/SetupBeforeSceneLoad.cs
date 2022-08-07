using UnityEngine;

namespace Arsene.Typing.Setup
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
            ProductionEnvironment.SetUp();
            DevelopmentEnvironment.SetUp();
        }
    }
}
