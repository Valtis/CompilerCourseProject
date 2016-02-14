using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            return "<" + repr.Item1 + ":" + Line + "," + Column + ";" + repr.Item2 + ">";
        }
        
        protected abstract Tuple<String, String> GetStringRepresentation();
    }
}
