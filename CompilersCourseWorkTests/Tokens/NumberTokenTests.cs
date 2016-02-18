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
    public class NumberTokenTests
    {

        [TestMethod()]
        public void StringRepresentationIsCorrect()
        {
            var token = new NumberToken(654);
            Assert.AreEqual("<number:0,0;654>", token.ToString());
        }
   
        [TestMethod()]
        public void TwoIntegerTokensWithSameValueAreEqual()
        {
            var token = new NumberToken(654);
            var token2 = new NumberToken(654);

            Assert.AreEqual(token, token2);
        }

        [TestMethod()]
        public void TwoIntegerTokensWitDifferentValueAreNotEqual()
        {
            var token = new NumberToken(654);
            var token2 = new NumberToken(123);

            Assert.AreNotEqual(token, token2);
        }

    }
}