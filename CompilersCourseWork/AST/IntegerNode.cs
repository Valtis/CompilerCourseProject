namespace CompilersCourseWork.AST
{
    public class IntegerNode : Node
    {
        private readonly long value;

        public IntegerNode(int line, int column, long value) : base(line, column)
        {
            this.value = value;
        }

        public long Value
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

            if (obj as IntegerNode == null)
            {
                return false;
            }

            return (obj as IntegerNode).Value == Value;
        }


        public override int GetHashCode()
        {
            return (int)(Value % int.MaxValue);
        }
    }
}
