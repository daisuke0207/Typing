namespace Arsene.Typing
{
    // 出題する問題を選定する
    public class OutputQuestionSelector
    {
        private readonly TypingQuestionList _questionList;

        public TypingQuestion Random()
        {
            // 問題の選択
            var index = UnityEngine.Random.Range(0, _questionList.Count() - 1);
            return _questionList.GetByIndex(index);
        }
    }
}