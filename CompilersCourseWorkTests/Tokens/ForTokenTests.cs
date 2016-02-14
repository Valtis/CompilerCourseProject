using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CompilersCourseWork.Tokens.Tests
{
    [TestClass()]
    public class ForTokenTests
    {
        [TestMethod()]
        public void ToStringReturnsCorrectRepresentation()
        {
            var token = new ForToken();
            token.Line = 20;
            token.Column = 40;

            Assert.AreEqual("<Keyword:20,40;for>", token.ToString()); 
        }

        [TestMethod()]
        public void TwoTokensAreEqual()
        {
            var token = new ForToken();
            token.Line = 5;
            token.Column = 20;


            var token2 = new ForToken();
            token2.Line = 44;
            token2.Column = 23;

            Assert.AreEqual(token, token2);
        }
    }
}