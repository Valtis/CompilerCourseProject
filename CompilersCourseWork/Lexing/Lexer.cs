using CompilersCourseWork;
using CompilersCourseWork.Tokens;
using System;
using System.Collections.Generic;

namespace CompilersCourseWork.Lexing
{
    public class Lexer
    {
        private TextReader reader;
        private IList<TokenParser> parsers;
        // whitespace parsing (ignoring, really) is special cased, as this is done before
        // regular tokenization to remove any whitespace.
        private WhitespaceParser whitespaceParser;


        public Lexer(string path, int spaces_per_tab=4)
        {
            reader = new TextReader(path, spaces_per_tab);
            parsers = new List<TokenParser>();
            whitespaceParser = new WhitespaceParser(reader);
            parsers.Add(new IdentifierAndKeywordParser(reader));
           
        }

        public Token GetToken()
        {
            var character = reader.PeekCharacter();

            if (!character.HasValue)
            {
                return new EOFToken();
            }

            if (whitespaceParser.Parses(character.Value))
            {
                whitespaceParser.Parse();
                character = reader.PeekCharacter();
            }

            foreach (var parser in parsers)
            {
                if (parser.Parses(character.Value))
                {
                    return parser.Parse();
                }
            }
            
            throw new NotImplementedException("Not implemented");
        }       
    }
}
