using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Arsene.Typing.ScriptableObjects
{
    public enum QuestionCategory
    {
        Animal,
        Sweets,
    }

    /// <summary>
    /// タイピング問題のアセット
    /// </summary>
    [CreateAssetMenu(menuName = "ScriptableObject/Typing")]
    public class TypingQuestionObject : ScriptableObject
    {
        public QuestionCategory category;
        public TextAsset fTextAsset;
        public TextAsset qTextAsset;
        
        public List<string> GetQuestions()
        {
            return qTextAsset.text.Split('\n').ToList();
        }

        public List<string> GetFuriganas()
        {
            return fTextAsset.text.Split('\n').ToList();
        }
    }
}
