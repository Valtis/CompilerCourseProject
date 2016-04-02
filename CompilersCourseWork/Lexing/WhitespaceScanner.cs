using System;
using System.Collections.Generic;
using CompilersCourseWork.Tokens;
using CompilersCourseWork.ErrorHandling;

namespace CompilersCourseWork.Lexing
{
    /*
    Scan and discard whitespace
    */
    internal class WhitespaceScanner : TokenScanner
    {
        private ISet<char> whitespace;


        internal WhitespaceScanner(TextReader reader, ErrorReporter reporter) : base(reader, reporter)
        {
            whitespace = new HashSet<char>();
            whitespace.Add(' ');
            whitespace.Add('\t');
            whitespace.Add('\n');
        }

        internal override bool Recognizes(char character)
        {
            return whitespace.Contains(character);
        }

        protected override Token DoScan()
        {
            while (Reader.PeekCharacter().HasValue &&
                whitespace.Contains(Reader.PeekCharacter().Value))
            {
                Reader.NextCharacter();
            }

            return new WhitespaceToken();
        }


    }
}
