using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Arsene.Typing
{
    public class TypingView : MonoBehaviour
    {
        // 表示テキスト
        [SerializeField] private Text _furiganaText;
        [SerializeField] private Text _questionText;
        [SerializeField] private Text _answerText;
        
        private async void OnEnable()
        {
            await UniTask.WaitUntil(() => Keyboard.current != null);
            Keyboard.current.onTextInput += OnTextInput;
        }

        private void OnTextInput(char inputChar)
        {
            
        }
        
        public void OnDisable()
        {
            Keyboard.current.onTextInput -= OnTextInput;
        }
    }
}