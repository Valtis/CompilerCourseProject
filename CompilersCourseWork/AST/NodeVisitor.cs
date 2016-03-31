namespace CompilersCourseWork.AST
{
    /*
    Interface for visitors
    */
    public interface NodeVisitor
    {
        void Visit(AddNode node);
        void Visit(AndNode node);
        void Visit(AssertNode node);
        void Visit(ComparisonNode node);
        void Visit(DivideNode node);
        void Visit(ErrorNode node);
        void Visit(ForNode node);
        void Visit(IdentifierNode node);
        void Visit(IntegerNode node);
        void Visit(LessThanNode node);
        void Visit(MultiplyNode node);
        void Visit(NotNode node);
        void Visit(PrintNode node);
        void Visit(ReadNode node);
        void Visit(StatementsNode node);
        void Visit(StringNode node);
        void Visit(SubtractNode node);
        void Visit(VariableAssignmentNode node);
        void Visit(VariableDeclarationNode node); 
    }
}
