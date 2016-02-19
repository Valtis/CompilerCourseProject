namespace CompilersCourseWork.Tokens
{
    public class StringToken : KeywordToken
    {
        protected override string GetKeyword()
        {
            return "string";
        }
    }
}
