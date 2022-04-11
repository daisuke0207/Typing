using UnityEngine;

namespace Arsene.Typing
{
    public enum QuestionCategory
    {
        Animal,
        Sweets,
    }

    /// <summary>
    /// TypingObject
    /// </summary>
    [CreateAssetMenu(menuName = "ScriptableObject/Typing")]
    public class TypingObject : ScriptableObject
    {
        public QuestionCategory category;
        public TextAsset fTextAsset;
        public TextAsset qTextAsset;
    }
}
