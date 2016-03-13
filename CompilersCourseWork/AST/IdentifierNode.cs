using CompilersCourseWork.Parsing;

namespace CompilersCourseWork.AST
{
    public class IdentifierNode : Node
    {
        private readonly string name;
        private VariableType type;


        public IdentifierNode(int line, int column, string name) : base(line, column)
        {
            this.name = name;
        }

        public string Name
        {
            get
            {
                return name;
            }
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            var asIdentifierNode = obj as IdentifierNode;
            if (asIdentifierNode == null || asIdentifierNode.Name != Name)
            {
                return false;
            }

            return true;
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }

        public override string ToString()
        {
            return "IdentifierNode - " + name;
        }

        public override void Accept(NodeVisitor visitor)
        {
            visitor.Visit(this);
        }

        internal override void SetType(VariableType type)
        {
            this.type = type;
        }

        public override VariableType NodeType()
        {
            return type;
        }
    }
}
