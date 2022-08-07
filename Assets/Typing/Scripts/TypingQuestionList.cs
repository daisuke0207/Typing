using System;
using System.Collections.Generic;
using Arsene.Typing.ScriptableObjects;

namespace Arsene.Typing
{
    /// <summary>
    /// 問題リストを扱う
    /// </summary>
    [Serializable]
    public class TypingQuestionList
    {
        private TypingQuestionObject _questionObj;
        private List<string> _furiganas;
        private List<string> _questions;

        public TypingQuestionList(TypingQuestionObject questionObj)
        {
            _questionObj = questionObj;
            _furiganas = _questionObj.GetFuriganas();
            _questions = _questionObj.GetQuestions();
        }

        public int Count()
        {
            return _questions.Count;
        }

        public TypingQuestion GetByIndex(int index)
        {
            var furigana = _furiganas[index];
            var qusetion = _questions[index];
            return new TypingQuestion(furigana, qusetion);
        }
    }
}