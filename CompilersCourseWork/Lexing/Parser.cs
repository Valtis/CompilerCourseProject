using CompilersCourseWork.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompilersCourseWork.Lexing
{
    internal abstract class Parser
    {
        private readonly TextReader reader;

        internal TextReader Reader
        {
            get
            {
                return reader;
            }
        }

        internal Parser(TextReader reader)
        {
            this.reader = reader;
        }

        internal Optional<Token> Parse()
        {
            var line = reader.Line;
            var column = reader.Column;

            var token = DoParse();
            if (token.HasValue)
            {
                token.Value.Line = line;
                token.Value.Column = column;
            }

            return token;
        }

        internal abstract bool Parses(char character);
        protected abstract Optional<Token> DoParse();
    }
}
