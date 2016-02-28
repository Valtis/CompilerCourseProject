using Microsoft.VisualStudio.TestTools.UnitTesting;
using CompilersCourseWork.AST;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CompilersCourseWork.Parsing;

namespace CompilersCourseWork.AST.Tests
{
    [TestClass()]
    public class VariableNodeTests
    {
        [TestMethod()]
        public void TwoVariableNodesWithSameNameAndTypeAreEqual()
        {
            var node1 = new VariableDeclarationNode(1, 2, "test", VariableType.STRING);
            var node2 = new VariableDeclarationNode(71, 122, "test", VariableType.STRING);


            Assert.AreEqual(node1, node2);
        }

        [TestMethod()]
        public void TwoVariableNodesWithSameNameAndDifferentTypeAreNotEqual()
        {
            var node1 = new VariableDeclarationNode(1, 2, "test", VariableType.INTEGER);
            var node2 = new VariableDeclarationNode(71, 122, "test", VariableType.STRING);
            Assert.AreNotEqual(node1, node2);
        }

        [TestMethod()]
        public void TwoVariableNodesWithDifferentNameAndSameTypeAreNotEqual()
        {
            var node1 = new VariableDeclarationNode(1, 2, "test", VariableType.BOOLEAN);
            var node2 = new VariableDeclarationNode(71, 122, "dfasdasd", VariableType.BOOLEAN);
            Assert.AreNotEqual(node1, node2);
        }

        [TestMethod()]
        public void TwoVariableNodesWithSameNameAndTypeHaveSameHashCode()
        {
            var node1 = new VariableDeclarationNode(1, 2, "test", VariableType.STRING);
            var node2 = new VariableDeclarationNode(71, 122, "test", VariableType.STRING);


            Assert.AreEqual(node1.GetHashCode(), node2.GetHashCode());
        }
    }
}