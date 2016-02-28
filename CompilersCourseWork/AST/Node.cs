using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompilersCourseWork.AST
{
    public class Node
    {
        private IList<Node> children;
        private readonly int line;
        private readonly int column;

        public int Line
        {
            get
            {
                return line;
            }
        }

        public int Column
        {
            get
            {
                return column;
            }
        }

        public IList<Node> Children
        {
            get
            {
                return children;
            }

            private set
            {
                children = value;
            }
        }

        public Node(int line, int column)
        {
            this.line = line;
            this.column = column;
            Children = new List<Node>();
        }


        public void AddChild(Node node)
        {
            Children.Add(node);
        }


        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            return obj.GetType().Equals(GetType());
        }

        public override int GetHashCode()
        {
            return GetType().ToString().GetHashCode();
        }
    }
}
