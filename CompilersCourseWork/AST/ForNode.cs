namespace CompilersCourseWork.AST
{
    public class ForNode : Node
    {
        public ForNode(
            int line, 
            int column, 
            Node loopVariable,
            Node startExpression,
            Node stopExpression,
            Node statements) : base(line, column)
        {
            Children.Add(loopVariable);
            Children.Add(startExpression);
            Children.Add(stopExpression);
            Children.Add(statements);
        }

        public override void Accept(NodeVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
