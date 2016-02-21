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
    public class IntegerNodeTests
    {
        [TestMethod()]
        public void TwoIntegerNodesWithSameValueAreEqual()
        {
            Assert.AreEqual(new IntegerNode(0, 2, 50), new IntegerNode(21, 1, 50));
        }

        [TestMethod()]
        public void TwoIntegerNodesWithDifferentValueAreNotEqual()
        {
            Assert.AreNotEqual(new IntegerNode(0, 2, 70), new IntegerNode(21, 1, 50));
        }

        [TestMethod()]
        public void TwoIntegerNodesWithSameValueHaveSameHashCode()
        {
            Assert.AreEqual(new IntegerNode(0, 2, 50).GetHashCode(), 
                new IntegerNode(21, 1, 50).GetHashCode());
        }
    }
}