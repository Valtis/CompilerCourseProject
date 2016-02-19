namespace CompilersCourseWork.Tokens
{
    public class ComparisonToken : OperatorToken
    {
        protected override string GetOperator()
        {
            return "=";
        }
    }
}