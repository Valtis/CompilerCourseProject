namespace CompilersCourseWork.AST
{
    public class PrintNode : Node
    {
        public PrintNode(int line, int column, Node expression) : base(line, column)
        {
            AddChild(expression);
        }

        public override void Accept(NodeVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
