using Microsoft.VisualStudio.TestTools.UnitTesting;
using CompilersCourseWork.Tokens;
using CompilersCourseWork.Lexing;

namespace CompilersCourseWork.Tests
{
    [TestClass()]
    public class LexerTests
    {

        [TestMethod()]
        public void GetTokenReturnsNoneOnEndOfFile()
        {
            var lexer = new Lexer(@"..\..\empty.txt");
            Assert.AreEqual(Optional<Token>.None, lexer.GetToken());
        }


        [TestMethod()]
        public void GetTokenHandlesIdentifiers()
        {
            var lexer = new Lexer(@"..\..\valid_identifiers.txt");

            Assert.AreEqual(new IdentifierToken("hello"), lexer.GetToken().Value);
            Assert.AreEqual(new IdentifierToken("w_or_ld"), lexer.GetToken().Value);
            Assert.AreEqual(new IdentifierToken("this_"), lexer.GetToken().Value);
            Assert.AreEqual(new IdentifierToken("i123s"), lexer.GetToken().Value);
            Assert.AreEqual(new IdentifierToken("a987"), lexer.GetToken().Value);
            Assert.AreEqual(new IdentifierToken("t_12_es_190t"), lexer.GetToken().Value);
        }

        [TestMethod()]
        public void GetTokenHandlesReservedWords()
        {
            var lexer = new Lexer(@"..\..\keywords.txt");

            Assert.AreEqual(new VarToken(), lexer.GetToken().Value);
            Assert.AreEqual(new ForToken(), lexer.GetToken().Value);
            Assert.AreEqual(new EndToken(), lexer.GetToken().Value);
            Assert.AreEqual(new InToken(), lexer.GetToken().Value);
            Assert.AreEqual(new DoToken(), lexer.GetToken().Value);
            Assert.AreEqual(new ReadToken(), lexer.GetToken().Value);
            Assert.AreEqual(new PrintToken(), lexer.GetToken().Value);
            Assert.AreEqual(new IntToken(), lexer.GetToken().Value);
            Assert.AreEqual(new StringToken(), lexer.GetToken().Value);
            Assert.AreEqual(new BoolToken(), lexer.GetToken().Value);
            Assert.AreEqual(new AssertToken(), lexer.GetToken().Value);
        }

        [TestMethod()]
        public void GetTokenSetsLineAndColumnCorrectlyWith4SpacesPerTab()
        {
            var lexer = new Lexer(@"..\..\line_and_column.txt", 4);

            Token token;

            token = lexer.GetToken().Value;
            Assert.AreEqual(0, token.Line);
            Assert.AreEqual(0, token.Column);

            token = lexer.GetToken().Value;
            Assert.AreEqual(0, token.Line);
            Assert.AreEqual(6, token.Column);

            token = lexer.GetToken().Value;
            Assert.AreEqual(2, token.Line);
            Assert.AreEqual(0, token.Column);

            token = lexer.GetToken().Value;
            Assert.AreEqual(4, token.Line);
            Assert.AreEqual(4, token.Column);

            token = lexer.GetToken().Value;
            Assert.AreEqual(5, token.Line);
            Assert.AreEqual(8, token.Column);
        }

        [TestMethod()]
        public void GetTokenSetsLineAndColumnCorrectlyWith8SpacesPerTab()
        {
            var lexer = new Lexer(@"..\..\line_and_column.txt", 8);

            Token token;

            token = lexer.GetToken().Value;
            Assert.AreEqual(0, token.Line);
            Assert.AreEqual(0, token.Column);

            token = lexer.GetToken().Value;
            Assert.AreEqual(0, token.Line);
            Assert.AreEqual(6, token.Column);

            token = lexer.GetToken().Value;
            Assert.AreEqual(2, token.Line);
            Assert.AreEqual(0, token.Column);

            token = lexer.GetToken().Value;
            Assert.AreEqual(4, token.Line);
            Assert.AreEqual(8, token.Column);

            token = lexer.GetToken().Value;
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