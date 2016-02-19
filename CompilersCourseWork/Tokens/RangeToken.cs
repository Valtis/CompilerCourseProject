namespace CompilersCourseWork.Tokens
{
    public class RangeToken : OperatorToken
    {
        protected override string GetOperator()
        {
            return "..";
        }
    }
}
