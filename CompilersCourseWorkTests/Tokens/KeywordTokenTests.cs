using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CompilersCourseWork.Tokens.Tests
{
    [TestClass()]
    public class KeywordTokenTests
    {
        [TestMethod()]
        public void ToStringReturnsCorrectRepresentation()
        {
            var token = new ForToken();
            token.Line = 20;
            token.Column = 40;

            Assert.AreEqual("<Keyword - 'for'>", token.ToString());

            var token2 = new VarToken();
            Assert.AreEqual("<Keyword - 'var'>", token2.ToString());
        }

        [TestMethod()]
        public void TwoSameKeywordTokensAreEqual()
        {
            var token = new ForToken();
            token.Line = 5;
            token.Column = 20;


            var token2 = new ForToken();
            token2.Line = 44;
            token2.Column = 23;

            Assert.AreEqual(token, token2);
        }


        [TestMethod()]
        public void TwoDifferentKeywordTokensAreNotEqual()
        {
            var token = new ForToken();
            token.Line = 5;
            token.Column = 20;


            var token2 = new VarToken();
            token2.Line = 44;
            token2.Column = 23;

            Assert.AreNotEqual(token, token2);
        }
    }
}