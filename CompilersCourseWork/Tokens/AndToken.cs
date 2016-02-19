namespace CompilersCourseWork.Tokens
{
    public class AndToken : OperatorToken
    {
        protected override string GetOperator()
        {
            return "&";
        }
    }
}