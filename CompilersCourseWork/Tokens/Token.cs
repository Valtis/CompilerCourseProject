using System;

namespace CompilersCourseWork.Tokens
{
    public abstract class Token
    {
        private int line;
        private int column;

        public Token()
        {
            Line = 0;
            Column = 0;
        }

        public int Line
        {
            get
            {
                return line;
            }

            set
            {
                line = value;
            }
        }

        public int Column
        {
            get
            {
                return column;
            }

            set
            {
                column = value;
            }
        }

        public override string ToString()
        {
            var repr = GetStringRepresentation();
            string ret =  "<" + repr.Item1;
            if (repr.Item2.Length != 0)
            {
                ret += " - '" + repr.Item2 + "'";
            }
            return ret + ">";
        }
         
                
        protected abstract Tuple<String, String> GetStringRepresentation();
    }
}
