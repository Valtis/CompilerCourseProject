using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CompilersCourseWork.Parsing;

namespace CompilersCourseWork.AST
{
    public class AddNode : Node
    {
        private VariableType type;

        public AddNode(int line, int column, Node lhs, Node rhs) : base(line, column)
        {
            Children.Add(lhs);
            Children.Add(rhs);
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
