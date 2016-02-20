using CompilersCourseWork.Parsing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompilersCourseWork.AST
{
    public class VariableNode : Node
    {
        private readonly string name;
        private readonly VariableType type;
        
        public VariableNode(int line, int column, string name, VariableType type) : base(line, column)
        {
            this.name = name;
            this.type = type;
        }

        public string Name
        {
            get
            {
                return name;
            }
        }

        public VariableType Type
        {
            get
            {
                return type;
            }
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            var asVariableNode = obj as VariableNode;
            if (asVariableNode == null || asVariableNode.Name != Name || asVariableNode.Type != Type)
            {
                return false;
            }

            return true;
        }

        public override int GetHashCode()
        {
            return 3*Name.GetHashCode() + 7*Type.GetHashCode();
        }
    }
}
