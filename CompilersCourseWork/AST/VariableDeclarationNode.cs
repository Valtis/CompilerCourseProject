using CompilersCourseWork.Parsing;

namespace CompilersCourseWork.AST
{
    public class VariableDeclarationNode : Node
    {
        private readonly string name;
        private readonly VariableType type;
        
        public VariableDeclarationNode(int line, int column, string name, VariableType type) : base(line, column)
        {
            this.name = name;
            this.type = type;
        }

        public string Name
        {
            get
            {
                return name;
            }
        }

        public VariableType Type
        {
            get
            {
                return type;
            }
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            var asVariableNode = obj as VariableDeclarationNode;
            if (asVariableNode == null || asVariableNode.Name != Name || asVariableNode.Type != Type)
            {
                return false;
            }

            return true;
        }

        public override int GetHashCode()
        {
            return 3*Name.GetHashCode() + 7*Type.GetHashCode();
        }
    }
}
