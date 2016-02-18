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
            var lexer = new Lexer(@"..\..\empty.txt", new ErrorReporter());
            Assert.AreEqual(new EOFToken(), lexer.GetToken());
        }


        [TestMethod()]
        public void GetTokenHandlesIdentifiers()
        {
            var lexer = new Lexer(@"..\..\valid_identifiers.txt", new ErrorReporter());

            Assert.AreEqual(new IdentifierToken("hello"), lexer.GetToken());
            Assert.AreEqual(new IdentifierToken("w_or_ld"), lexer.GetToken());
            Assert.AreEqual(new IdentifierToken("this_"), lexer.GetToken());
            Assert.AreEqual(new IdentifierToken("i123s"), lexer.GetToken());
            Assert.AreEqual(new IdentifierToken("a987"), lexer.GetToken());
            Assert.AreEqual(new IdentifierToken("t_12_es_190t"), lexer.GetToken());
        }

        [TestMethod()]
        public void GetTokenHandlesReservedWords()
        {
            var lexer = new Lexer(@"..\..\keywords.txt", new ErrorReporter());

            Assert.AreEqual(new VarToken(), lexer.GetToken());
            Assert.AreEqual(new ForToken(), lexer.GetToken());
            Assert.AreEqual(new EndToken(), lexer.GetToken());
            Assert.AreEqual(new InToken(), lexer.GetToken());
            Assert.AreEqual(new DoToken(), lexer.GetToken());
            Assert.AreEqual(new ReadToken(), lexer.GetToken());
            Assert.AreEqual(new PrintToken(), lexer.GetToken());
            Assert.AreEqual(new IntToken(), lexer.GetToken());
            Assert.AreEqual(new StringToken(), lexer.GetToken());
            Assert.AreEqual(new BoolToken(), lexer.GetToken());
            Assert.AreEqual(new AssertToken(), lexer.GetToken());
        }
        
        [TestMethod()]
        public void GetTokensHandlesIntegers()
        {
            var lexer = new Lexer(@"..\..\valid_integers.txt", new ErrorReporter());

            Assert.AreEqual(new NumberToken(123), lexer.GetToken());
            Assert.AreEqual(new NumberToken(0), lexer.GetToken());
            Assert.AreEqual(new NumberToken(9876543210), lexer.GetToken());
        }

        [TestMethod()]
        public void GetTokensHandlesInvalidIntegers()
        {
            var reporter = new ErrorReporter();
            var lexer = new Lexer(@"..\..\invalid_integers.txt", reporter);

            Assert.AreEqual(new NumberToken(567), lexer.GetToken());
            Assert.AreEqual(new IdentifierToken("Hello"), lexer.GetToken());
            Assert.AreEqual(new NumberToken(1), lexer.GetToken());

            Assert.AreEqual(2, reporter.Errors.Count);

            Assert.AreEqual(Error.LEXICAL_ERROR, reporter.Errors[0].Type);
            Assert.AreEqual(0, reporter.Errors[0].Line);
            Assert.AreEqual(3, reporter.Errors[0].Column);
            Assert.IsTrue(reporter.Errors[0].Message.ToLower().Contains("invalid character"));


            Assert.AreEqual(Error.LEXICAL_ERROR, reporter.Errors[0].Type);
            Assert.AreEqual(1, reporter.Errors[1].Line);
            Assert.AreEqual(0, reporter.Errors[1].Column);
            Assert.IsTrue(reporter.Errors[1].Message.ToLower().Contains("number does not fit"));
        }

        [TestMethod()]
        public void GetTokenHandlesValidStrings()
        {
            var lexer = new Lexer(@"..\..\valid_strings.txt", new ErrorReporter());

            Assert.AreEqual(new TextToken("hello"), lexer.GetToken());
            Assert.AreEqual(new TextToken("wor\tld"), lexer.GetToken());
            Assert.AreEqual(new TextToken("thi\ns"), lexer.GetToken());
            Assert.AreEqual(new TextToken("\"is\""), lexer.GetToken());
            Assert.AreEqual(new TextToken("a\rtest"), lexer.GetToken());
        }

        [TestMethod()]
        public void GetTokenHandlesInvalidStrings()
        {
            var reporter = new ErrorReporter();
            var lexer = new Lexer(@"..\..\invalid_strings.txt", reporter);

            Assert.AreEqual(new TextToken("invaliddescape"), lexer.GetToken());
            Assert.AreEqual(new TextToken("unterminated"), lexer.GetToken());
            Assert.AreEqual(new TextToken("valid for checking unterminated"), lexer.GetToken());

            Assert.AreEqual(2, reporter.Errors.Count);

            Assert.AreEqual(Error.LEXICAL_ERROR, reporter.Errors[0].Type);
            Assert.AreEqual(0, reporter.Errors[0].Line);
            Assert.AreEqual(9, reporter.Errors[0].Column);
            Assert.IsTrue(reporter.Errors[0].Message.ToLower().Contains("invalid escape sequence"));


            Assert.AreEqual(Error.LEXICAL_ERROR, reporter.Errors[0].Type);
            Assert.AreEqual(1, reporter.Errors[1].Line);
            Assert.AreEqual(13, reporter.Errors[1].Column);
            Assert.IsTrue(reporter.Errors[1].Message.ToLower().Contains("unmatched"));
        }
        
        [TestMethod()]
        public void GetTokenSetsLineAndColumnCorrectlyWith4SpacesPerTab()
        {
            var lexer = new Lexer(@"..\..\line_and_column.txt", new ErrorReporter(), 4);

            Token token;

            token = lexer.GetToken();
            Assert.AreEqual(0, token.Line);
            Assert.AreEqual(0, token.Column);

            token = lexer.GetToken();
            Assert.AreEqual(0, token.Line);
            Assert.AreEqual(6, token.Column);

            token = lexer.GetToken();
            Assert.AreEqual(2, token.Line);
            Assert.AreEqual(0, token.Column);

            token = lexer.GetToken();
            Assert.AreEqual(4, token.Line);
            Assert.AreEqual(4, token.Column);

            token = lexer.GetToken();
            Assert.AreEqual(5, token.Line);
            Assert.AreEqual(8, token.Column);
        }

        [TestMethod()]
        public void GetTokenSetsLineAndColumnCorrectlyWith8SpacesPerTab()
        {
            var lexer = new Lexer(@"..\..\line_and_column.txt", new ErrorReporter(), 8);

            Token token;

            token = lexer.GetToken();
            Assert.AreEqual(0, token.Line);
            Assert.AreEqual(0, token.Column);

            token = lexer.GetToken();
            Assert.AreEqual(0, token.Line);
            Assert.AreEqual(6, token.Column);

            token = lexer.GetToken();
            Assert.AreEqual(2, token.Line);
            Assert.AreEqual(0, token.Column);

            token = lexer.GetToken();
            Assert.AreEqual(4, token.Line);
            Assert.AreEqual(8, token.Column);

            token = lexer.GetToken();
            Assert.AreEqual(5, token.Line);
            Assert.AreEqual(16, token.Column);
        }

        // comments
        // invalid identifiers
        // integers
        // invalid integers
        // strings
        // invalid strings
        // booleans
        // invalid booleans
        // operators
        // invalid operators

    }
}