using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CompilersCourseWork.Parsing;

namespace CompilersCourseWork.AST
{
    public class NotNode : Node
    {
        private VariableType type;
        public NotNode(int line, int column, Node child) : base(line, column)
        {
            Children.Add(child);
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
