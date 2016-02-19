namespace CompilersCourseWork.Tokens
{
    public class InToken : KeywordToken
    {
        protected override string GetKeyword()
        {
            return "in";
        }
    }
}
