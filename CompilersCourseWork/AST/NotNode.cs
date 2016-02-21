using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompilersCourseWork.AST
{
    public class NotNode : Node
    {
        public NotNode(int line, int column, Node child) : base(line, column)
        {
            Children.Add(child);
        }
    }
}
