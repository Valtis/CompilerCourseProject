using Microsoft.VisualStudio.TestTools.UnitTesting;
using CompilersCourseWork.ErrorHandling;
using CompilersCourseWork.Lexing;
using CompilersCourseWork.AST;
using System.Collections.Generic;

namespace CompilersCourseWork.Parsing.Tests
{
    [TestClass()]
    public class ParserTests
    {
        [TestMethod()]
        public void ParserParserVariableDeclarationWithoutAssignment()
        {
            var reporter = new ErrorReporter();
            var parser = new Parser(
                new Lexer("../../Parsing/variable_declaration_no_assignment.txt", reporter), 
                reporter);

            var node = parser.Parse();

            ASTPreOrderMatches(
                node, 
                new List<Node>{
                    new StatementsNode(0, 0),
                    new VariableNode(0, 0, "a", VariableType.STRING),
                    new VariableNode(0, 0, "b", VariableType.INTEGER),
                    new VariableNode(0, 0, "c", VariableType.BOOLEAN),
                });

            Assert.AreEqual(0, reporter.Errors.Count);
        }

        private void ASTPreOrderMatches(Node node, IList<Node> nodes_preorder)
        {
            CheckPreorder(node, nodes_preorder);
            Assert.AreEqual(0, nodes_preorder.Count);
        }

        private void CheckPreorder(Node node, IList<Node> nodes_preorder)
        {
            Assert.IsTrue(nodes_preorder.Count != 0);
            Assert.AreEqual(node, nodes_preorder[0]);

            nodes_preorder.RemoveAt(0);

            foreach (var child in node.Children)
            {
                CheckPreorder(child, nodes_preorder);
            }
        }
    }
}