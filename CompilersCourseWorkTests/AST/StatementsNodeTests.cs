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
    public class StatementsNodeTests
    {
        [TestMethod()]
        public void TwoStatementsNodesAreEqual()
        {
            var node1 = new StatementsNode(0, 1);
            var node2 = new StatementsNode(50, 23);

            Assert.AreEqual(node1, node2);            
        }

        [TestMethod()]
        public void TwoStatementsNodesHaveSameHashCode()
        {
            var node1 = new StatementsNode(4, 56);
            var node2 = new StatementsNode(9, 12);

            Assert.AreEqual(node1.GetHashCode(), node2.GetHashCode());
        }
    }
}