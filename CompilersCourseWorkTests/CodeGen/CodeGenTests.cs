using Microsoft.VisualStudio.TestTools.UnitTesting;
using CompilersCourseWork.ErrorHandling;
using CompilersCourseWork.Parsing;
using CompilersCourseWork.SemanticChecking;
using CompilersCourseWork.Lexing;
using CompilersCourseWork.Interpreting;
using System.Collections.Generic;
using System;

namespace CompilersCourseWork.CodeGeneration.Tests
{
    [TestClass()]
    public class CodeGenTests
    {
        [TestMethod()]
        public void VariableDeclarationGeneratesCorrectCode()
        {
            var reporter = new ErrorReporter();
            var parser = new Parser(
                new Lexer("../../CodeGen/variable_declaration.txt", reporter),
                reporter);

            var node = parser.Parse();

            var semanticChecker = new SemanticChecker(reporter);

            node.Accept(semanticChecker);

            Assert.AreEqual(0, reporter.Errors.Count);

            var codeGenerator = new CodeGenerator(semanticChecker.SymbolTable);
            node.Accept(codeGenerator);

            var bytecode = codeGenerator.Bytecodes;
            Assert.AreEqual(Bytecode.PUSH_INT, bytecode[0]);
            Assert.AreEqual(0, GetLongValue(bytecode, 1));
            Assert.AreEqual(Bytecode.STORE_VARIABLE, bytecode[9]);
            Assert.AreEqual(0, GetIntValue(bytecode, 10));

            Assert.AreEqual(Bytecode.PUSH_STRING, bytecode[14]);
            Assert.AreEqual(0, GetIntValue(bytecode, 15));
            Assert.AreEqual(Bytecode.STORE_VARIABLE, bytecode[19]);
            Assert.AreEqual(1, GetIntValue(bytecode, 20));


            Assert.AreEqual(Bytecode.PUSH_FALSE, bytecode[24]);
            Assert.AreEqual(Bytecode.STORE_VARIABLE, bytecode[25]);
            Assert.AreEqual(2, GetIntValue(bytecode, 26));

            Assert.AreEqual(Bytecode.PUSH_INT, bytecode[30]);
            Assert.AreEqual(323981, GetLongValue(bytecode, 31));
            Assert.AreEqual(Bytecode.STORE_VARIABLE, bytecode[39]);
            Assert.AreEqual(3, GetIntValue(bytecode, 40));


            Assert.AreEqual(Bytecode.PUSH_STRING, bytecode[44]);
            Assert.AreEqual(1, GetIntValue(bytecode, 45));
            Assert.AreEqual(Bytecode.STORE_VARIABLE, bytecode[49]);
            Assert.AreEqual(4, GetIntValue(bytecode, 50));


            Assert.AreEqual(Bytecode.PUSH_INT_VAR, bytecode[54]);
            Assert.AreEqual(0, GetIntValue(bytecode, 55));
            Assert.AreEqual(Bytecode.PUSH_INT, bytecode[59]);
            Assert.AreEqual(4, GetLongValue(bytecode, 60));
            Assert.AreEqual(Bytecode.IS_LESS_INT, bytecode[68]);
            Assert.AreEqual(Bytecode.STORE_VARIABLE, bytecode[69]);
            Assert.AreEqual(5, GetIntValue(bytecode, 70));


            Assert.AreEqual(Bytecode.PUSH_INT_VAR, bytecode[74]);
            Assert.AreEqual(3, GetIntValue(bytecode, 75));
            Assert.AreEqual(Bytecode.PUSH_INT, bytecode[79]);
            Assert.AreEqual(298, GetLongValue(bytecode, 80));
            Assert.AreEqual(Bytecode.IS_EQUAL_INT, bytecode[88]);
            Assert.AreEqual(Bytecode.STORE_VARIABLE, bytecode[89]);
            Assert.AreEqual(6, GetIntValue(bytecode, 90));


            Assert.AreEqual(Bytecode.PUSH_INT_VAR, bytecode[94]);
            Assert.AreEqual(0, GetIntValue(bytecode, 95));
            Assert.AreEqual(Bytecode.PUSH_INT_VAR, bytecode[99]);
            Assert.AreEqual(3, GetIntValue(bytecode, 100));
            Assert.AreEqual(Bytecode.IS_LESS_INT, bytecode[104]);
            Assert.AreEqual(Bytecode.NOT, bytecode[105]);
            Assert.AreEqual(Bytecode.STORE_VARIABLE, bytecode[106]);
            Assert.AreEqual(7, GetIntValue(bytecode, 107));

            Assert.AreEqual(Bytecode.PUSH_INT, bytecode[111]);
            Assert.AreEqual(2, GetLongValue(bytecode, 112));
            Assert.AreEqual(Bytecode.PUSH_INT, bytecode[120]);
            Assert.AreEqual(3, GetLongValue(bytecode, 121));
            Assert.AreEqual(Bytecode.ADD, bytecode[129]);
            Assert.AreEqual(Bytecode.STORE_VARIABLE, bytecode[130]);
            Assert.AreEqual(8, GetIntValue(bytecode, 131));

            Assert.AreEqual(Bytecode.PUSH_INT_VAR, bytecode[135]);
            Assert.AreEqual(0, GetIntValue(bytecode, 136));
            Assert.AreEqual(Bytecode.PUSH_INT_VAR, bytecode[140]);
            Assert.AreEqual(3, GetIntValue(bytecode, 141));
            Assert.AreEqual(Bytecode.SUB, bytecode[145]);
            Assert.AreEqual(Bytecode.STORE_VARIABLE, bytecode[146]);
            Assert.AreEqual(9, GetIntValue(bytecode, 147));

            Assert.AreEqual(Bytecode.PUSH_INT_VAR, bytecode[151]);
            Assert.AreEqual(0, GetIntValue(bytecode, 152));
            Assert.AreEqual(Bytecode.PUSH_INT_VAR, bytecode[156]);
            Assert.AreEqual(3, GetIntValue(bytecode, 157));
            Assert.AreEqual(Bytecode.MUL, bytecode[161]);
            Assert.AreEqual(Bytecode.STORE_VARIABLE, bytecode[162]);
            Assert.AreEqual(10, GetIntValue(bytecode, 163));

            Assert.AreEqual(Bytecode.PUSH_INT_VAR, bytecode[167]);
            Assert.AreEqual(0, GetIntValue(bytecode, 168));
            Assert.AreEqual(Bytecode.PUSH_INT_VAR, bytecode[172]);
            Assert.AreEqual(3, GetIntValue(bytecode, 173));
            Assert.AreEqual(Bytecode.DIV, bytecode[177]);
            Assert.AreEqual(Bytecode.STORE_VARIABLE, bytecode[178]);
            Assert.AreEqual(11, GetIntValue(bytecode, 179));
            
            Assert.AreEqual(Bytecode.PUSH_INT, bytecode[183]);
            Assert.AreEqual(2, GetLongValue(bytecode, 184));
            Assert.AreEqual(Bytecode.PUSH_INT, bytecode[192]);
            Assert.AreEqual(3, GetIntValue(bytecode, 193));
            Assert.AreEqual(Bytecode.MUL, bytecode[201]);
            Assert.AreEqual(Bytecode.PUSH_INT, bytecode[202]);
            Assert.AreEqual(4, GetLongValue(bytecode, 203));
            Assert.AreEqual(Bytecode.PUSH_INT, bytecode[211]);
            Assert.AreEqual(2, GetLongValue(bytecode, 212));
            Assert.AreEqual(Bytecode.DIV, bytecode[220]);
            Assert.AreEqual(Bytecode.SUB, bytecode[221]);
            Assert.AreEqual(Bytecode.STORE_VARIABLE, bytecode[222]);
            Assert.AreEqual(12, GetIntValue(bytecode, 223));

            Assert.AreEqual(Bytecode.PUSH_STRING_VAR, bytecode[227]);
            Assert.AreEqual(1, GetIntValue(bytecode, 228));
            Assert.AreEqual(Bytecode.PUSH_STRING, bytecode[232]);
            Assert.AreEqual(2, GetIntValue(bytecode, 233));
            Assert.AreEqual(Bytecode.CONCAT, bytecode[237]);
            Assert.AreEqual(Bytecode.STORE_VARIABLE, bytecode[238]);
            Assert.AreEqual(13, GetIntValue(bytecode, 239));

            Assert.AreEqual(Bytecode.PUSH_STRING_VAR, bytecode[243]);
            Assert.AreEqual(1, GetIntValue(bytecode, 244));
            Assert.AreEqual(Bytecode.PUSH_STRING, bytecode[248]);
            Assert.AreEqual(2, GetIntValue(bytecode, 249));
            Assert.AreEqual(Bytecode.IS_LESS_STRING, bytecode[253]);
            Assert.AreEqual(Bytecode.STORE_VARIABLE, bytecode[254]);
            Assert.AreEqual(14, GetIntValue(bytecode, 255));

            Assert.AreEqual(Bytecode.PUSH_STRING, bytecode[259]);
            Assert.AreEqual(3, GetIntValue(bytecode, 260));
            Assert.AreEqual(Bytecode.PUSH_STRING, bytecode[264]);
            Assert.AreEqual(4, GetIntValue(bytecode, 265));
            Assert.AreEqual(Bytecode.IS_EQUAL_STRING, bytecode[269]);
            Assert.AreEqual(Bytecode.STORE_VARIABLE, bytecode[270]);
            Assert.AreEqual(15, GetIntValue(bytecode, 271));

            Assert.AreEqual(Bytecode.PUSH_BOOLEAN_VAR, bytecode[275]);
            Assert.AreEqual(5, GetIntValue(bytecode, 276));
            Assert.AreEqual(Bytecode.PUSH_BOOLEAN_VAR, bytecode[280]);
            Assert.AreEqual(6, GetIntValue(bytecode, 281));
            Assert.AreEqual(Bytecode.IS_LESS_BOOLEAN, bytecode[285]);
            Assert.AreEqual(Bytecode.STORE_VARIABLE, bytecode[286]);
            Assert.AreEqual(16, GetIntValue(bytecode, 287));

            Assert.AreEqual(Bytecode.PUSH_BOOLEAN_VAR, bytecode[291]);
            Assert.AreEqual(5, GetIntValue(bytecode, 292));
            Assert.AreEqual(Bytecode.PUSH_BOOLEAN_VAR, bytecode[296]);
            Assert.AreEqual(6, GetIntValue(bytecode, 297));
            Assert.AreEqual(Bytecode.IS_EQUAL_BOOLEAN, bytecode[301]);
            Assert.AreEqual(Bytecode.STORE_VARIABLE, bytecode[302]);
            Assert.AreEqual(17, GetIntValue(bytecode, 303));


            Assert.AreEqual(Bytecode.PUSH_BOOLEAN_VAR, bytecode[307]);
            Assert.AreEqual(5, GetIntValue(bytecode, 308));
            Assert.AreEqual(Bytecode.PUSH_BOOLEAN_VAR, bytecode[312]);
            Assert.AreEqual(6, GetIntValue(bytecode, 313));
            Assert.AreEqual(Bytecode.AND, bytecode[317]);
            Assert.AreEqual(Bytecode.STORE_VARIABLE, bytecode[318]);
            Assert.AreEqual(18, GetIntValue(bytecode, 319));
        }

        [TestMethod()]
        public void VariableAssignmentGeneratesCorrectCode()
        {
            var reporter = new ErrorReporter();
            var parser = new Parser(
                new Lexer("../../CodeGen/variable_assignment.txt", reporter),
                reporter);

            var node = parser.Parse();

            var semanticChecker = new SemanticChecker(reporter);

            node.Accept(semanticChecker);

            Assert.AreEqual(0, reporter.Errors.Count);

            var codeGenerator = new CodeGenerator(semanticChecker.SymbolTable);
            node.Accept(codeGenerator);

            var bytecode = codeGenerator.Bytecodes;

            Assert.AreEqual(Bytecode.PUSH_INT, bytecode[0]);
            Assert.AreEqual(0, GetLongValue(bytecode, 1));
            Assert.AreEqual(Bytecode.STORE_VARIABLE, bytecode[9]);
            Assert.AreEqual(0, GetIntValue(bytecode, 10));

            Assert.AreEqual(Bytecode.PUSH_STRING, bytecode[14]);
            Assert.AreEqual(0, GetIntValue(bytecode, 15));
            Assert.AreEqual(Bytecode.STORE_VARIABLE, bytecode[19]);
            Assert.AreEqual(1, GetIntValue(bytecode, 20));
            
            Assert.AreEqual(Bytecode.PUSH_FALSE, bytecode[24]);
            Assert.AreEqual(Bytecode.STORE_VARIABLE, bytecode[25]);
            Assert.AreEqual(2, GetIntValue(bytecode, 26));

            Assert.AreEqual(Bytecode.PUSH_INT, bytecode[30]);
            Assert.AreEqual(4, GetLongValue(bytecode, 31));
            Assert.AreEqual(Bytecode.PUSH_INT_VAR, bytecode[39]);
            Assert.AreEqual(0, GetIntValue(bytecode, 40));
            Assert.AreEqual(Bytecode.MUL, bytecode[44]);
            Assert.AreEqual(Bytecode.STORE_VARIABLE, bytecode[45]);
            Assert.AreEqual(0, GetIntValue(bytecode, 46));

            Assert.AreEqual(Bytecode.PUSH_STRING_VAR, bytecode[50]);
            Assert.AreEqual(1, GetIntValue(bytecode, 51));
            Assert.AreEqual(Bytecode.PUSH_STRING, bytecode[55]);
            Assert.AreEqual(1, GetIntValue(bytecode, 56));
            Assert.AreEqual(Bytecode.CONCAT, bytecode[60]);
            Assert.AreEqual(Bytecode.STORE_VARIABLE, bytecode[61]);
            Assert.AreEqual(1, GetIntValue(bytecode, 62));

            Assert.AreEqual(Bytecode.PUSH_INT_VAR, bytecode[66]);
            Assert.AreEqual(0, GetIntValue(bytecode, 67));
            Assert.AreEqual(Bytecode.PUSH_INT, bytecode[71]);
            Assert.AreEqual(9876543210, GetLongValue(bytecode, 72));
            Assert.AreEqual(Bytecode.IS_LESS_INT, bytecode[80]);
            Assert.AreEqual(Bytecode.NOT, bytecode[81]);
            Assert.AreEqual(Bytecode.PUSH_STRING_VAR, bytecode[82]);
            Assert.AreEqual(1, GetIntValue(bytecode, 83));
            Assert.AreEqual(Bytecode.PUSH_STRING, bytecode[87]);
            Assert.AreEqual(2, GetIntValue(bytecode, 88));
            Assert.AreEqual(Bytecode.IS_EQUAL_STRING, bytecode[92]);
            Assert.AreEqual(Bytecode.AND, bytecode[93]);
            Assert.AreEqual(Bytecode.STORE_VARIABLE, bytecode[94]);
            Assert.AreEqual(2, GetIntValue(bytecode, 95));
        }


        [TestMethod()]
        public void PrintStatementGeneratesCorrectCode()
        {
            var reporter = new ErrorReporter();
            var parser = new Parser(
                new Lexer("../../CodeGen/print_statement.txt", reporter),
                reporter);

            var node = parser.Parse();

            var semanticChecker = new SemanticChecker(reporter);

            node.Accept(semanticChecker);

            Assert.AreEqual(0, reporter.Errors.Count);

            var codeGenerator = new CodeGenerator(semanticChecker.SymbolTable);
            node.Accept(codeGenerator);

            var bytecode = codeGenerator.Bytecodes;

            Assert.AreEqual(Bytecode.PUSH_INT, bytecode[0]);
            Assert.AreEqual(0, GetLongValue(bytecode, 1));
            Assert.AreEqual(Bytecode.STORE_VARIABLE, bytecode[9]);
            Assert.AreEqual(0, GetIntValue(bytecode, 10));

            Assert.AreEqual(Bytecode.PUSH_STRING, bytecode[14]);
            Assert.AreEqual(0, GetIntValue(bytecode, 15));
            Assert.AreEqual(Bytecode.STORE_VARIABLE, bytecode[19]);
            Assert.AreEqual(1, GetIntValue(bytecode, 20));

            Assert.AreEqual(Bytecode.PUSH_INT, bytecode[24]);
            Assert.AreEqual(4, GetLongValue(bytecode, 25));
            Assert.AreEqual(Bytecode.PRINT_INT, bytecode[33]);

            Assert.AreEqual(Bytecode.PUSH_STRING, bytecode[34]);
            Assert.AreEqual(1, GetIntValue(bytecode, 35));
            Assert.AreEqual(Bytecode.PRINT_STRING, bytecode[39]);

            Assert.AreEqual(Bytecode.PUSH_INT, bytecode[40]);
            Assert.AreEqual(4, GetLongValue(bytecode, 41));
            Assert.AreEqual(Bytecode.PUSH_INT, bytecode[49]);
            Assert.AreEqual(9, GetLongValue(bytecode, 50));
            Assert.AreEqual(Bytecode.ADD, bytecode[58]);
            Assert.AreEqual(Bytecode.PRINT_INT, bytecode[59]);

            Assert.AreEqual(Bytecode.PUSH_STRING, bytecode[60]);
            Assert.AreEqual(2, GetIntValue(bytecode, 61));
            Assert.AreEqual(Bytecode.PUSH_STRING, bytecode[65]);
            Assert.AreEqual(3, GetIntValue(bytecode, 66));
            Assert.AreEqual(Bytecode.CONCAT, bytecode[70]);
            Assert.AreEqual(Bytecode.PRINT_STRING, bytecode[71]);

            Assert.AreEqual(Bytecode.PUSH_STRING, bytecode[72]);
            Assert.AreEqual(4, GetIntValue(bytecode, 73));
            Assert.AreEqual(Bytecode.PUSH_STRING, bytecode[77]);
            Assert.AreEqual(5, GetIntValue(bytecode, 78));
            Assert.AreEqual(Bytecode.CONCAT, bytecode[82]);
            Assert.AreEqual(Bytecode.STORE_VARIABLE, bytecode[83]);
            Assert.AreEqual(1, GetIntValue(bytecode, 84));
            Assert.AreEqual(Bytecode.PUSH_STRING_VAR, bytecode[88]);
            Assert.AreEqual(1, GetIntValue(bytecode, 89));
            Assert.AreEqual(Bytecode.PRINT_STRING, bytecode[93]);

            Assert.AreEqual(Bytecode.PUSH_INT, bytecode[94]);
            Assert.AreEqual(2, GetLongValue(bytecode, 95));
            Assert.AreEqual(Bytecode.PUSH_INT, bytecode[103]);
            Assert.AreEqual(3, GetLongValue(bytecode, 104));
            Assert.AreEqual(Bytecode.MUL, bytecode[112]);
            Assert.AreEqual(Bytecode.STORE_VARIABLE, bytecode[113]);
            Assert.AreEqual(0, GetIntValue(bytecode, 114));
            Assert.AreEqual(Bytecode.PUSH_INT_VAR, bytecode[118]);
            Assert.AreEqual(0, GetIntValue(bytecode, 119));
            Assert.AreEqual(Bytecode.PUSH_INT, bytecode[123]);
            Assert.AreEqual(1, GetLongValue(bytecode, 124));
            Assert.AreEqual(Bytecode.SUB, bytecode[132]);
            Assert.AreEqual(Bytecode.PRINT_INT, bytecode[133]);            
        }

        private long GetLongValue(byte[] bytecodes, int startIndex)
        {
            return BitConverter.ToInt64(bytecodes, startIndex);
        }

        private long GetIntValue(byte[] bytecodes, int startIndex)
        {
            return BitConverter.ToInt32(bytecodes, startIndex);
        }

    }
}