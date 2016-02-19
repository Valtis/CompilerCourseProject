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
        private ErrorReporter reporter;
        private IList<TokenParser> parsers;

        // whitespace parsing (ignoring, really) is special cased, as this is done before
        // regular tokenization to remove any whitespace.

        public Lexer(string path, ErrorReporter reporter, int spaces_per_tab=4)
        {
            reader = new TextReader(path, spaces_per_tab);
            this.reporter = reporter;
            reporter.Lines = reader.Lines;
               

            parsers = new List<TokenParser>();
            parsers.Add(new WhitespaceParser(reader, reporter));
            parsers.Add(new CommentParser(reader, reporter));
            parsers.Add(new IdentifierAndKeywordParser(reader, reporter));
            parsers.Add(new IntegerParser(reader, reporter));
            parsers.Add(new StringParser(reader, reporter));
            parsers.Add(new OperatorParser(reader, reporter));

        }

        public Token NextToken()
        {
            Token token = null;

            while (token == null || 
                token.GetType() == typeof(WhitespaceToken) ||
                token.GetType() == typeof(CommentToken))
            {
                token = GetToken();
            }

            return token;
        }
        
        private Token GetToken()
        {
            var line = reader.Line;
            var column = reader.Column;
            var character = reader.PeekCharacter();

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


            reporter.ReportError(
                Error.LEXICAL_ERROR,
                "Invalid start for token: " + "'" + character.Value + "'",
                line,
                column
                );

            reader.NextCharacter();
            // force next level to get next token
            return new WhitespaceToken();
        }       
    }
}
