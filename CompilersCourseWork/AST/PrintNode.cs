using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompilersCourseWork.AST
{
    public class PrintNode : Node
    {
        public PrintNode(int line, int column, Node expression) : base(line, column)
        {
            AddChild(expression);
        }
    }
}
