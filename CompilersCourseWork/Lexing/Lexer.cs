using CompilersCourseWork.ErrorHandling;
using CompilersCourseWork.Tokens;
using System.Collections.Generic;

namespace CompilersCourseWork.Lexing
{
    public class Lexer
    {
        public const int BACKTRACK_BUFFER_SIZE = 4;

        private TextReader reader;
        private ErrorReporter reporter;
        private IList<TokenScanner> parsers;

        // maintain a list of previously read tokens in order to allow backtracking if needed
        private BacktrackBuffer backTrackBuffer;

        public IList<string> Lines
        {
            get
            {
                return reader.Lines;
            }
        }
        
        public Lexer(string path, ErrorReporter reporter, int spacesPerTab=8)
        {

            reader = new TextReader(path, spacesPerTab);
            this.reporter = reporter;
            reporter.Lines = reader.Lines;

            backTrackBuffer = new BacktrackBuffer(BACKTRACK_BUFFER_SIZE);
           
            parsers = new List<TokenScanner>();
            parsers.Add(new WhitespaceScanner(reader, reporter));
            parsers.Add(new CommentScanner(reader, reporter));
            parsers.Add(new IdentifierAndKeywordScanner(reader, reporter));
            parsers.Add(new IntegerScanner(reader, reporter));
            parsers.Add(new StringScanner(reader, reporter));
            parsers.Add(new OperatorScanner(reader, reporter));
        }

        
        public Token NextToken()
        {

            if (!backTrackBuffer.Empty())
            {
                return backTrackBuffer.GetToken();
            }

            Token token = null;
            while (token == null ||
                token.GetType() == typeof(WhitespaceToken) ||
                token.GetType() == typeof(CommentToken))
            {
                token = GetToken();
            }

            backTrackBuffer.AddToken(token);
            return token;
        }

        public Token PeekToken()
        {
            if (!backTrackBuffer.Empty())
            {
                return backTrackBuffer.PeekToken();
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

                backTrackBuffer.AddToken(token);
                backTrackBuffer.Backtrack();
                return backTrackBuffer.PeekToken();
            }
        }

        public void Backtrack()
        {
            backTrackBuffer.Backtrack();            
        }

        private Token GetToken()
        {
            var line = reader.Line;
            var column = reader.Column;
            var character = reader.PeekCharacter();

            if (!character.HasValue)
            {
                var eof = new EOFToken();
                eof.Line = reader.Lines.Length - 1;
                if (eof.Line != -1)
                {
                    eof.Column = reader.Lines[eof.Line].Length - 1;
                }
                else
                {
                    eof.Column = 0;
                }
                return eof;
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
