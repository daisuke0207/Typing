namespace Arsene.Typing
{
    public class TypingQuestion
    {
        public readonly string Furigana;
        public readonly string Question;

        public TypingQuestion(string furigana, string question)
        {
            Furigana = furigana;
            Question = question;
        }
    }
}