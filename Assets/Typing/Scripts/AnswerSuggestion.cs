using System.Collections.Generic;

namespace Arsene.Typing
{
    public class AnswerSuggestion
    {
        private TypingDictionary _typingDictionary;

        private string _currentAnswer = ""; // conan

        // 回答を分割したリスト
        private List<string> _romSliceList = new List<string>(); // co na n

        // 現在の問題の一文字を参照するための変数
        private int _curAIndex = 0;

        // 回答文字数に合わせたふりがなのインデックス
        private List<int> _fIndexList = new List<int>(); // 0 0 1 1 2  => 進行中に変更される

        // 分割されたローマ字単位のインデックス
        private List<int> _romIndexList = new List<int>(); // 0 1 0 1 0  => 進行中に変更される

        public string Suggest()
        {
            // TODO: 答え
            return "";
        }
    }
}