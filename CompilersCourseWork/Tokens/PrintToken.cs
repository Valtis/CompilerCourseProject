namespace CompilersCourseWork.Tokens
{
    public class PrintToken : KeywordToken
    {
        protected override string GetKeyword()
        {
            return "print";
        }
    }
}
