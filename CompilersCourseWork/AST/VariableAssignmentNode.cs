using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompilersCourseWork.AST
{
    public class VariableAssignmentNode : Node
    {
        private readonly string name;

        public VariableAssignmentNode(int line, int column, string name) : base(line, column)
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

            var asVariableNode = obj as VariableAssignmentNode;
            if (asVariableNode == null || asVariableNode.Name != Name)
            {
                return false;
            }

            return true;
        }

        public override int GetHashCode()
        {
            return 3 * Name.GetHashCode();
        }
    }
}
