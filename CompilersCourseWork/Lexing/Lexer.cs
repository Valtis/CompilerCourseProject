using CompilersCourseWork;
using CompilersCourseWork.ErrorHandling;
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


        public Lexer(string path, ErrorReporter reporter, int spaces_per_tab=4)
        {
            reader = new TextReader(path, spaces_per_tab);
            reporter.Lines = reader.Lines;
               
            whitespaceParser = new WhitespaceParser(reader, reporter);

            parsers = new List<TokenParser>();
            parsers.Add(new IdentifierAndKeywordParser(reader, reporter));
            parsers.Add(new IntegerParser(reader, reporter));
            parsers.Add(new StringParser(reader, reporter));

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

            if (!character.HasValue)
            {
                return new EOFToken();
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
