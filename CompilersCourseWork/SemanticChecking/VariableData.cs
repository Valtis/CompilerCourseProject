using CompilersCourseWork.AST;

namespace CompilersCourseWork.SemanticChecking
{
    /*
    Class storing the variable id and declaration node, which contains things like type and name
    */
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