namespace CompilersCourseWork.Tokens
{
    public class NotToken : OperatorToken
    {
        protected override string GetOperator()
        {
            return "!";
        }
    }
}