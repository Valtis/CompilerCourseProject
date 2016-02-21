using Microsoft.VisualStudio.TestTools.UnitTesting;
using CompilersCourseWork.AST;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompilersCourseWork.AST.Tests
{
    [TestClass()]
    public class StringNodeTests
    {
        [TestMethod()]
        public void TwoStringNodesWithSameValueAreEqual()
        {
            Assert.AreEqual(new StringNode(0, 1, "hello"), new StringNode(234, 23, "hello"));
        }

        [TestMethod()]
        public void TwoStringNodesWithDifferentValueAreNotEqual()
        {
            Assert.AreNotEqual(new StringNode(0, 1, "hello"), new StringNode(234, 23, "world"));
        }

        [TestMethod()]
        public void TwoStringNodesWithSameValueHaveSameHashcode()
        {
            Assert.AreEqual(
                new StringNode(0, 1, "hello").GetHashCode(), 
                new StringNode(234, 23, "hello").GetHashCode());
        }
    }
}