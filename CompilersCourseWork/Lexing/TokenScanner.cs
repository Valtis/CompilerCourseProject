﻿using CompilersCourseWork.ErrorHandling;
using CompilersCourseWork.Tokens;

namespace CompilersCourseWork.Lexing
{
    /*
    Abstract base class for token scanners
    */
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

        /*
        Invokes DoParse-method of the child class. Adds line and column number into the token
        */
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

        // returns true if the scanner recognizes\scans the token
        internal abstract bool Parses(char character);
        // actually scans the token
        protected abstract Token DoParse();
    }
}
