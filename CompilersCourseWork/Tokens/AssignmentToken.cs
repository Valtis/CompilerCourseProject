namespace CompilersCourseWork.Tokens
{
    public class AssignmentToken : OperatorToken
    {
        protected override string GetOperator()
        {
            return ":=";
        }
    }
}