using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompilersCourseWork.AST
{
    public class StringNode : Node
    {
        private readonly string value;

        public StringNode(int line, int column, string value) : base(line, column)
        {
            this.value = value;
        }

        public string Value
        {
            get
            {
                return value;
            }
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            if (obj as StringNode == null)
            {
                return false;
            }

            return (obj as StringNode).Value == Value;
        }


        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }
    }
}
