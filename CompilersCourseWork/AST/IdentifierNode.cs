using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompilersCourseWork.AST
{
    public class IdentifierNode : Node
    {
        private readonly string name;

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
    }
}
