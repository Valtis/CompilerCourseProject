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
        public void ParserParsesVariableDeclarationsWithoutAssignment()
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

        [TestMethod()]
        public void ParserParsesInvalidVariableDeclarationsWithoutAssignment()
        {
            var reporter = new ErrorReporter();
            var parser = new Parser(
                new Lexer("../../Parsing/invalid_variable_declaration_no_assignment.txt", reporter),
                reporter);

            var node = parser.Parse();

            ASTPreOrderMatches(
                node,
                new List<Node>{
                    new StatementsNode(0, 0),
                    new ErrorNode(),
                    new ErrorNode(),
                    new VariableNode(0, 0, "valid", VariableType.STRING),
                    new ErrorNode(),
                    new ErrorNode(),
                    new ErrorNode(),
                    new ErrorNode(),
                    new VariableNode(0, 0, "a", VariableType.INTEGER),
                });

            Assert.AreEqual(7, reporter.Errors.Count);

            Assert.AreEqual(Error.SYNTAX_ERROR, reporter.Errors[0].Type);
            Assert.AreEqual(0, reporter.Errors[0].Line);
            Assert.AreEqual(4, reporter.Errors[0].Column);
            Assert.IsTrue(reporter.Errors[0].Message.ToLower().Contains("was <operator - ':'>"));

            Assert.AreEqual(Error.SYNTAX_ERROR, reporter.Errors[1].Type);
            Assert.AreEqual(1, reporter.Errors[1].Line);
            Assert.AreEqual(7, reporter.Errors[1].Column);
            Assert.IsTrue(reporter.Errors[1].Message.ToLower().Contains("was <keyword - 'string'>"));

            Assert.AreEqual(Error.SYNTAX_ERROR, reporter.Errors[2].Type);
            Assert.AreEqual(3, reporter.Errors[2].Line);
            Assert.AreEqual(8, reporter.Errors[2].Column);
            Assert.IsTrue(reporter.Errors[2].Message.ToLower().Contains("was <text - 'hello'>"));

            Assert.AreEqual(Error.SYNTAX_ERROR, reporter.Errors[3].Type);
            Assert.AreEqual(4, reporter.Errors[3].Line);
            Assert.AreEqual(4, reporter.Errors[3].Column);
            Assert.IsTrue(reporter.Errors[3].Message.ToLower().Contains("was <number - '412'>"));

            Assert.AreEqual(Error.SYNTAX_ERROR, reporter.Errors[4].Type);
            Assert.AreEqual(5, reporter.Errors[4].Line);
            Assert.AreEqual(10, reporter.Errors[4].Column);
            Assert.IsTrue(reporter.Errors[4].Message.ToLower().Contains("was <number - '12345'>"));

            Assert.AreEqual(Error.SYNTAX_ERROR, reporter.Errors[5].Type);
            Assert.AreEqual(6, reporter.Errors[5].Line);
            Assert.AreEqual(3, reporter.Errors[5].Column);
            Assert.IsTrue(reporter.Errors[5].Message.ToLower().Contains("was <operator - ';'>"));

            Assert.AreEqual(Error.SYNTAX_ERROR, reporter.Errors[6].Type);
            Assert.AreEqual(7, reporter.Errors[6].Line);
            Assert.AreEqual(11, reporter.Errors[6].Column);
            Assert.IsTrue(reporter.Errors[6].Message.ToLower().Contains("was <eof - 'eof'>"));
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