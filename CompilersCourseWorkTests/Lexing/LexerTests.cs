using Microsoft.VisualStudio.TestTools.UnitTesting;
using CompilersCourseWork.Tokens;
using CompilersCourseWork.Lexing;
using CompilersCourseWork.ErrorHandling;

namespace CompilersCourseWork.Tests
{
    [TestClass()]
    public class LexerTests
    {

        [TestMethod()]
        public void GetTokenReturnsEOFOnEndOfFile()
        {
            var reporter = new ErrorReporter();
            var lexer = new Lexer(@"..\..\Lexing\empty.txt", reporter);
            Assert.AreEqual(new EOFToken(), lexer.NextToken());
            Assert.AreEqual(0, reporter.Errors.Count);
        }


        [TestMethod()]
        public void GetTokenHandlesIdentifiers()
        {
            var reporter = new ErrorReporter();
            var lexer = new Lexer(@"..\..\Lexing\valid_identifiers.txt", reporter);

            Assert.AreEqual(new IdentifierToken("hello"), lexer.NextToken());
            Assert.AreEqual(new IdentifierToken("w_or_ld"), lexer.NextToken());
            Assert.AreEqual(new IdentifierToken("this_"), lexer.NextToken());
            Assert.AreEqual(new IdentifierToken("i123s"), lexer.NextToken());
            Assert.AreEqual(new IdentifierToken("a987"), lexer.NextToken());
            Assert.AreEqual(new IdentifierToken("t_12_es_190t"), lexer.NextToken());
            Assert.AreEqual(0, reporter.Errors.Count);
        }

        [TestMethod()]
        public void GetTokenHandlesReservedWords()
        {
            var reporter = new ErrorReporter();
            var lexer = new Lexer(@"..\..\Lexing\keywords.txt", reporter);

            Assert.AreEqual(new VarToken(), lexer.NextToken());
            Assert.AreEqual(new ForToken(), lexer.NextToken());
            Assert.AreEqual(new EndToken(), lexer.NextToken());
            Assert.AreEqual(new InToken(), lexer.NextToken());
            Assert.AreEqual(new DoToken(), lexer.NextToken());
            Assert.AreEqual(new ReadToken(), lexer.NextToken());
            Assert.AreEqual(new PrintToken(), lexer.NextToken());
            Assert.AreEqual(new IntToken(), lexer.NextToken());
            Assert.AreEqual(new StringToken(), lexer.NextToken());
            Assert.AreEqual(new BoolToken(), lexer.NextToken());
            Assert.AreEqual(new AssertToken(), lexer.NextToken());
            Assert.AreEqual(0, reporter.Errors.Count);
        }

        [TestMethod()]
        public void GetTokensHandlesIntegers()
        {
            var reporter = new ErrorReporter();
            var lexer = new Lexer(@"..\..\Lexing\valid_integers.txt", reporter);

            Assert.AreEqual(new NumberToken(123), lexer.NextToken());
            Assert.AreEqual(new NumberToken(0), lexer.NextToken());
            Assert.AreEqual(new NumberToken(9876543210), lexer.NextToken());
            Assert.AreEqual(0, reporter.Errors.Count);
        }

        [TestMethod()]
        public void GetTokensHandlesInvalidIntegers()
        {
            var reporter = new ErrorReporter();
            var lexer = new Lexer(@"..\..\Lexing\invalid_integers.txt", reporter);

            Assert.AreEqual(new NumberToken(567), lexer.NextToken());
            Assert.AreEqual(new IdentifierToken("Hello"), lexer.NextToken());
            Assert.AreEqual(new NumberToken(1), lexer.NextToken());

            Assert.AreEqual(2, reporter.Errors.Count);

            Assert.AreEqual(Error.LEXICAL_ERROR, reporter.Errors[0].Type);
            Assert.AreEqual(0, reporter.Errors[0].Line);
            Assert.AreEqual(3, reporter.Errors[0].Column);
            Assert.IsTrue(reporter.Errors[0].Message.ToLower().Contains("invalid character"));


            Assert.AreEqual(Error.LEXICAL_ERROR, reporter.Errors[1].Type);
            Assert.AreEqual(1, reporter.Errors[1].Line);
            Assert.AreEqual(0, reporter.Errors[1].Column);
            Assert.IsTrue(reporter.Errors[1].Message.ToLower().Contains("number does not fit"));
        }

        [TestMethod()]
        public void GetTokenHandlesValidStrings()
        {
            var reporter = new ErrorReporter();
            var lexer = new Lexer(@"..\..\Lexing\valid_strings.txt", reporter);

            Assert.AreEqual(new TextToken("hello"), lexer.NextToken());
            Assert.AreEqual(new TextToken("wor\tld"), lexer.NextToken());
            Assert.AreEqual(new TextToken("thi\ns"), lexer.NextToken());
            Assert.AreEqual(new TextToken("\"is\""), lexer.NextToken());
            Assert.AreEqual(new TextToken("a\rtest"), lexer.NextToken());
            Assert.AreEqual(0, reporter.Errors.Count);
        }

        [TestMethod()]
        public void GetTokenHandlesInvalidStrings()
        {
            var reporter = new ErrorReporter();
            var lexer = new Lexer(@"..\..\Lexing\invalid_strings.txt", reporter);

            Assert.AreEqual(new TextToken("invaliddescape"), lexer.NextToken());
            Assert.AreEqual(new TextToken("unterminated"), lexer.NextToken());
            Assert.AreEqual(new TextToken("valid for checking unterminated"), lexer.NextToken());

            Assert.AreEqual(2, reporter.Errors.Count);

            Assert.AreEqual(Error.LEXICAL_ERROR, reporter.Errors[0].Type);
            Assert.AreEqual(0, reporter.Errors[0].Line);
            Assert.AreEqual(9, reporter.Errors[0].Column);
            Assert.IsTrue(reporter.Errors[0].Message.ToLower().Contains("invalid escape sequence"));


            Assert.AreEqual(Error.LEXICAL_ERROR, reporter.Errors[1].Type);
            Assert.AreEqual(1, reporter.Errors[1].Line);
            Assert.AreEqual(13, reporter.Errors[1].Column);
            Assert.IsTrue(reporter.Errors[1].Message.ToLower().Contains("unmatched"));
        }

        [TestMethod()]
        public void GetTokenParsesOperatorsCorrectly()
        {
            var reporter = new ErrorReporter();
            var lexer = new Lexer(@"..\..\Lexing\operators.txt", reporter);

            Assert.AreEqual(new PlusToken(), lexer.NextToken());
            Assert.AreEqual(new MinusToken(), lexer.NextToken());
            Assert.AreEqual(new MultiplyToken(), lexer.NextToken());
            Assert.AreEqual(new DivideToken(), lexer.NextToken());
            Assert.AreEqual(new AssignmentToken(), lexer.NextToken());
            Assert.AreEqual(new LessThanToken(), lexer.NextToken());
            Assert.AreEqual(new ComparisonToken(), lexer.NextToken());
            Assert.AreEqual(new AndToken(), lexer.NextToken());
            Assert.AreEqual(new NotToken(), lexer.NextToken());
            Assert.AreEqual(new SemicolonToken(), lexer.NextToken());
            Assert.AreEqual(new ColonToken(), lexer.NextToken());
            Assert.AreEqual(new RangeToken(), lexer.NextToken());
            Assert.AreEqual(new LParenToken(), lexer.NextToken());
            Assert.AreEqual(new RParenToken(), lexer.NextToken());
            Assert.AreEqual(0, reporter.Errors.Count);
        }

        [TestMethod()]
        public void GetTokenParsesHandlesInvalidOperators()
        {
            var reporter = new ErrorReporter();
            var lexer = new Lexer(@"..\..\Lexing\invalid_operators.txt", reporter);

            Assert.AreEqual(new IdentifierToken("this"), lexer.NextToken());
            Assert.AreEqual(new IdentifierToken("contains"), lexer.NextToken());
            Assert.AreEqual(new RangeToken(), lexer.NextToken());
            Assert.AreEqual(new IdentifierToken("invalid_operators"), lexer.NextToken());

            Assert.AreEqual(2, reporter.Errors.Count);

            Assert.AreEqual(Error.LEXICAL_ERROR, reporter.Errors[0].Type);
            Assert.AreEqual(0, reporter.Errors[0].Line);
            Assert.AreEqual(5, reporter.Errors[0].Column);
            Assert.IsTrue(reporter.Errors[0].Message.ToLower().Contains("invalid operator"));


            Assert.AreEqual(Error.LEXICAL_ERROR, reporter.Errors[1].Type);
            Assert.AreEqual(0, reporter.Errors[1].Line);
            Assert.AreEqual(17, reporter.Errors[1].Column);
            Assert.IsTrue(reporter.Errors[1].Message.ToLower().Contains("invalid operator"));
        }
                
        [TestMethod()]
        public void GetTokenParsesHandlesInvalidCharacters()
        {
            var reporter = new ErrorReporter();
            var lexer = new Lexer(@"..\..\Lexing\invalid_characters.txt", reporter);

            Assert.AreEqual(new IdentifierToken("hello"), lexer.NextToken());
            Assert.AreEqual(new IdentifierToken("there"), lexer.NextToken());
            Assert.AreEqual(new TextToken("just"), lexer.NextToken());
            Assert.AreEqual(new IdentifierToken("testing"), lexer.NextToken());
            Assert.AreEqual(new NumberToken(1234), lexer.NextToken());

            Assert.AreEqual(4, reporter.Errors.Count);

            Assert.AreEqual(Error.LEXICAL_ERROR, reporter.Errors[0].Type);
            Assert.AreEqual(0, reporter.Errors[0].Line);
            Assert.AreEqual(0, reporter.Errors[0].Column);
            Assert.IsTrue(reporter.Errors[0].Message.ToLower().Contains("invalid start"));

            Assert.AreEqual(Error.LEXICAL_ERROR, reporter.Errors[1].Type);
            Assert.AreEqual(0, reporter.Errors[1].Line);
            Assert.AreEqual(6, reporter.Errors[1].Column);
            Assert.IsTrue(reporter.Errors[1].Message.ToLower().Contains("invalid start"));

            Assert.AreEqual(Error.LEXICAL_ERROR, reporter.Errors[2].Type);
            Assert.AreEqual(0, reporter.Errors[2].Line);
            Assert.AreEqual(18, reporter.Errors[2].Column);
            Assert.IsTrue(reporter.Errors[2].Message.ToLower().Contains("invalid start"));

            Assert.AreEqual(Error.LEXICAL_ERROR, reporter.Errors[3].Type);
            Assert.AreEqual(0, reporter.Errors[3].Line);
            Assert.AreEqual(26, reporter.Errors[3].Column);
            Assert.IsTrue(reporter.Errors[3].Message.ToLower().Contains("invalid start"));
        }

        [TestMethod()]
        public void GetTokenParsesCommentsCorrectly()
        {
            var reporter = new ErrorReporter();
            var lexer = new Lexer(@"..\..\Lexing\comments.txt", reporter);

            Assert.AreEqual(new IdentifierToken("valid_token"), lexer.NextToken());
            Assert.AreEqual(new DivideToken(), lexer.NextToken());
            Assert.AreEqual(new IdentifierToken("anothertoken"), lexer.NextToken());
            Assert.AreEqual(new NumberToken(1234), lexer.NextToken());
            Assert.AreEqual(new TextToken("test"), lexer.NextToken());
            Assert.AreEqual(new EOFToken(), lexer.NextToken());
            Assert.AreEqual(0, reporter.Errors.Count);
        }

        [TestMethod()]
        public void GetTokenSetsLineAndColumnCorrectlyWith4SpacesPerTab()
        {
            var reporter = new ErrorReporter();
            var lexer = new Lexer(@"..\..\Lexing\line_and_column.txt", reporter, 4);

            Token token;

            token = lexer.NextToken();
            Assert.AreEqual(0, token.Line);
            Assert.AreEqual(0, token.Column);

            token = lexer.NextToken();
            Assert.AreEqual(0, token.Line);
            Assert.AreEqual(6, token.Column);

            token = lexer.NextToken();
            Assert.AreEqual(2, token.Line);
            Assert.AreEqual(0, token.Column);

            token = lexer.NextToken();
            Assert.AreEqual(4, token.Line);
            Assert.AreEqual(4, token.Column);

            token = lexer.NextToken();
            Assert.AreEqual(5, token.Line);
            Assert.AreEqual(8, token.Column);
            Assert.AreEqual(0, reporter.Errors.Count);
        }

        [TestMethod()]
        public void GetTokenSetsLineAndColumnCorrectlyWith8SpacesPerTab()
        {
            var reporter = new ErrorReporter();
            var lexer = new Lexer(@"..\..\Lexing\line_and_column.txt", reporter, 8);

            Token token;

            token = lexer.NextToken();
            Assert.AreEqual(0, token.Line);
            Assert.AreEqual(0, token.Column);

            token = lexer.NextToken();
            Assert.AreEqual(0, token.Line);
            Assert.AreEqual(6, token.Column);

            token = lexer.NextToken();
            Assert.AreEqual(2, token.Line);
            Assert.AreEqual(0, token.Column);

            token = lexer.NextToken();
            Assert.AreEqual(4, token.Line);
            Assert.AreEqual(8, token.Column);

            token = lexer.NextToken();
            Assert.AreEqual(5, token.Line);
            Assert.AreEqual(16, token.Column);
            Assert.AreEqual(0, reporter.Errors.Count);
        }

        [TestMethod()]
        public void LexerPeekingWorks()
        {
            var reporter = new ErrorReporter();
            var lexer = new Lexer(@"..\..\example_program.txt", reporter);

            Assert.AreEqual(new PrintToken(), lexer.PeekToken());
            Assert.AreEqual(new PrintToken(), lexer.PeekToken());
            Assert.AreEqual(new PrintToken(), lexer.NextToken());
            Assert.AreEqual(new TextToken("Give a number"), lexer.NextToken());
            Assert.AreEqual(new SemicolonToken(), lexer.PeekToken());
            Assert.AreEqual(new SemicolonToken(), lexer.NextToken());

            Assert.AreEqual(0, reporter.Errors.Count);
        }

        [TestMethod()]
        public void LexerPeekingWorksWithBacktracking()
        {
            var reporter = new ErrorReporter();
            var lexer = new Lexer(@"..\..\example_program.txt", reporter);

            lexer.NextToken();
            lexer.NextToken();
            lexer.NextToken();
            Assert.AreEqual(new VarToken(), lexer.PeekToken());

            lexer.Backtrack();
            Assert.AreEqual(new SemicolonToken(), lexer.PeekToken());
            Assert.AreEqual(new SemicolonToken(), lexer.NextToken());
            Assert.AreEqual(new VarToken(), lexer.PeekToken());
            Assert.AreEqual(new VarToken(), lexer.PeekToken());
            lexer.Backtrack();
            lexer.Backtrack();
            
            Assert.AreEqual(new TextToken("Give a number"), lexer.PeekToken());
            Assert.AreEqual(new TextToken("Give a number"), lexer.NextToken());
            Assert.AreEqual(0, reporter.Errors.Count);
        }


        [TestMethod()]
        public void LexerBackTrackingWorks()
        {
            var reporter = new ErrorReporter();
            var lexer = new Lexer(@"..\..\example_program.txt", reporter);

            Assert.AreEqual(new PrintToken(), lexer.NextToken());
            Assert.AreEqual(new TextToken("Give a number"), lexer.NextToken());
            Assert.AreEqual(new SemicolonToken(), lexer.NextToken());

            lexer.Backtrack();

            Assert.AreEqual(new SemicolonToken(), lexer.NextToken());

            lexer.Backtrack();
            lexer.Backtrack();

            Assert.AreEqual(new TextToken("Give a number"), lexer.NextToken());
            Assert.AreEqual(new SemicolonToken(), lexer.NextToken());
            Assert.AreEqual(0, reporter.Errors.Count);
        }

        [TestMethod()]
        public void LexerBackTrackingThrowsIfCalledTooManyTimes()
        {
            var reporter = new ErrorReporter();
            var lexer = new Lexer(@"..\..\example_program.txt", reporter);

            Assert.AreEqual(new PrintToken(), lexer.NextToken());
            Assert.AreEqual(new TextToken("Give a number"), lexer.NextToken());
            Assert.AreEqual(new SemicolonToken(), lexer.NextToken());

            for (int i = 0; i < Lexer.BACKTRACK_BUFFER_SIZE; ++i)
            {
                lexer.Backtrack();
            }

            bool throws = false;
            try
            {
               lexer.Backtrack();
            } catch (InternalCompilerErrorException e)
            {
                throws = true;
            }

            Assert.IsTrue(throws);
            Assert.AreEqual(0, reporter.Errors.Count);
        }

        [TestMethod()]
        public void LexerBackTrackingThrowsIfCalledWithNonFullBufferTooManyTimes()
        {
            var reporter = new ErrorReporter();
            var lexer = new Lexer(@"..\..\example_program.txt", reporter);

            bool throws = false;
            try
            {
               lexer.Backtrack();
            }
            catch (InternalCompilerErrorException e)
            {
                throws = true;
            }

            Assert.IsTrue(throws);
            Assert.AreEqual(0, reporter.Errors.Count);
        }


        [TestMethod()]
        public void ExampleProgramIsTokenizedCorrectly()
        {
            var reporter = new ErrorReporter();
            var lexer = new Lexer(@"..\..\example_program.txt", reporter, 8);
            TokenHelper(lexer, new PrintToken(), 0, 0);
            TokenHelper(lexer, new TextToken("Give a number"), 0, 6);
            TokenHelper(lexer, new SemicolonToken(), 0, 21);

            TokenHelper(lexer, new VarToken(), 1, 0);
            TokenHelper(lexer, new IdentifierToken("n"), 1, 4);
            TokenHelper(lexer, new ColonToken(), 1, 6);
            TokenHelper(lexer, new IntToken(), 1, 8);
            TokenHelper(lexer, new SemicolonToken(), 1, 11);

            TokenHelper(lexer, new ReadToken(), 2, 0);
            TokenHelper(lexer, new IdentifierToken("n"), 2, 5);
            TokenHelper(lexer, new SemicolonToken(), 2, 6);

            TokenHelper(lexer, new VarToken(), 3, 0);
            TokenHelper(lexer, new IdentifierToken("v"), 3, 4);
            TokenHelper(lexer, new ColonToken(), 3, 6);
            TokenHelper(lexer, new IntToken(), 3, 8);
            TokenHelper(lexer, new AssignmentToken(), 3, 12);
            TokenHelper(lexer, new NumberToken(1), 3, 15);
            TokenHelper(lexer, new SemicolonToken(), 3, 16);

            TokenHelper(lexer, new VarToken(), 4, 0);
            TokenHelper(lexer, new IdentifierToken("i"), 4, 4);
            TokenHelper(lexer, new ColonToken(), 4, 6);
            TokenHelper(lexer, new IntToken(), 4, 8);
            TokenHelper(lexer, new SemicolonToken(), 4, 11);

            TokenHelper(lexer, new ForToken(), 5, 0);
            TokenHelper(lexer, new IdentifierToken("i"), 5, 4);
            TokenHelper(lexer, new InToken(), 5, 6);
            TokenHelper(lexer, new NumberToken(1), 5, 9);
            TokenHelper(lexer, new RangeToken(), 5, 10);
            TokenHelper(lexer, new IdentifierToken("n"), 5, 12);
            TokenHelper(lexer, new DoToken(), 5, 14);

            TokenHelper(lexer, new IdentifierToken("v"), 6, 8);
            TokenHelper(lexer, new AssignmentToken(), 6, 10);
            TokenHelper(lexer, new IdentifierToken("v"), 6, 13);
            TokenHelper(lexer, new MultiplyToken(), 6, 15);
            TokenHelper(lexer, new IdentifierToken("i"), 6, 17);
            TokenHelper(lexer, new SemicolonToken(), 6, 18);

            TokenHelper(lexer, new EndToken(), 7, 0);
            TokenHelper(lexer, new ForToken(), 7, 4);
            TokenHelper(lexer, new SemicolonToken(), 7, 7);


            TokenHelper(lexer, new PrintToken(), 8, 0);
            TokenHelper(lexer, new TextToken("The result is: "), 8, 6);
            TokenHelper(lexer, new SemicolonToken(), 8, 23);

            TokenHelper(lexer, new PrintToken(), 9, 0);
            TokenHelper(lexer, new IdentifierToken("v"), 9, 6);
            TokenHelper(lexer, new SemicolonToken(), 9, 7);
            Assert.AreEqual(0, reporter.Errors.Count);
        }

        [TestMethod()]
        public void ExampleProgram2IsTokenizedCorrectly()
        {
            var reporter = new ErrorReporter();
            var lexer = new Lexer(@"..\..\example_program2.txt", reporter, 8);
            TokenHelper(lexer, new VarToken(), 0, 0);
            TokenHelper(lexer, new IdentifierToken("X"), 0, 4);
            TokenHelper(lexer, new ColonToken(), 0, 6);
            TokenHelper(lexer, new IntToken(), 0, 8);
            TokenHelper(lexer, new AssignmentToken(), 0, 12);
            TokenHelper(lexer, new NumberToken(4), 0, 15);
            TokenHelper(lexer, new PlusToken(), 0, 17);
            TokenHelper(lexer, new LParenToken(), 0, 19);
            TokenHelper(lexer, new NumberToken(6), 0, 20);
            TokenHelper(lexer, new MultiplyToken(), 0, 22);
            TokenHelper(lexer, new NumberToken(2), 0, 24);
            TokenHelper(lexer, new RParenToken(), 0, 25);
            TokenHelper(lexer, new SemicolonToken(), 0, 26);

            TokenHelper(lexer, new PrintToken(), 1, 0);
            TokenHelper(lexer, new IdentifierToken("X"), 1, 6);
            TokenHelper(lexer, new SemicolonToken(), 1, 7);
            Assert.AreEqual(0, reporter.Errors.Count);
        }

        private void TokenHelper(Lexer lexer, Token expected, int line, int column)
        {
            var actual = lexer.NextToken();
            Assert.AreEqual(expected, actual);
            Assert.AreEqual(line, actual.Line);
            Assert.AreEqual(column, actual.Column);
        }


        // invalid characters when token start expected
        // invalid operators (.. mostly)

    }
}