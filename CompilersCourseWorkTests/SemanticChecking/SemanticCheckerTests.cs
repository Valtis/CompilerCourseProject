using Microsoft.VisualStudio.TestTools.UnitTesting;
using CompilersCourseWork.SemanticChecking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CompilersCourseWork.ErrorHandling;
using CompilersCourseWork.Parsing;
using CompilersCourseWork.Lexing;

namespace CompilersCourseWork.SemanticChecking.Tests
{
    [TestClass()]
    public class SemanticCheckerTests
    {

        [TestMethod()]
        public void SemanticCheckerAcceptsValidVariableDeclaration()
        {
            var reporter = new ErrorReporter();
            var parser = new Parser(
                new Lexer("../../SemanticChecking/valid_variable_declaration.txt", reporter),
                reporter);

            var node = parser.Parse();

            var semanticChecker = new SemanticChecker(reporter);
            node.Accept(semanticChecker);

            Assert.AreEqual(0, reporter.Errors.Count);

        }

        [TestMethod()]
        public void SemanticCheckerRejectsInvalidVariableDeclaration()
        {
            var reporter = new ErrorReporter();
            var parser = new Parser(
                new Lexer("../../SemanticChecking/invalid_variable_declaration.txt", reporter),
                reporter);

            var node = parser.Parse();

            var semanticChecker = new SemanticChecker(reporter);
            node.Accept(semanticChecker);

            Assert.AreEqual(29, reporter.Errors.Count);

            Assert.AreEqual(Error.SEMANTIC_ERROR, reporter.Errors[0].Type);
            Assert.AreEqual(1, reporter.Errors[0].Line);
            Assert.AreEqual(4, reporter.Errors[0].Column);
            Assert.IsTrue(reporter.Errors[0].Message.ToLower().Contains("redeclaration of variable"));

            Assert.AreEqual(Error.SEMANTIC_ERROR, reporter.Errors[1].Type);
            Assert.AreEqual(2, reporter.Errors[1].Line);
            Assert.AreEqual(15, reporter.Errors[1].Column);
            Assert.IsTrue(reporter.Errors[1].Message.ToLower().Contains("expression has invalid type 'string'"));

            Assert.AreEqual(Error.SEMANTIC_ERROR, reporter.Errors[2].Type);
            Assert.AreEqual(3, reporter.Errors[2].Line);
            Assert.AreEqual(15, reporter.Errors[2].Column);
            Assert.IsTrue(reporter.Errors[2].Message.ToLower().Contains("cannot initialize variable with self"));

            Assert.AreEqual(Error.SEMANTIC_ERROR, reporter.Errors[3].Type);
            Assert.AreEqual(4, reporter.Errors[3].Line);
            Assert.AreEqual(17, reporter.Errors[3].Column);
            Assert.IsTrue(reporter.Errors[3].Message.ToLower().Contains("expression has invalid type 'boolean'"));

            Assert.AreEqual(Error.SEMANTIC_ERROR, reporter.Errors[4].Type);
            Assert.AreEqual(5, reporter.Errors[4].Line);
            Assert.AreEqual(23, reporter.Errors[4].Column);
            Assert.IsTrue(reporter.Errors[4].Message.ToLower().Contains("incompatible types for operator '<'"));

            Assert.AreEqual(Error.SEMANTIC_ERROR, reporter.Errors[5].Type);
            Assert.AreEqual(6, reporter.Errors[5].Line);
            Assert.AreEqual(18, reporter.Errors[5].Column);
            Assert.IsTrue(reporter.Errors[5].Message.ToLower().Contains("expression has invalid type 'integer'"));

            Assert.AreEqual(Error.SEMANTIC_ERROR, reporter.Errors[6].Type);
            Assert.AreEqual(7, reporter.Errors[6].Line);
            Assert.AreEqual(26, reporter.Errors[6].Column);
            Assert.IsTrue(reporter.Errors[6].Message.ToLower().Contains("incompatible types for operator '+"));

            Assert.AreEqual(Error.SEMANTIC_ERROR, reporter.Errors[7].Type);
            Assert.AreEqual(8, reporter.Errors[7].Line);
            Assert.AreEqual(20, reporter.Errors[7].Column);
            Assert.IsTrue(reporter.Errors[7].Message.ToLower().Contains("expression has invalid type 'boolean'"));

            Assert.AreEqual(Error.SEMANTIC_ERROR, reporter.Errors[8].Type);
            Assert.AreEqual(9, reporter.Errors[8].Line);
            Assert.AreEqual(18, reporter.Errors[8].Column);
            Assert.IsTrue(reporter.Errors[8].Message.ToLower().Contains("operator '&' expects"));

            Assert.AreEqual(Error.SEMANTIC_ERROR, reporter.Errors[9].Type);
            Assert.AreEqual(10, reporter.Errors[9].Line);
            Assert.AreEqual(16, reporter.Errors[9].Column);
            Assert.IsTrue(reporter.Errors[9].Message.ToLower().Contains("incompatible expression for operator '!'"));

            Assert.AreEqual(Error.SEMANTIC_ERROR, reporter.Errors[10].Type);
            Assert.AreEqual(11, reporter.Errors[10].Line);
            Assert.AreEqual(24, reporter.Errors[10].Column);
            Assert.IsTrue(reporter.Errors[10].Message.ToLower().Contains("incompatible types for operator '*"));

            Assert.AreEqual(Error.SEMANTIC_ERROR, reporter.Errors[11].Type);
            Assert.AreEqual(12, reporter.Errors[11].Line);
            Assert.AreEqual(19, reporter.Errors[11].Column);
            Assert.IsTrue(reporter.Errors[11].Message.ToLower().Contains("incompatible types for operator '/"));

            Assert.AreEqual(Error.SEMANTIC_ERROR, reporter.Errors[12].Type);
            Assert.AreEqual(13, reporter.Errors[12].Line);
            Assert.AreEqual(24, reporter.Errors[12].Column);
            Assert.IsTrue(reporter.Errors[12].Message.ToLower().Contains("operator '-' expects"));

            Assert.AreEqual(Error.SEMANTIC_ERROR, reporter.Errors[13].Type);
            Assert.AreEqual(14, reporter.Errors[13].Line);
            Assert.AreEqual(22, reporter.Errors[13].Column);
            Assert.IsTrue(reporter.Errors[13].Message.ToLower().Contains("incompatible types for operator '="));

            Assert.AreEqual(Error.SEMANTIC_ERROR, reporter.Errors[14].Type);
            Assert.AreEqual(15, reporter.Errors[14].Line);
            Assert.AreEqual(18, reporter.Errors[14].Column);
            Assert.IsTrue(reporter.Errors[14].Message.ToLower().Contains("expression has invalid type 'integer'"));

            Assert.AreEqual(Error.SEMANTIC_ERROR, reporter.Errors[15].Type);
            Assert.AreEqual(16, reporter.Errors[15].Line);
            Assert.AreEqual(23, reporter.Errors[15].Column);
            Assert.IsTrue(reporter.Errors[15].Message.ToLower().Contains("operator '+' expects"));

            Assert.AreEqual(Error.SEMANTIC_ERROR, reporter.Errors[16].Type);
            Assert.AreEqual(17, reporter.Errors[16].Line);
            Assert.AreEqual(23, reporter.Errors[16].Column);
            Assert.IsTrue(reporter.Errors[16].Message.ToLower().Contains("operator '-' expects"));

            Assert.AreEqual(Error.SEMANTIC_ERROR, reporter.Errors[17].Type);
            Assert.AreEqual(18, reporter.Errors[17].Line);
            Assert.AreEqual(23, reporter.Errors[17].Column);
            Assert.IsTrue(reporter.Errors[17].Message.ToLower().Contains("operator '*' expects"));

            Assert.AreEqual(Error.SEMANTIC_ERROR, reporter.Errors[18].Type);
            Assert.AreEqual(19, reporter.Errors[18].Line);
            Assert.AreEqual(23, reporter.Errors[18].Column);
            Assert.IsTrue(reporter.Errors[18].Message.ToLower().Contains("operator '/' expects"));

            Assert.AreEqual(Error.SEMANTIC_ERROR, reporter.Errors[19].Type);
            Assert.AreEqual(20, reporter.Errors[19].Line);
            Assert.AreEqual(24, reporter.Errors[19].Column);
            Assert.IsTrue(reporter.Errors[19].Message.ToLower().Contains("incompatible types for operator '="));

            Assert.AreEqual(Error.SEMANTIC_ERROR, reporter.Errors[20].Type);
            Assert.AreEqual(21, reporter.Errors[20].Line);
            Assert.AreEqual(24, reporter.Errors[20].Column);
            Assert.IsTrue(reporter.Errors[20].Message.ToLower().Contains("incompatible types for operator '="));

            Assert.AreEqual(Error.SEMANTIC_ERROR, reporter.Errors[21].Type);
            Assert.AreEqual(22, reporter.Errors[21].Line);
            Assert.AreEqual(24, reporter.Errors[21].Column);
            Assert.IsTrue(reporter.Errors[21].Message.ToLower().Contains("incompatible types for operator '*"));

            Assert.AreEqual(Error.SEMANTIC_ERROR, reporter.Errors[22].Type);
            Assert.AreEqual(23, reporter.Errors[22].Line);
            Assert.AreEqual(24, reporter.Errors[22].Column);
            Assert.IsTrue(reporter.Errors[22].Message.ToLower().Contains("incompatible types for operator '/"));

            Assert.AreEqual(Error.SEMANTIC_ERROR, reporter.Errors[23].Type);
            Assert.AreEqual(24, reporter.Errors[23].Line);
            Assert.AreEqual(24, reporter.Errors[23].Column);
            Assert.IsTrue(reporter.Errors[23].Message.ToLower().Contains("incompatible types for operator '&"));

            Assert.AreEqual(Error.SEMANTIC_ERROR, reporter.Errors[24].Type);
            Assert.AreEqual(25, reporter.Errors[24].Line);
            Assert.AreEqual(24, reporter.Errors[24].Column);
            Assert.IsTrue(reporter.Errors[24].Message.ToLower().Contains("incompatible types for operator '<"));

            Assert.AreEqual(Error.SEMANTIC_ERROR, reporter.Errors[25].Type);
            Assert.AreEqual(26, reporter.Errors[25].Line);
            Assert.AreEqual(24, reporter.Errors[25].Column);
            Assert.IsTrue(reporter.Errors[25].Message.ToLower().Contains("incompatible types for operator '+"));

            Assert.AreEqual(Error.SEMANTIC_ERROR, reporter.Errors[26].Type);
            Assert.AreEqual(27, reporter.Errors[26].Line);
            Assert.AreEqual(25, reporter.Errors[26].Column);
            Assert.IsTrue(reporter.Errors[26].Message.ToLower().Contains("operator '-' expects"));

            Assert.AreEqual(Error.SEMANTIC_ERROR, reporter.Errors[27].Type);
            Assert.AreEqual(28, reporter.Errors[27].Line);
            Assert.AreEqual(28, reporter.Errors[27].Column);
            Assert.IsTrue(reporter.Errors[27].Message.ToLower().Contains("incompatible expression for operator '!'"));

            Assert.AreEqual(Error.SEMANTIC_ERROR, reporter.Errors[28].Type);
            Assert.AreEqual(29, reporter.Errors[28].Line);
            Assert.AreEqual(26, reporter.Errors[28].Column);
            Assert.IsTrue(reporter.Errors[28].Message.ToLower().Contains("cannot initialize variable with self"));
        }

        [TestMethod()]
        public void SemanticCheckerAcceptsValidVariableAssignment()
        {
            var reporter = new ErrorReporter();
            var parser = new Parser(
                new Lexer("../../SemanticChecking/valid_variable_assignment.txt", reporter),
                reporter);

            var node = parser.Parse();

            var semanticChecker = new SemanticChecker(reporter);
            node.Accept(semanticChecker);

            Assert.AreEqual(0, reporter.Errors.Count);
        }


        [TestMethod()]
        public void SemanticCheckerRejectsInalidValidVariableAssignment()
        {
            var reporter = new ErrorReporter();
            var parser = new Parser(
                new Lexer("../../SemanticChecking/invalid_variable_assignment.txt", reporter),
                reporter);

            var node = parser.Parse();

            var semanticChecker = new SemanticChecker(reporter);
            node.Accept(semanticChecker);

            Assert.AreEqual(11, reporter.Errors.Count);

            Assert.AreEqual(Error.SEMANTIC_ERROR, reporter.Errors[0].Type);
            Assert.AreEqual(0, reporter.Errors[0].Line);
            Assert.AreEqual(0, reporter.Errors[0].Column);
            Assert.IsTrue(reporter.Errors[0].Message.ToLower().Contains("variable 'a' has not been declared at this point"));

            Assert.AreEqual(Error.SEMANTIC_ERROR, reporter.Errors[1].Type);
            Assert.AreEqual(4, reporter.Errors[1].Line);
            Assert.AreEqual(5, reporter.Errors[1].Column);
            Assert.IsTrue(reporter.Errors[1].Message.ToLower().Contains("expression has invalid type 'string'"));

            Assert.AreEqual(Error.SEMANTIC_ERROR, reporter.Errors[2].Type);
            Assert.AreEqual(5, reporter.Errors[2].Line);
            Assert.AreEqual(7, reporter.Errors[2].Column);
            Assert.IsTrue(reporter.Errors[2].Message.ToLower().Contains("expression has invalid type 'boolean'"));

            Assert.AreEqual(Error.SEMANTIC_ERROR, reporter.Errors[3].Type);
            Assert.AreEqual(6, reporter.Errors[3].Line);
            Assert.AreEqual(13, reporter.Errors[3].Column);
            Assert.IsTrue(reporter.Errors[3].Message.ToLower().Contains("incompatible types for operator '+'"));

            Assert.AreEqual(Error.SEMANTIC_ERROR, reporter.Errors[4].Type);
            Assert.AreEqual(7, reporter.Errors[4].Line);
            Assert.AreEqual(15, reporter.Errors[4].Column);
            Assert.IsTrue(reporter.Errors[4].Message.ToLower().Contains("incompatible types for operator '+'"));

            Assert.AreEqual(Error.SEMANTIC_ERROR, reporter.Errors[5].Type);
            Assert.AreEqual(8, reporter.Errors[5].Line);
            Assert.AreEqual(5, reporter.Errors[5].Column);
            Assert.IsTrue(reporter.Errors[5].Message.ToLower().Contains("expression has invalid type 'integer'"));

            Assert.AreEqual(Error.SEMANTIC_ERROR, reporter.Errors[6].Type);
            Assert.AreEqual(9, reporter.Errors[6].Line);
            Assert.AreEqual(5, reporter.Errors[6].Column);
            Assert.IsTrue(reporter.Errors[6].Message.ToLower().Contains("expression has invalid type 'string'"));

            Assert.AreEqual(Error.SEMANTIC_ERROR, reporter.Errors[7].Type);
            Assert.AreEqual(10, reporter.Errors[7].Line);
            Assert.AreEqual(5, reporter.Errors[7].Column);
            Assert.IsTrue(reporter.Errors[7].Message.ToLower().Contains("incompatible expression for operator '!'"));

            Assert.AreEqual(Error.SEMANTIC_ERROR, reporter.Errors[8].Type);
            Assert.AreEqual(11, reporter.Errors[8].Line);
            Assert.AreEqual(5, reporter.Errors[8].Column);
            Assert.IsTrue(reporter.Errors[8].Message.ToLower().Contains("expression has invalid type 'integer'"));

            Assert.AreEqual(Error.SEMANTIC_ERROR, reporter.Errors[9].Type);
            Assert.AreEqual(12, reporter.Errors[9].Line);
            Assert.AreEqual(7, reporter.Errors[9].Column);
            Assert.IsTrue(reporter.Errors[9].Message.ToLower().Contains("expression has invalid type 'boolean'"));

            Assert.AreEqual(Error.SEMANTIC_ERROR, reporter.Errors[10].Type);
            Assert.AreEqual(13, reporter.Errors[10].Line);
            Assert.AreEqual(13, reporter.Errors[10].Column);
            Assert.IsTrue(reporter.Errors[10].Message.ToLower().Contains("operator '-' expects operands to have type 'integer'"));
        }

        [TestMethod()]
        public void SemanticCheckerAcceptsValidForStatement()
        {
            var reporter = new ErrorReporter();
            var parser = new Parser(
                new Lexer("../../SemanticChecking/valid_for_statement.txt", reporter),
                reporter);

            var node = parser.Parse();

            var semanticChecker = new SemanticChecker(reporter);
            node.Accept(semanticChecker);

            Assert.AreEqual(0, reporter.Errors.Count);
        }

        [TestMethod()]
        public void SemanticCheckerRejectsInvalidForStatement()
        {
            var reporter = new ErrorReporter();
            var parser = new Parser(
                new Lexer("../../SemanticChecking/invalid_for_statement.txt", reporter),
                reporter);

            var node = parser.Parse();

            var semanticChecker = new SemanticChecker(reporter);
            node.Accept(semanticChecker);

            Assert.AreEqual(11, reporter.Errors.Count);

            Assert.AreEqual(Error.SEMANTIC_ERROR, reporter.Errors[0].Type);
            Assert.AreEqual(0, reporter.Errors[0].Line);
            Assert.AreEqual(4, reporter.Errors[0].Column);
            Assert.IsTrue(reporter.Errors[0].Message.ToLower().Contains("variable 'undeclared' has not been"));

            Assert.AreEqual(Error.SEMANTIC_ERROR, reporter.Errors[1].Type);
            Assert.AreEqual(6, reporter.Errors[1].Line);
            Assert.AreEqual(9, reporter.Errors[1].Column);
            Assert.IsTrue(reporter.Errors[1].Message.ToLower().Contains("variable 'undeclared' has not been"));

            Assert.AreEqual(Error.SEMANTIC_ERROR, reporter.Errors[2].Type);
            Assert.AreEqual(12, reporter.Errors[2].Line);
            Assert.AreEqual(4, reporter.Errors[2].Column);
            Assert.IsTrue(reporter.Errors[2].Message.ToLower().Contains("loop control variable must have type 'integer'")); Assert.AreEqual(Error.SEMANTIC_ERROR, reporter.Errors[1].Type);
            Assert.IsTrue(reporter.Errors[2].Message.ToLower().Contains("has type 'string'"));

            Assert.AreEqual(Error.SEMANTIC_ERROR, reporter.Errors[3].Type);
            Assert.AreEqual(16, reporter.Errors[3].Line);
            Assert.AreEqual(9, reporter.Errors[3].Column);
            Assert.IsTrue(reporter.Errors[3].Message.ToLower().Contains("expression expects type 'integer'"));
            Assert.IsTrue(reporter.Errors[3].Message.ToLower().Contains("but has type 'string'"));

            Assert.AreEqual(Error.SEMANTIC_ERROR, reporter.Errors[4].Type);
            Assert.AreEqual(21, reporter.Errors[4].Line);
            Assert.AreEqual(9, reporter.Errors[4].Column);
            Assert.IsTrue(reporter.Errors[4].Message.ToLower().Contains("expression expects type 'integer'"));
            Assert.IsTrue(reporter.Errors[4].Message.ToLower().Contains("but has type 'string'"));

            Assert.AreEqual(Error.SEMANTIC_ERROR, reporter.Errors[5].Type);
            Assert.AreEqual(25, reporter.Errors[5].Line);
            Assert.AreEqual(14, reporter.Errors[5].Column);
            Assert.IsTrue(reporter.Errors[5].Message.ToLower().Contains("expression expects type 'integer'"));
            Assert.IsTrue(reporter.Errors[5].Message.ToLower().Contains("but has type 'boolean'"));

            Assert.AreEqual(Error.SEMANTIC_ERROR, reporter.Errors[6].Type);
            Assert.AreEqual(31, reporter.Errors[6].Line);
            Assert.AreEqual(12, reporter.Errors[6].Column);
            Assert.IsTrue(reporter.Errors[6].Message.ToLower().Contains("expression expects type 'integer'"));
            Assert.IsTrue(reporter.Errors[6].Message.ToLower().Contains("but has type 'boolean'"));

            Assert.AreEqual(Error.SEMANTIC_ERROR, reporter.Errors[7].Type);
            Assert.AreEqual(36, reporter.Errors[7].Line);
            Assert.AreEqual(23, reporter.Errors[7].Column);
            Assert.IsTrue(reporter.Errors[7].Message.ToLower().Contains("expression has invalid type 'string'"));
            Assert.IsTrue(reporter.Errors[7].Message.ToLower().Contains("while variable has type 'integer'"));

            Assert.AreEqual(Error.SEMANTIC_ERROR, reporter.Errors[8].Type);
            Assert.AreEqual(40, reporter.Errors[8].Line);
            Assert.AreEqual(12, reporter.Errors[8].Column);
            Assert.IsTrue(reporter.Errors[8].Message.ToLower().Contains("redeclaration of variable 'a'"));

            Assert.AreEqual(Error.SEMANTIC_ERROR, reporter.Errors[9].Type);
            Assert.AreEqual(45, reporter.Errors[9].Line);
            Assert.AreEqual(8, reporter.Errors[9].Column);
            Assert.IsTrue(reporter.Errors[9].Message.ToLower().Contains("cannot reassign control variable 'i'"));

            Assert.AreEqual(Error.SEMANTIC_ERROR, reporter.Errors[10].Type);
            Assert.AreEqual(53, reporter.Errors[10].Line);
            Assert.AreEqual(16, reporter.Errors[10].Column);
            Assert.IsTrue(reporter.Errors[10].Message.ToLower().Contains("cannot reassign control variable 'i'"));
        }

        [TestMethod()]
        public void SemanticCheckerAcceptsExampleProgram()
        {
            var reporter = new ErrorReporter();
            var parser = new Parser(
                new Lexer("../../example_program.txt", reporter),
                reporter);

            var node = parser.Parse();

            var semanticChecker = new SemanticChecker(reporter);
            node.Accept(semanticChecker);

            Assert.AreEqual(0, reporter.Errors.Count);
        }

        [TestMethod()]
        public void SemanticCheckerAcceptsExampleProgram2()
        {
            var reporter = new ErrorReporter();
            var parser = new Parser(
                new Lexer("../../example_program2.txt", reporter),
                reporter);

            var node = parser.Parse();

            var semanticChecker = new SemanticChecker(reporter);
            node.Accept(semanticChecker);

            Assert.AreEqual(0, reporter.Errors.Count);

        }
    }
}