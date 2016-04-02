using CompilersCourseWork.ErrorHandling;
using CompilersCourseWork.Tokens;
using System.Collections.Generic;

namespace CompilersCourseWork.Lexing
{

    /*
    Lexer. Coordinates token scanning and backtracking
    */
    public class Lexer
    {
        public const int BACKTRACK_BUFFER_SIZE = 4;

        private TextReader reader;
        private ErrorReporter reporter;
        private IList<TokenScanner> scanners;

        // maintain a list of previously read tokens in order to allow backtracking if needed
        private BacktrackBuffer backtrackBuffer;

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

            backtrackBuffer = new BacktrackBuffer(BACKTRACK_BUFFER_SIZE);
            
            scanners = new List<TokenScanner>();
            scanners.Add(new WhitespaceScanner(reader, reporter));
            scanners.Add(new CommentScanner(reader, reporter));
            scanners.Add(new IdentifierAndKeywordScanner(reader, reporter));
            scanners.Add(new IntegerScanner(reader, reporter));
            scanners.Add(new StringScanner(reader, reporter));
            scanners.Add(new OperatorScanner(reader, reporter));
        }

        
        public Token NextToken()
        {
            // if backtrack buffer has a token (we have backtracked at least once previously), 
            // use it instead
            if (!backtrackBuffer.Empty())
            {
                return backtrackBuffer.GetToken();
            }

            Token token = null;
            // discard whitespace and comment tokens
            while (token == null ||
                token.GetType() == typeof(WhitespaceToken) ||
                token.GetType() == typeof(CommentToken))
            {
                token = GetToken();
            }

            backtrackBuffer.AddToken(token);
            return token;
        }

        public Token PeekToken()
        {
            // if backtrack buffer has tokens (we have backtracked previously), use them instead
            if (!backtrackBuffer.Empty())
            {
                return backtrackBuffer.PeekToken();
            }

            Token token = null;
            while (token == null ||
                token.GetType() == typeof(WhitespaceToken) ||
                token.GetType() == typeof(CommentToken))
            {
                token = GetToken();
            }

            // add the token to the backtrack buffer, then immediately backtrack and peek the next token.
            // this means backtrack buffer is in valid state for next operation
            backtrackBuffer.AddToken(token);
            backtrackBuffer.Backtrack();
            return backtrackBuffer.PeekToken();
        }

        public void Backtrack()
        {
            backtrackBuffer.Backtrack();            
        }

        private Token GetToken()
        {
            var line = reader.Line;
            var column = reader.Column;
            var character = reader.PeekCharacter();

            // return EOF token, if text buffer is empty
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

            // check scanners one by one until we find a scanner that recognizes the token
            foreach (var parser in scanners)
            {
                if (parser.Recognizes(character.Value))
                {
                    return parser.Scan();
                }
            }

            // ...or report error if no scanner recognizes the token
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
