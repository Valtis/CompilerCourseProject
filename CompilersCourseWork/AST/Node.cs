using CompilersCourseWork.Parsing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompilersCourseWork.AST
{
    /*
    Base class for AST nodes
    */
    public abstract class Node
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

        // generic equality for nodes - checks if the two nodes have same type (in general we do not consider line or column
        // when considering equality)
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

        public abstract void Accept(NodeVisitor visitor);

        // Returns node type. Used for nodes like IdentifierNode (->integer) or AddNode (intger or string, based on operands)
        // only nodes that need type info implement this, other nodes just throw
        public virtual VariableType NodeType()
        {
            throw new InternalCompilerError("Invalid node type for variable type parameter");
        }
        // as above, but a setter
        internal virtual void SetType(VariableType type)
        {
            throw new InternalCompilerError("Invalid node type for variable type parameter");
        }
    }
}
