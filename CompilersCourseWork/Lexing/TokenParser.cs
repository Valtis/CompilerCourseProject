using CompilersCourseWork.Tokens;

namespace CompilersCourseWork.Lexing
{
    internal abstract class TokenParser
    {
        private readonly TextReader reader;

        internal TextReader Reader
        {
            get
            {
                return reader;
            }
        }

        internal TokenParser(TextReader reader)
        {
            this.reader = reader;
        }

        internal Token Parse()
        {
            var line = reader.Line;
            var column = reader.Column;

            var token = DoParse();
            if (token != null)
            {
                token.Line = line;
                token.Column = column;
            }

            return token;
        }

        internal abstract bool Parses(char character);
        protected abstract Token DoParse();
    }
}
