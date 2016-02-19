namespace CompilersCourseWork.Tokens
{
    public class IntToken : KeywordToken
    {
        protected override string GetKeyword()
        {
            return "int";
        }
    }
}
