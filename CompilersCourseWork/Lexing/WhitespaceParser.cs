using System;
using System.Collections.Generic;
using CompilersCourseWork.Tokens;
using CompilersCourseWork.ErrorHandling;

namespace CompilersCourseWork.Lexing
{
    internal class WhitespaceParser : TokenParser
    {
        private ISet<char> whitespace;


        internal WhitespaceParser(TextReader reader, ErrorReporter reporter) : base(reader, reporter)
        {
            whitespace = new HashSet<char>();
            whitespace.Add(' ');
            whitespace.Add('\t');
            whitespace.Add('\n');
        }

        internal override bool Parses(char character)
        {
            return whitespace.Contains(character);
        }

        protected override Token DoParse()
        {
            while (Reader.PeekCharacter().HasValue &&
                whitespace.Contains(Reader.PeekCharacter().Value))
            {
                Reader.NextCharacter();
            }

            return null;
        }


    }
}
