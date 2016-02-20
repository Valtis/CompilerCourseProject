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
    public class TextTokenTests
    {
        [TestMethod()]
        public void StringRepresentationIsCorrect()
        {
            var token = new TextToken("Hello");
            token.Line = 5;
            token.Column = 25;

            Assert.AreEqual("<text - 'Hello'>", token.ToString());
            
        }

        [TestMethod()]
        public void TwoTokensWithSameContentAreEqual()
        {
            var token = new TextToken("Hello");
            token.Line = 5;
            token.Column = 25;

            var token2 = new TextToken("Hello");
            token2.Line = 32;
            token2.Column = 251;
            Assert.AreEqual(token, token2);
        }

        [TestMethod()]
        public void TwoTokensWithDifferentContentAreNotEqual()
        {
            var token = new TextToken("Hello");
            token.Line = 5;
            token.Column = 25;

            var token2 = new TextToken("World");
            token2.Line = 32;
            token2.Column = 251;
            Assert.AreNotEqual(token, token2);
        }
    }
}