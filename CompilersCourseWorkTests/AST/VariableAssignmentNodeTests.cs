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
    public class VariableAssignmentNodeTests
    {
        [TestMethod()]
        public void TwoVariableAssignmentNodesWithSameVariableNameAreEqual()
        {
            Assert.AreEqual(
                new VariableAssignmentNode(1, 2, "hello"),
                new VariableAssignmentNode(9, 345, "hello")
                );
        }

        [TestMethod()]
        public void TwoVariableAssignmentNodesWithDifferentVariableNameAreNotEqual()
        {
            Assert.AreNotEqual(
                new VariableAssignmentNode(1, 2, "hello"),
                new VariableAssignmentNode(9, 345, "world")
                );
        }


        [TestMethod()]
        public void HashCodeIsSameForVariableAssignmentNodesWithSameVariable()
        {
            Assert.AreEqual(
                new VariableAssignmentNode(1, 2, "hello").GetHashCode(),
                new VariableAssignmentNode(9, 345, "hello").GetHashCode()
                );
        }
    }
}