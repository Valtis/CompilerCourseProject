namespace CompilersCourseWork.Tokens
{
    public class EndToken : KeywordToken
    {
        protected override string GetKeyword()
        {
            return "end";
        }
    }
}
