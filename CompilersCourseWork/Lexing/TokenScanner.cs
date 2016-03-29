using CompilersCourseWork.ErrorHandling;
using CompilersCourseWork.Tokens;

namespace CompilersCourseWork.Lexing
{
    internal abstract class TokenScanner
    {
        private readonly TextReader reader;
        private ErrorReporter reporter;

        internal TextReader Reader
        {
            get
            {
                return reader;
            }
        }

        public ErrorReporter Reporter
        {
            get
            {
                return reporter;
            }

            set
            {
                reporter = value;
            }
        }

        internal TokenScanner(TextReader reader, ErrorReporter reporter)
        {
            this.reader = reader;
            this.Reporter = reporter;
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
