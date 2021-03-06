﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
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

            Assert.AreEqual(0, reporter.Errors.Count);

            ASTPreOrderMatches(
                node,
                new List<Node>{
                    new StatementsNode(0, 0),
                    new VariableDeclarationNode(0, 0, "a", VariableType.STRING),
                    new VariableDeclarationNode(0, 0, "b", VariableType.INTEGER),
                    new VariableDeclarationNode(0, 0, "c", VariableType.BOOLEAN),
                });


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
                    new VariableDeclarationNode(0, 0, "valid", VariableType.STRING),
                    new ErrorNode(),
                    new ErrorNode(),
                    new ErrorNode(),
                    new ErrorNode(),
                    new ErrorNode(),
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
            Assert.AreEqual(6, reporter.Errors[4].Column);
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


        [TestMethod()]
        public void ParserParsesValidVariableDeclarationWithAssignment()
        {
            var reporter = new ErrorReporter();
            var parser = new Parser(
                new Lexer("../../Parsing/variable_declaration_with_assignment.txt", reporter),
                reporter);

            var node = parser.Parse();

            Assert.AreEqual(0, reporter.Errors.Count);

            ASTPreOrderMatches(
               node,
               new List<Node>{
                    new StatementsNode(0, 0),

                    new VariableDeclarationNode(0, 0, "a", VariableType.STRING),
                    new IntegerNode(0, 0, 4),

                    new VariableDeclarationNode(0, 0, "b", VariableType.BOOLEAN),
                    new IdentifierNode(0, 0, "abc"),

                    new VariableDeclarationNode(0, 0, "c", VariableType.INTEGER),
                    new AddNode(0, 0, null, null),
                    new IntegerNode(0, 0, 2),
                    new IntegerNode(0, 0, 4),


                    new VariableDeclarationNode(0, 0, "d", VariableType.INTEGER),
                    new AddNode(0, 0, null, null),
                    new StringNode(0, 0, "hello "),
                    new StringNode(0, 0, " world"),

                    new VariableDeclarationNode(0, 0, "e", VariableType.INTEGER),
                    new SubtractNode(0, 0, null, null),
                    new IntegerNode(0, 0, 2),
                    new IntegerNode(0, 0, 4),

                    new VariableDeclarationNode(0, 0, "f", VariableType.INTEGER),
                    new MultiplyNode(0, 0, null, null),
                    new IntegerNode(0, 0, 2),
                    new IntegerNode(0, 0, 4),

                    new VariableDeclarationNode(0, 0, "g", VariableType.INTEGER),
                    new DivideNode(0, 0, null, null),
                    new IntegerNode(0, 0, 20),
                    new IntegerNode(0, 0, 42),

                    new VariableDeclarationNode(0, 0, "h", VariableType.INTEGER),
                    new AndNode(0, 0, null, null),
                    new IntegerNode(0, 0, 20),
                    new IntegerNode(0, 0, 42),

                    new VariableDeclarationNode(0, 0, "i", VariableType.INTEGER),
                    new ComparisonNode(0, 0, null, null),
                    new IntegerNode(0, 0, 20),
                    new IntegerNode(0, 0, 4),

                    new VariableDeclarationNode(0, 0, "j", VariableType.INTEGER),
                    new LessThanNode(0, 0, null, null),
                    new IntegerNode(0, 0, 402),
                    new IntegerNode(0, 0, 32),

                    new VariableDeclarationNode(0, 0, "k", VariableType.INTEGER),
                    new NotNode(0, 0, null),
                    new IntegerNode(0, 0, 20),

                    new VariableDeclarationNode(0, 0, "l", VariableType.STRING),
                    new LessThanNode(0, 0, null, null),
                    new StringNode(0, 0, "hello"),
                    new StringNode(0, 0, "world"),

                    new VariableDeclarationNode(0, 0, "m", VariableType.STRING),
                    new MultiplyNode(0, 0, null, null),
                    new AddNode(0, 0, null, null),
                    new IntegerNode(0, 0, 2),
                    new IntegerNode(0, 0, 4),
                    new SubtractNode(0, 0, null, null),
                    new IntegerNode(0, 0, 5),
                    new IntegerNode(0, 0, 6),

                    new VariableDeclarationNode(0, 0, "n", VariableType.STRING),
                    new MultiplyNode(0, 0, null, null),
                    new AddNode(0, 0, null, null),
                    new IdentifierNode(0, 0, "abc"),
                    new IntegerNode(0, 0, 4),
                    new IdentifierNode(0, 0, "def"),

                    new VariableDeclarationNode(0, 0, "o", VariableType.BOOLEAN),
                    new NotNode(0, 0, null),
                    new AndNode(0, 0, null, null),
                    new IdentifierNode(0, 0, "a"),
                    new IdentifierNode(0, 0, "b"),
               });

        }

        [TestMethod()]
        public void ParserParsesInvalidVariableDeclarationWithAssignment()
        {
            var reporter = new ErrorReporter();
            var parser = new Parser(
                new Lexer("../../Parsing/invalid_variable_declaration_with_assignment.txt", reporter),
                reporter);

            var node = parser.Parse();

            ASTPreOrderMatches(
                node,
                new List<Node>{
                    new StatementsNode(0, 0),
                    new ErrorNode(),
                    new ErrorNode(),
                    new ErrorNode(),
                    new ErrorNode(),
                    new ErrorNode(),
                    new ErrorNode(),
                    new ErrorNode(),
                    new ErrorNode(),
                });

            Assert.AreEqual(8, reporter.Errors.Count);

            Assert.AreEqual(Error.SYNTAX_ERROR, reporter.Errors[0].Type);
            Assert.AreEqual(0, reporter.Errors[0].Line);
            Assert.AreEqual(12, reporter.Errors[0].Column);
            Assert.IsTrue(reporter.Errors[0].Message.ToLower().Contains("was <number - '2'>"));
            Assert.IsTrue(reporter.Errors[0].Message.ToLower().Contains("one of"));
            Assert.IsTrue(reporter.Errors[0].Message.ToLower().Contains("<operator - ';'>"));
            Assert.IsTrue(reporter.Errors[0].Message.ToLower().Contains("<operator - ':='>"));



            Assert.AreEqual(Error.SYNTAX_ERROR, reporter.Errors[1].Type);
            Assert.AreEqual(2, reporter.Errors[1].Line);
            Assert.AreEqual(0, reporter.Errors[1].Column);
            Assert.IsTrue(reporter.Errors[1].Message.ToLower().Contains("was <keyword - 'var'>"));
            Assert.IsTrue(reporter.Errors[1].Message.ToLower().Contains("one of"));
            Assert.IsTrue(reporter.Errors[1].Message.ToLower().Contains("<operator - ';'>"));
            Assert.IsTrue(reporter.Errors[1].Message.ToLower().Contains("<operator - ':='>"));


            Assert.AreEqual(Error.SYNTAX_ERROR, reporter.Errors[2].Type);
            Assert.AreEqual(3, reporter.Errors[2].Line);
            Assert.AreEqual(12, reporter.Errors[2].Column);
            Assert.IsTrue(reporter.Errors[2].Message.ToLower().Contains("was <operator - '='>"));
            Assert.IsTrue(reporter.Errors[2].Message.ToLower().Contains("one of"));
            Assert.IsTrue(reporter.Errors[2].Message.ToLower().Contains("<operator - ';'>"));
            Assert.IsTrue(reporter.Errors[2].Message.ToLower().Contains("<operator - ':='>"));


            Assert.AreEqual(Error.SYNTAX_ERROR, reporter.Errors[3].Type);
            Assert.AreEqual(4, reporter.Errors[3].Line);
            Assert.AreEqual(14, reporter.Errors[3].Column);
            Assert.IsTrue(reporter.Errors[3].Message.ToLower().Contains("<operator - ';'>"));
            Assert.IsTrue(reporter.Errors[3].Message.ToLower().Contains("operand was expected"));


            Assert.AreEqual(Error.SYNTAX_ERROR, reporter.Errors[4].Type);
            Assert.AreEqual(5, reporter.Errors[4].Line);
            Assert.AreEqual(17, reporter.Errors[4].Column);
            Assert.IsTrue(reporter.Errors[4].Message.ToLower().Contains("<operator - ';'>"));
            Assert.IsTrue(reporter.Errors[4].Message.ToLower().Contains("operand was expected"));


            Assert.AreEqual(Error.SYNTAX_ERROR, reporter.Errors[5].Type);
            Assert.AreEqual(6, reporter.Errors[5].Line);
            Assert.AreEqual(17, reporter.Errors[5].Column);
            Assert.IsTrue(reporter.Errors[5].Message.ToLower().Contains("unexpected token <number - '4'>"));
            Assert.IsTrue(reporter.Errors[5].Message.ToLower().Contains(""));
            Assert.IsTrue(reporter.Errors[5].Message.ToLower().Contains("binary operator"));
            Assert.IsTrue(reporter.Errors[5].Message.ToLower().Contains("<operator - ';'>"));
            Assert.IsTrue(reporter.Errors[5].Message.ToLower().Contains("was expected"));


            Assert.AreEqual(Error.SYNTAX_ERROR, reporter.Errors[6].Type);
            Assert.AreEqual(7, reporter.Errors[6].Line);
            Assert.AreEqual(31, reporter.Errors[6].Column);
            Assert.IsTrue(reporter.Errors[6].Message.ToLower().Contains("expected token <operator - ')'>"));
            Assert.IsTrue(reporter.Errors[6].Message.ToLower().Contains("was <operator - ';'>"));


            Assert.AreEqual(Error.SYNTAX_ERROR, reporter.Errors[7].Type);
            Assert.AreEqual(8, reporter.Errors[7].Line);
            Assert.AreEqual(20, reporter.Errors[7].Column);
            Assert.IsTrue(reporter.Errors[7].Message.ToLower().Contains("expected token <operator - ';'>"));
            Assert.IsTrue(reporter.Errors[7].Message.ToLower().Contains("was <eof - 'eof'>"));
        }

        [TestMethod()]
        public void ParserParsesValidVariableAssignment()
        {
            var reporter = new ErrorReporter();
            var parser = new Parser(
                new Lexer("../../Parsing/valid_variable_assignment.txt", reporter),
                reporter);

            var node = parser.Parse();

            ASTPreOrderMatches(
                node,
                new List<Node>{
                    new StatementsNode(0, 0),

                    new VariableAssignmentNode(0, 0, "a"),
                    new IntegerNode(0, 0, 5),

                    new VariableAssignmentNode(0, 0, "b"),
                    new MultiplyNode(0, 0, null, null),
                    new AddNode(0, 0, null, null),
                    new IntegerNode(0, 0, 6),
                    new IntegerNode(0, 0, 4),
                    new IntegerNode(0, 0, 2),

                    new VariableAssignmentNode(0, 0, "c"),
                    new NotNode(0, 0, null),
                    new ComparisonNode(0, 0, null, null),
                    new IdentifierNode(0, 0, "a"),
                    new IdentifierNode(0, 0, "b"),

                    new VariableAssignmentNode(0, 0, "d"),
                    new AndNode(0, 0, null, null),
                    new IdentifierNode(0, 0, "a"),
                    new IdentifierNode(0, 0, "b"),

                });

            Assert.AreEqual(0, reporter.Errors.Count);
        }

        [TestMethod()]
        public void ParserParsesInvalidVariableAssignment()
        {
            var reporter = new ErrorReporter();
            var parser = new Parser(
                new Lexer("../../Parsing/invalid_variable_assignment.txt", reporter),
                reporter);

            var node = parser.Parse();

            ASTPreOrderMatches(
                node,
                new List<Node>{
                    new StatementsNode(0, 0),
                    new ErrorNode(),
                    new ErrorNode(),
                    new ErrorNode(),
                    new ErrorNode(),
                    new ErrorNode(),
                    new ErrorNode(),
                    new ErrorNode(),
                });

            Assert.AreEqual(7, reporter.Errors.Count);

            Assert.AreEqual(Error.SYNTAX_ERROR, reporter.Errors[0].Type);
            Assert.AreEqual(0, reporter.Errors[0].Line);
            Assert.AreEqual(4, reporter.Errors[0].Column);
            Assert.IsTrue(reporter.Errors[0].Message.ToLower().Contains("unexpected token <operator - ';'>"));
            Assert.IsTrue(reporter.Errors[0].Message.ToLower().Contains("operand was expected"));

            Assert.AreEqual(Error.SYNTAX_ERROR, reporter.Errors[1].Type);
            Assert.AreEqual(1, reporter.Errors[1].Line);
            Assert.AreEqual(2, reporter.Errors[1].Column);
            Assert.IsTrue(reporter.Errors[1].Message.ToLower().Contains("expected token <operator - ':='>"));
            Assert.IsTrue(reporter.Errors[1].Message.ToLower().Contains("was <operator - '='>"));

            Assert.AreEqual(Error.SYNTAX_ERROR, reporter.Errors[2].Type);
            Assert.AreEqual(2, reporter.Errors[2].Line);
            Assert.AreEqual(2, reporter.Errors[2].Column);
            Assert.IsTrue(reporter.Errors[2].Message.ToLower().Contains("expected token <operator - ':='>"));
            Assert.IsTrue(reporter.Errors[2].Message.ToLower().Contains("was <number - '4'>"));

            Assert.AreEqual(Error.SYNTAX_ERROR, reporter.Errors[3].Type);
            Assert.AreEqual(3, reporter.Errors[3].Line);
            Assert.AreEqual(8, reporter.Errors[3].Column);
            Assert.IsTrue(reporter.Errors[3].Message.ToLower().Contains("unexpected token <operator - ';'>"));
            Assert.IsTrue(reporter.Errors[3].Message.ToLower().Contains("operand was expected"));

            Assert.AreEqual(Error.SYNTAX_ERROR, reporter.Errors[4].Type);
            Assert.AreEqual(5, reporter.Errors[4].Line);
            Assert.AreEqual(0, reporter.Errors[4].Column);
            Assert.IsTrue(reporter.Errors[4].Message.ToLower().Contains("expected token <operator - ';'>"));
            Assert.IsTrue(reporter.Errors[4].Message.ToLower().Contains("was <identifier - 'valid'>"));


            Assert.AreEqual(Error.SYNTAX_ERROR, reporter.Errors[5].Type);
            Assert.AreEqual(6, reporter.Errors[5].Line);
            Assert.AreEqual(11, reporter.Errors[5].Column);
            Assert.IsTrue(reporter.Errors[5].Message.ToLower().Contains("unexpected token <operator - ')'>"));
            Assert.IsTrue(reporter.Errors[5].Message.ToLower().Contains("operand was expected"));


            Assert.AreEqual(Error.SYNTAX_ERROR, reporter.Errors[6].Type);
            Assert.AreEqual(7, reporter.Errors[6].Line);
            Assert.AreEqual(1, reporter.Errors[6].Column);
            Assert.IsTrue(reporter.Errors[6].Message.ToLower().Contains("expected token <operator - ':='>"));
            Assert.IsTrue(reporter.Errors[6].Message.ToLower().Contains("was <operator - ';'>"));
        }


        [TestMethod()]
        public void ParserParsesValidForStatement()
        {

            var reporter = new ErrorReporter();
            var parser = new Parser(
                new Lexer("../../Parsing/valid_for_statement.txt", reporter),
                reporter);

            var node = parser.Parse();

            ASTPreOrderMatches(
                node,
                new List<Node>{
                    new StatementsNode(0, 0),

                    new ForNode(0, 0, null, null, null, null),
                    new IdentifierNode(0, 0, "foo"),
                    new IntegerNode(0, 0, 5),
                    new IntegerNode(0, 0, 9),
                    new StatementsNode(0, 0),
                    new VariableAssignmentNode(0, 0, "a"),
                    new IntegerNode(0, 0, 4),

                    new ForNode(0, 0, null, null, null, null),
                    new IdentifierNode(0, 0, "bar"),
                    new AddNode(0, 0, null, null),
                    new IntegerNode(0, 0, 20),
                    new IntegerNode(0, 0, 412),
                    new IdentifierNode(0, 0, "hello"),
                    new StatementsNode(0, 0),
                    new VariableDeclarationNode(0, 0, "foo", VariableType.INTEGER),
                    new IntegerNode(0, 0, 4),

                    new ForNode(0, 0, null, null, null, null),
                    new IdentifierNode(0, 0, "bar"),
                    new NotNode(0, 0, null),
                    new AddNode(0, 0, null, null),
                    new MultiplyNode(0, 0, null, null),
                    new IntegerNode(0, 0, 1),
                    new IntegerNode(0, 0, 4),
                    new IntegerNode(0, 0, 61),
                    new SubtractNode(0, 0, null, null),
                    new IdentifierNode(0, 0, "hello"),
                    new IntegerNode(0, 0, 2),
                    new StatementsNode(0, 0),
                    new VariableDeclarationNode(0, 0, "foo", VariableType.INTEGER),
                    new IntegerNode(0, 0, 4),
                    new VariableAssignmentNode(0, 0, "foo"),
                    new AddNode(0, 0, null, null),
                    new IdentifierNode(0, 0, "foo"),
                    new IntegerNode(0, 0, 1),


                    new ForNode(0, 0, null, null, null, null),
                    new IdentifierNode(0, 0, "f"),
                    new IntegerNode(0, 0, 3),
                    new IntegerNode(0, 0, 6),
                    new StatementsNode(0, 0),
                    new ForNode(0, 0, null, null, null, null),
                    new IdentifierNode(0, 0, "g"),
                    new IntegerNode(0, 0, 7),
                    new IntegerNode(0, 0, 8),
                    new StatementsNode(0, 0),
                    new VariableAssignmentNode(0, 0, "a"),
                    new IntegerNode(0, 0, 5),

                });

            Assert.AreEqual(0, reporter.Errors.Count);
        }

        [TestMethod()]
        public void ParserParsesInvalidForStatements()
        {

            var reporter = new ErrorReporter();
            var parser = new Parser(
                new Lexer("../../Parsing/invalid_for_statement.txt", reporter),
                reporter);

            var node = parser.Parse();

            ASTPreOrderMatches(
                node,
                new List<Node>{
                    new StatementsNode(0, 0),
                    new ErrorNode(),
                    new ErrorNode(),
                    new ErrorNode(),
                    new ErrorNode(),
                    new ErrorNode(),
                    new ErrorNode(),
                    new ErrorNode(),
                    new ErrorNode(),

                    new ForNode(0, 0, null, null, null, null),
                    new IdentifierNode(0, 0, "variable"),
                    new IntegerNode(0, 0, 5),
                    new IntegerNode(0, 0, 20),
                    new StatementsNode(0, 0),
                    new ErrorNode(),
                    new VariableAssignmentNode(0, 0, "a"),
                    new IntegerNode(0, 0, 91),

                    new ForNode(0, 0, null, null, null, null),
                    new IdentifierNode(0, 0, "abcd"),
                    new IntegerNode(0, 0, 1),
                    new IntegerNode(0, 0, 5),
                    new StatementsNode(0, 0),
                    new ErrorNode(),

                    new ErrorNode(),


                    new ForNode(0, 0, null, null, null, null),
                    new IdentifierNode(0, 0, "baz"),
                    new IntegerNode(0, 0, 5),
                    new IntegerNode(0, 0, 9),
                    new StatementsNode(0, 0),
                    new ErrorNode(),


                    new ErrorNode(),

            });

            Assert.AreEqual(13, reporter.Errors.Count);

            Assert.AreEqual(Error.SYNTAX_ERROR, reporter.Errors[0].Type);
            Assert.AreEqual(0, reporter.Errors[0].Line);
            Assert.AreEqual(4, reporter.Errors[0].Column);
            Assert.IsTrue(reporter.Errors[0].Message.ToLower().Contains("expected token <identifier>"));
            Assert.IsTrue(reporter.Errors[0].Message.ToLower().Contains("but was <number - '1243'>"));


            Assert.AreEqual(Error.SYNTAX_ERROR, reporter.Errors[1].Type);
            Assert.AreEqual(4, reporter.Errors[1].Line);
            Assert.AreEqual(4, reporter.Errors[1].Column);
            Assert.IsTrue(reporter.Errors[1].Message.ToLower().Contains("expected token <identifier>"));
            Assert.IsTrue(reporter.Errors[1].Message.ToLower().Contains("but was <keyword - 'in'>"));


            Assert.AreEqual(Error.SYNTAX_ERROR, reporter.Errors[2].Type);
            Assert.AreEqual(8, reporter.Errors[2].Line);
            Assert.AreEqual(14, reporter.Errors[2].Column);
            Assert.IsTrue(reporter.Errors[2].Message.ToLower().Contains("expected token <keyword - 'in'>"));
            Assert.IsTrue(reporter.Errors[2].Message.ToLower().Contains("but was <number - '5'>"));


            Assert.AreEqual(Error.SYNTAX_ERROR, reporter.Errors[3].Type);
            Assert.AreEqual(12, reporter.Errors[3].Line);
            Assert.AreEqual(16, reporter.Errors[3].Column);
            Assert.IsTrue(reporter.Errors[3].Message.ToLower().Contains("unexpected token <operator - '..'>"));
            Assert.IsTrue(reporter.Errors[3].Message.ToLower().Contains("when operand was expected"));


            Assert.AreEqual(Error.SYNTAX_ERROR, reporter.Errors[4].Type);
            Assert.AreEqual(16, reporter.Errors[4].Line);
            Assert.AreEqual(18, reporter.Errors[4].Column);
            Assert.IsTrue(reporter.Errors[4].Message.ToLower().Contains("unexpected token <number - '9'>"));
            Assert.IsTrue(reporter.Errors[4].Message.ToLower().Contains("when binary operator"));
            Assert.IsTrue(reporter.Errors[4].Message.ToLower().Contains("<keyword - 'do'>"));

            Assert.AreEqual(Error.SYNTAX_ERROR, reporter.Errors[5].Type);
            Assert.AreEqual(20, reporter.Errors[5].Line);
            Assert.AreEqual(20, reporter.Errors[5].Column);
            Assert.IsTrue(reporter.Errors[5].Message.ToLower().Contains("unexpected token <keyword - 'do'>"));
            Assert.IsTrue(reporter.Errors[5].Message.ToLower().Contains("when operand was expected"));


            Assert.AreEqual(Error.SYNTAX_ERROR, reporter.Errors[6].Type);
            Assert.AreEqual(24, reporter.Errors[6].Line);
            Assert.AreEqual(18, reporter.Errors[6].Column);
            Assert.IsTrue(reporter.Errors[6].Message.ToLower().Contains("expected token <operator - ')'>"));
            Assert.IsTrue(reporter.Errors[6].Message.ToLower().Contains("but was <operator - '..'>"));


            Assert.AreEqual(Error.SYNTAX_ERROR, reporter.Errors[7].Type);
            Assert.AreEqual(29, reporter.Errors[7].Line);
            Assert.AreEqual(8, reporter.Errors[7].Column);
            Assert.IsTrue(reporter.Errors[7].Message.ToLower().Contains("unexpected token <identifier - 'a'>"));
            Assert.IsTrue(reporter.Errors[7].Message.ToLower().Contains("when binary operator"));
            Assert.IsTrue(reporter.Errors[7].Message.ToLower().Contains("<keyword - 'do'>"));

            Assert.AreEqual(Error.SYNTAX_ERROR, reporter.Errors[8].Type);
            Assert.AreEqual(34, reporter.Errors[8].Line);
            Assert.AreEqual(13, reporter.Errors[8].Column);
            Assert.IsTrue(reporter.Errors[8].Message.ToLower().Contains("unexpected token <operator - ';'>"));
            Assert.IsTrue(reporter.Errors[8].Message.ToLower().Contains("when operand was expected"));

            Assert.AreEqual(Error.SYNTAX_ERROR, reporter.Errors[9].Type);
            Assert.AreEqual(40, reporter.Errors[9].Line);
            Assert.AreEqual(0, reporter.Errors[9].Column);
            Assert.IsTrue(reporter.Errors[9].Message.ToLower().Contains("unexpected token <keyword - 'end'>"));
            Assert.IsTrue(reporter.Errors[9].Message.ToLower().Contains("when binary operator"));
            Assert.IsTrue(reporter.Errors[9].Message.ToLower().Contains("<keyword - 'do'>"));

            Assert.AreEqual(Error.SYNTAX_ERROR, reporter.Errors[10].Type);
            Assert.AreEqual(44, reporter.Errors[10].Line);
            Assert.AreEqual(0, reporter.Errors[10].Column);
            Assert.IsTrue(reporter.Errors[10].Message.ToLower().Contains("expected token <keyword - 'end'> but was <keyword - 'for'>"));

            Assert.AreEqual(Error.SYNTAX_ERROR, reporter.Errors[11].Type);
            Assert.AreEqual(47, reporter.Errors[11].Line);
            Assert.AreEqual(0, reporter.Errors[11].Column);
            Assert.IsTrue(reporter.Errors[11].Message.ToLower().Contains("for loop must contain at least a single statement"));

            Assert.AreEqual(Error.SYNTAX_ERROR, reporter.Errors[12].Type);
            Assert.AreEqual(55, reporter.Errors[12].Line);
            Assert.AreEqual(4, reporter.Errors[12].Column);
            Assert.IsTrue(reporter.Errors[12].Message.ToLower().Contains("expected token <keyword - 'for'> but was <operator - ';'>"));
        }

        [TestMethod()]
        public void ParserParsesValidReadStatement()
        {
            var reporter = new ErrorReporter();
            var parser = new Parser(
                new Lexer("../../Parsing/valid_read_statement.txt", reporter),
                reporter);

            var node = parser.Parse();

            ASTPreOrderMatches(
                node,
                new List<Node>{
                    new StatementsNode(0, 0),
                    new ReadNode(0, 0, null),
                    new IdentifierNode(0, 0, "hello"),

                    new ReadNode(0, 0, null),
                    new IdentifierNode(0, 0, "ab12_32")

                });

            Assert.AreEqual(0, reporter.Errors.Count);
        }


        [TestMethod()]
        public void ParserParsesInvalidReadStatement()
        {
            var reporter = new ErrorReporter();
            var parser = new Parser(
                new Lexer("../../Parsing/invalid_read_statement.txt", reporter),
                reporter);

            var node = parser.Parse();

            ASTPreOrderMatches(
                node,
                new List<Node>{
                    new StatementsNode(0, 0),
                    new ErrorNode(),
                    new ErrorNode(),

                    new ReadNode(0, 0, null),
                    new IdentifierNode(0, 0, "valid"),

                    new ErrorNode(),
                    new ErrorNode(),
                });


            Assert.AreEqual(4, reporter.Errors.Count);

            Assert.AreEqual(Error.SYNTAX_ERROR, reporter.Errors[0].Type);
            Assert.AreEqual(0, reporter.Errors[0].Line);
            Assert.AreEqual(5, reporter.Errors[0].Column);
            Assert.IsTrue(reporter.Errors[0].Message.ToLower().Contains("expected token <identifier> but was <number - '13'>"));


            Assert.AreEqual(Error.SYNTAX_ERROR, reporter.Errors[1].Type);
            Assert.AreEqual(1, reporter.Errors[1].Line);
            Assert.AreEqual(4, reporter.Errors[1].Column);
            Assert.IsTrue(reporter.Errors[1].Message.ToLower().Contains("expected token <identifier> but was <operator - ';'>"));


            Assert.AreEqual(Error.SYNTAX_ERROR, reporter.Errors[2].Type);
            Assert.AreEqual(3, reporter.Errors[2].Line);
            Assert.AreEqual(5, reporter.Errors[2].Column);
            Assert.IsTrue(reporter.Errors[2].Message.ToLower().Contains("expected token <identifier> but was <keyword - 'var'>"));


            Assert.AreEqual(Error.SYNTAX_ERROR, reporter.Errors[3].Type);
            Assert.AreEqual(5, reporter.Errors[3].Line);
            Assert.AreEqual(0, reporter.Errors[3].Column);
            Assert.IsTrue(reporter.Errors[3].Message.ToLower().Contains("expected token <operator - ';'> but was <keyword - 'read'>"));
        }

        [TestMethod()]
        public void ParserParsesValidPrintStatement()
        {

            var reporter = new ErrorReporter();
            var parser = new Parser(
                new Lexer("../../Parsing/valid_print_statement.txt", reporter),
                reporter);

            var node = parser.Parse();

            ASTPreOrderMatches(
                node,
                new List<Node>{
                    new StatementsNode(0, 0),

                    new PrintNode(0, 0, null),
                    new StringNode(0, 0, "hello"),

                    new PrintNode(0, 0, null),
                    new IntegerNode(0, 0, 1),

                    new PrintNode(0, 0, null),
                    new IdentifierNode(0, 0, "abc"),

                    new PrintNode(0, 0, null),
                    new AddNode(0, 0, null, null),
                    new IdentifierNode(0, 0, "abc"),
                    new IntegerNode(0, 0, 4),

                    new PrintNode(0, 0, null),
                    new NotNode(0, 0, null),
                    new IdentifierNode(0, 0, "foo"),

                    new PrintNode(0, 0, null),
                    new IdentifierNode(0, 0, "y"),

                });

            Assert.AreEqual(0, reporter.Errors.Count);
        }

        [TestMethod()]
        public void ParserParsesInvalidPrintStatement()
        {

            var reporter = new ErrorReporter();
            var parser = new Parser(
                new Lexer("../../Parsing/invalid_print_statement.txt", reporter),
                reporter);

            var node = parser.Parse();

            ASTPreOrderMatches(
                node,
                new List<Node>{
                    new StatementsNode(0, 0),

                    new ErrorNode(),
                    new ErrorNode(),
                    new ErrorNode(),
                    new ErrorNode(),
                });

            Assert.AreEqual(4, reporter.Errors.Count);

            Assert.AreEqual(Error.SYNTAX_ERROR, reporter.Errors[0].Type);
            Assert.AreEqual(0, reporter.Errors[0].Line);
            Assert.AreEqual(6, reporter.Errors[0].Column);
            Assert.IsTrue(reporter.Errors[0].Message.ToLower().Contains("unexpected token <operator - '+'>"));

            Assert.AreEqual(Error.SYNTAX_ERROR, reporter.Errors[1].Type);
            Assert.AreEqual(1, reporter.Errors[1].Line);
            Assert.AreEqual(5, reporter.Errors[1].Column);
            Assert.IsTrue(reporter.Errors[1].Message.ToLower().Contains("unexpected token <operator - ';'>"));

            Assert.AreEqual(Error.SYNTAX_ERROR, reporter.Errors[2].Type);
            Assert.AreEqual(3, reporter.Errors[2].Line);
            Assert.AreEqual(0, reporter.Errors[2].Column);
            Assert.IsTrue(reporter.Errors[2].Message.ToLower().Contains("unexpected token <identifier - 'valid_assignment'>"));

            Assert.AreEqual(Error.SYNTAX_ERROR, reporter.Errors[3].Type);
            Assert.AreEqual(4, reporter.Errors[3].Line);
            Assert.AreEqual(6, reporter.Errors[3].Column);
            Assert.IsTrue(reporter.Errors[3].Message.ToLower().Contains("unexpected token <keyword - 'read'>"));
        }

        [TestMethod()]
        public void ParserParsesValidAssertStatement()
        {
            var reporter = new ErrorReporter();
            var parser = new Parser(
                new Lexer("../../Parsing/valid_assert_statement.txt", reporter),
                reporter);

            var node = parser.Parse();

            ASTPreOrderMatches(
                node,
                new List<Node>{
                    new StatementsNode(0, 0),

                    new AssertNode(0, 0, null),
                    new IdentifierNode(0, 0, "abcd"),

                    new AssertNode(0, 0, null),
                    new StringNode(0, 0, "hello"),

                    new AssertNode(0, 0, null),
                    new IntegerNode(0, 0, 4),

                    new AssertNode(0, 0, null),
                    new ComparisonNode(0, 0, null, null),
                    new IntegerNode(0, 0, 5),
                    new IdentifierNode(0, 0, "foo"),


                });

            Assert.AreEqual(0, reporter.Errors.Count);
        }

        [TestMethod()]
        public void ParserParsesInvalidAssertStatement()
        {
            var reporter = new ErrorReporter();
            var parser = new Parser(
                new Lexer("../../Parsing/invalid_assert_statement.txt", reporter),
                reporter);

            var node = parser.Parse();

            ASTPreOrderMatches(
                node,
                new List<Node>{
                    new StatementsNode(0, 0),

                    new ErrorNode(),
                    new ErrorNode(),
                    new ErrorNode(),
                    new ErrorNode(),
                    new ErrorNode(),
                });

            Assert.AreEqual(5, reporter.Errors.Count);

            Assert.AreEqual(Error.SYNTAX_ERROR, reporter.Errors[0].Type);
            Assert.AreEqual(0, reporter.Errors[0].Line);
            Assert.AreEqual(7, reporter.Errors[0].Column);
            Assert.IsTrue(reporter.Errors[0].Message.ToLower().Contains("unexpected token <keyword - 'read'>"));

            Assert.AreEqual(Error.SYNTAX_ERROR, reporter.Errors[1].Type);
            Assert.AreEqual(1, reporter.Errors[1].Line);
            Assert.AreEqual(7, reporter.Errors[1].Column);
            Assert.IsTrue(reporter.Errors[1].Message.ToLower().Contains("expected token <operator - '('> but was <number - '4'>"));


            Assert.AreEqual(Error.SYNTAX_ERROR, reporter.Errors[2].Type);
            Assert.AreEqual(2, reporter.Errors[2].Line);
            Assert.AreEqual(9, reporter.Errors[2].Column);
            Assert.IsTrue(reporter.Errors[2].Message.ToLower().Contains("unexpected token <operator - ':='>"));

            Assert.AreEqual(Error.SYNTAX_ERROR, reporter.Errors[3].Type);
            Assert.AreEqual(3, reporter.Errors[3].Line);
            Assert.AreEqual(6, reporter.Errors[3].Column);
            Assert.IsTrue(reporter.Errors[3].Message.ToLower().Contains("expected token <operator - '('>"));

            Assert.AreEqual(Error.SYNTAX_ERROR, reporter.Errors[4].Type);
            Assert.AreEqual(4, reporter.Errors[4].Line);
            Assert.AreEqual(7, reporter.Errors[4].Column);
            Assert.IsTrue(reporter.Errors[4].Message.ToLower().Contains("unexpected token <keyword - 'assert'>"));
        }

        [TestMethod()]
        public void EmptyProgramIsError()
        {
            var reporter = new ErrorReporter();
            var parser = new Parser(
                new Lexer("../../empty.txt", reporter),
                reporter);

            var node = parser.Parse();

            ASTPreOrderMatches(
               node,
               new List<Node>{
                    new StatementsNode(0, 0),
                    new ErrorNode(),
               });


            Assert.AreEqual(1, reporter.Errors.Count);

            Assert.AreEqual(Error.SYNTAX_ERROR, reporter.Errors[0].Type);
            Assert.AreEqual(0, reporter.Errors[0].Line);
            Assert.AreEqual(0, reporter.Errors[0].Column);
            Assert.IsTrue(reporter.Errors[0].Message.ToLower().Contains("program must contain at least a single statement"));
        }

        [TestMethod()]
        public void ParserParsesValidProgram()
        {
            var reporter = new ErrorReporter();
            var parser = new Parser(
                new Lexer("../../example_program.txt", reporter),
                reporter);

            var node = parser.Parse();

            ASTPreOrderMatches(
               node,
               new List<Node>{
                    new StatementsNode(0, 0),
                    new PrintNode(0, 0, null),
                    new StringNode(0, 0, "Give a number"),

                    new VariableDeclarationNode(0, 0, "n", VariableType.INTEGER),

                    new ReadNode(0, 0, null),
                    new IdentifierNode(0, 0, "n"),

                    new VariableDeclarationNode(0, 0, "v", VariableType.INTEGER),
                    new IntegerNode(0, 0, 1),

                    new VariableDeclarationNode(0, 0, "i", VariableType.INTEGER),

                    new ForNode(0, 0, null, null, null, null),
                    new IdentifierNode(0, 0, "i"),
                    new IntegerNode(0, 0, 1),
                    new IdentifierNode(0, 0, "n"),
                    new StatementsNode(0, 0),
                    new VariableAssignmentNode(0, 0, "v"),
                    new MultiplyNode(0, 0, null, null),
                    new IdentifierNode(0, 0, "v"),
                    new IdentifierNode(0, 0, "i"),

                    new PrintNode(0, 0, null),
                    new StringNode(0, 0, "The result is: "),

                    new PrintNode(0, 0, null),
                    new IdentifierNode(0, 0, "v"),
               });

            Assert.AreEqual(0, reporter.Errors.Count);
        }

        [TestMethod()]
        public void ParserParsesValidProgram2()
        {
            var reporter = new ErrorReporter();
            var parser = new Parser(
                new Lexer("../../example_program2.txt", reporter),
                reporter);

            var node = parser.Parse();

            ASTPreOrderMatches(
               node,
               new List<Node>{
                    new StatementsNode(0, 0),
                    
                    new VariableDeclarationNode(0, 0, "X", VariableType.INTEGER),
                    new AddNode(0, 0, null, null),
                    new IntegerNode(0, 0, 4),
                    new MultiplyNode(0, 0, null, null),
                    new IntegerNode(0, 0, 6),
                    new IntegerNode(0, 0, 2),

                    new PrintNode(0, 0, null),
                    new IdentifierNode(0, 0, "X"),
               });
            Assert.AreEqual(0, reporter.Errors.Count);
        }

        private void ASTPreOrderMatches(Node node, IList<Node> nodesPreorder)
        {
            CheckPreorder(node, nodesPreorder);
            Assert.AreEqual(0, nodesPreorder.Count);
        }

        private void CheckPreorder(Node node, IList<Node> nodesPreOrder)
        {
            Assert.AreNotEqual(nodesPreOrder.Count, 0);
            Assert.AreEqual(nodesPreOrder[0], node);

            nodesPreOrder.RemoveAt(0);

            foreach (var child in node.Children)
            {
                CheckPreorder(child, nodesPreOrder);
            }
        }
    }
}