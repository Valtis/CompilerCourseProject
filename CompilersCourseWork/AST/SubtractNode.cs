﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompilersCourseWork.AST
{
    public class SubtractNode : Node
    {
        public SubtractNode(int line, int column, Node lhs, Node rhs) : base(line, column)
        {
            Children.Add(lhs);
            Children.Add(rhs);
        }
    }
}
