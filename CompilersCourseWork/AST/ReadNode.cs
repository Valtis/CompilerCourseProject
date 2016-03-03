using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompilersCourseWork.AST
{
    public class ReadNode : Node
    {
        public ReadNode(int line, int column, IdentifierNode identifier) : base(line, column)
        {
            Children.Add(identifier);
        }
    }
}
