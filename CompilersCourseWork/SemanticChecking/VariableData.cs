using CompilersCourseWork.AST;

namespace CompilersCourseWork.SemanticChecking
{
    public class VariableData
    {
        public readonly int id;
        public readonly VariableDeclarationNode node;

        public VariableData(int id, VariableDeclarationNode node)
        {
            this.id = id;
            this.node = node;
        }

    }
}