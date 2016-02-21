using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CompilersCourseWork.AST.Tests
{
    [TestClass()]
    public class IdentifierNodeTests
    {
        [TestMethod()]
        public void TwoIdentifiersWithSameNameAreEqual()
        {
            Assert.AreEqual(
                new IdentifierNode(65, 23,"foo"), 
                new IdentifierNode(76, 23, "foo"));
        }

        [TestMethod()]
        public void TwoIdentifiersWithDifferentNameAreNotEqual()
        {
            Assert.AreNotEqual(
                new IdentifierNode(65, 23, "foo"),
                new IdentifierNode(76, 23, "bar"));
        }

        [TestMethod()]
        public void TwoIdentifiersWithSameNameHaveSameHashcode()
        {
            Assert.AreEqual(
                new IdentifierNode(4, 1453, "foo").GetHashCode(), 
                new IdentifierNode(0, 12, "foo").GetHashCode());
        }
    }
}