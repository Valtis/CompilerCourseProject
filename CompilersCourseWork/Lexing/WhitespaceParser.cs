using System;
using System.Collections.Generic;
using CompilersCourseWork.Tokens;

namespace CompilersCourseWork.Lexing
{
    internal class WhitespaceParser : Parser
    {
        private ISet<char> whitespace;


        internal WhitespaceParser(TextReader reader) : base(reader)
        {
            whitespace = new HashSet<char>();
            whitespace.Add(' ');
            whitespace.Add('\t');
            whitespace.Add('\n');

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

        internal override bool Parses(char character)
        {
            return whitespace.Contains(character);
        }   
    }
}
