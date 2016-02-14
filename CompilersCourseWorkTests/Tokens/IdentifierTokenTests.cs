using Microsoft.VisualStudio.TestTools.UnitTesting;
using CompilersCourseWork.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompilersCourseWork.Tokens.Tests
{
    [TestClass()]
    public class IdentifierTokenTests
    {
        [TestMethod()]
        public void ToStringReturnsCorrectRepresentation()
        {
            var token = new IdentifierToken("Hello");
            token.Line = 5;
            token.Column = 20;

            Assert.AreEqual("<Identifier:5,20;Hello>", token.ToString());
        }


        [TestMethod()]
        public void TwoTokensWithSameIdentifierAreEqual()
        {
            var token = new IdentifierToken("Hello");
            token.Line = 5;
            token.Column = 20;


            var token2 = new IdentifierToken("Hello");
            token2.Line = 44;
            token2.Column = 23;

            Assert.AreEqual(token, token2);
        }

        [TestMethod()]
        public void TwoTokensWithDifferentIdentifierAreNotEqual()
        {
            var token = new IdentifierToken("Hello");
            token.Line = 5;
            token.Column = 20;


            var token2 = new IdentifierToken("HelloWorld");
            token2.Line = 44;
            token2.Column = 23;

            Assert.AreNotEqual(token, token2);
        }

        [TestMethod()]
        public void EqualityHandlesNull()
        {
            var token = new IdentifierToken("Hello");
            token.Line = 5;
            token.Column = 20;


            Assert.AreNotEqual(token, null);
            Assert.AreNotEqual(null, token);
        }

    }
}