namespace Arsene.Typing.Setup
{
    public class ProductionEnvironment
    {
        public static void SetUp()
        {
            // WebGLの設定
#if !UNITY_EDITOR && UNITY_WEBGL
        // disable WebGLInput.captureAllKeyboardInput so elements in web page can handle keyboard inputs
        WebGLInput.captureAllKeyboardInput = false;
#endif
        }

    }
}