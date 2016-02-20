using CompilersCourseWork.ErrorHandling;
using CompilersCourseWork.Tokens;
using System.Collections.Generic;

namespace CompilersCourseWork.Lexing
{
    public class Lexer
    {
        public const int BACKTRACK_BUFFER_SIZE = 3;

        private TextReader reader;
        private ErrorReporter reporter;
        private IList<TokenParser> parsers;

        // maintain a list of previously read tokens in order to allow backtracking if needed
        private BacktrackBuffer backtrack_buffer;
        

        
        // whitespace parsing (ignoring, really) is special cased, as this is done before
        // regular tokenization to remove any whitespace.

        public Lexer(string path, ErrorReporter reporter, int spaces_per_tab=4)
        {

            reader = new TextReader(path, spaces_per_tab);
            this.reporter = reporter;
            reporter.Lines = reader.Lines;

            backtrack_buffer = new BacktrackBuffer(BACKTRACK_BUFFER_SIZE);
           
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

            if (!backtrack_buffer.Empty())
            {
                return backtrack_buffer.GetToken();
            }

            Token token = null;
            while (token == null ||
                token.GetType() == typeof(WhitespaceToken) ||
                token.GetType() == typeof(CommentToken))
            {
                token = GetToken();
            }

            backtrack_buffer.AddToken(token);
            return token;
        }

        public Token PeekToken()
        {
            if (!backtrack_buffer.Empty())
            {
                return backtrack_buffer.PeekToken();
            }
            else
            {
                Token token = null;
                while (token == null ||
                   token.GetType() == typeof(WhitespaceToken) ||
                   token.GetType() == typeof(CommentToken))
                {
                    token = GetToken();
                }

                backtrack_buffer.AddToken(token);
                backtrack_buffer.Backtrack();
                return backtrack_buffer.PeekToken();
            }
        }

        public void Backtrack()
        {
            backtrack_buffer.Backtrack();            
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
