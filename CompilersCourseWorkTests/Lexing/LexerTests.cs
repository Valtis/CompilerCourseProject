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
            Assert.AreEqual(new IdentifierToken("world"), lexer.GetToken().Value);
            Assert.AreEqual(new IdentifierToken("this"), lexer.GetToken().Value);
            Assert.AreEqual(new IdentifierToken("is"), lexer.GetToken().Value);
            Assert.AreEqual(new IdentifierToken("a"), lexer.GetToken().Value);
            Assert.AreEqual(new IdentifierToken("test"), lexer.GetToken().Value);
        }

        [TestMethod()]
        public void GetTokenHandlesReservedWords()
        {
            var lexer = new Lexer(@"..\..\valid_identifiers.txt");

            Assert.AreEqual(new ForToken(), lexer.GetToken().Value);


            Assert.Fail();
          /*  Assert.AreEqual(new IdentifierToken("world"), lexer.GetToken().Value);
            Assert.AreEqual(new IdentifierToken("this"), lexer.GetToken().Value);
            Assert.AreEqual(new IdentifierToken("is"), lexer.GetToken().Value);
            Assert.AreEqual(new IdentifierToken("a"), lexer.GetToken().Value);
            Assert.AreEqual(new IdentifierToken("test"), lexer.GetToken().Value);*/
        }

        [TestMethod()]
        public void GetTokenSetsLineAndColumnCorrectly()
        {
            Assert.Fail();
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