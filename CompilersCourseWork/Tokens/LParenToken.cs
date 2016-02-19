namespace CompilersCourseWork.Tokens
{
    public class LParenToken : OperatorToken
    {
        protected override string GetOperator()
        {
            return "(";
        }
    }
}
