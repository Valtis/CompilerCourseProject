using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CompilersCourseWork.Parsing;

namespace CompilersCourseWork.AST
{
    public class ErrorNode : Node
    {
        public ErrorNode() : base(0, 0)
        {

        }

        public override void Accept(NodeVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override VariableType NodeType()
        {
            return VariableType.ERROR_TYPE;
        }
    }
}
