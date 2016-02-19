namespace CompilersCourseWork.Tokens
{
    public class VarToken : KeywordToken
    {
        protected override string GetKeyword()
        {
            return "var";
        }
    }
}
