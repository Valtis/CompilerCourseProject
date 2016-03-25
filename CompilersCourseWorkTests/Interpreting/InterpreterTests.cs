using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace CompilersCourseWork.Interpreting.Tests
{
    [TestClass()]
    public class InterpreterTests
    {
        [TestMethod()]
        public void PushIntegerWorks()
        {
            var bytecode = new List<byte>();
            bytecode.Add(Bytecode.PUSH_INT);
            bytecode.AddRange(BitConverter.GetBytes(23L));

            var interpreter = new Interpreter(bytecode.ToArray(), new List<string> { "" }, 0);
            interpreter.Run();
            Assert.AreEqual(1, interpreter.Stack.Count);
            Assert.AreEqual(23, interpreter.Stack.Pop());
            Assert.AreEqual(9, interpreter.PC);
        }

        [TestMethod()]
        public void PushStringWorks()
        {
            var bytecode = new List<byte>();
            bytecode.Add(Bytecode.PUSH_STRING);
            bytecode.AddRange(BitConverter.GetBytes(4568));

            var interpreter = new Interpreter(bytecode.ToArray(), new List<string> { "" }, 0);
            interpreter.Run();
            Assert.AreEqual(1, interpreter.Stack.Count);
            Assert.AreEqual(4568, interpreter.Stack.Pop());
            Assert.AreEqual(5, interpreter.PC);
        }

        [TestMethod()]
        public void PushFalseWorks()
        {
            var bytecode = new List<byte>();
            bytecode.Add(Bytecode.PUSH_FALSE);

            var interpreter = new Interpreter(bytecode.ToArray(), new List<string> { "" }, 0);
            interpreter.Run();
            Assert.AreEqual(1, interpreter.Stack.Count);
            Assert.AreEqual(0, interpreter.Stack.Pop());
            Assert.AreEqual(1, interpreter.PC);
        }

        [TestMethod()]
        public void PushIntVarWorks()
        {
            var bytecode = new List<byte>();
            bytecode.Add(Bytecode.PUSH_INT_VAR);
            bytecode.AddRange(BitConverter.GetBytes(2));

            var interpreter = new Interpreter(bytecode.ToArray(), new List<string> { "" }, 4);
            interpreter.Variables[2] = 2324;

            interpreter.Run();
            Assert.AreEqual(1, interpreter.Stack.Count);
            Assert.AreEqual(2324, interpreter.Stack.Pop());
            Assert.AreEqual(5, interpreter.PC);
        }

        [TestMethod()]
        public void PushStringVarWorks()
        {
            var bytecode = new List<byte>();
            bytecode.Add(Bytecode.PUSH_STRING_VAR);
            bytecode.AddRange(BitConverter.GetBytes(3));

            var interpreter = new Interpreter(bytecode.ToArray(), new List<string> { "" }, 4);
            interpreter.Variables[3] = 25;

            interpreter.Run();
            Assert.AreEqual(1, interpreter.Stack.Count);
            Assert.AreEqual(25, interpreter.Stack.Pop());
            Assert.AreEqual(5, interpreter.PC);
        }

        [TestMethod()]
        public void PushBooleanVarWorks()
        {
            var bytecode = new List<byte>();
            bytecode.Add(Bytecode.PUSH_BOOLEAN_VAR);
            bytecode.AddRange(BitConverter.GetBytes(8));

            var interpreter = new Interpreter(bytecode.ToArray(), new List<string> { "" }, 9);
            interpreter.Variables[8] = 1;

            interpreter.Run();
            Assert.AreEqual(1, interpreter.Stack.Count);
            Assert.AreEqual(1, interpreter.Stack.Pop());
            Assert.AreEqual(5, interpreter.PC);
        }

        [TestMethod()]
        public void StoreVariableWorks()
        {
            var bytecode = new List<byte>();
            bytecode.Add(Bytecode.PUSH_INT);
            bytecode.AddRange(BitConverter.GetBytes(23L));
            bytecode.Add(Bytecode.STORE_VARIABLE);
            bytecode.AddRange(BitConverter.GetBytes(14));

            var interpreter = new Interpreter(bytecode.ToArray(), new List<string> { "" }, 15);
            interpreter.Run();

            Assert.AreEqual(0, interpreter.Stack.Count);
            Assert.AreEqual(23, interpreter.Variables[14]);
            Assert.AreEqual(14, interpreter.PC);
        }

        [TestMethod()]
        public void IsLessIntPushesTrueIfFirstValueIsLess()
        {
            var bytecode = new List<byte>();
            bytecode.Add(Bytecode.PUSH_INT);
            bytecode.AddRange(BitConverter.GetBytes(23L));
            bytecode.Add(Bytecode.PUSH_INT);
            bytecode.AddRange(BitConverter.GetBytes(29L));
            bytecode.Add(Bytecode.IS_LESS_INT);

            var interpreter = new Interpreter(bytecode.ToArray(), new List<string> { "" }, 0);
            interpreter.Run();

            Assert.AreEqual(1, interpreter.Stack.Count);
            Assert.AreEqual(1, interpreter.Stack.Pop());
            Assert.AreEqual(19, interpreter.PC);
        }

        [TestMethod()]
        public void IsLessIntPushesFalseIfValuesAreEqual()
        {
            var bytecode = new List<byte>();
            bytecode.Add(Bytecode.PUSH_INT);
            bytecode.AddRange(BitConverter.GetBytes(23L));
            bytecode.Add(Bytecode.PUSH_INT);
            bytecode.AddRange(BitConverter.GetBytes(23L));
            bytecode.Add(Bytecode.IS_LESS_INT);

            var interpreter = new Interpreter(bytecode.ToArray(), new List<string> { "" }, 0);
            interpreter.Run();

            Assert.AreEqual(1, interpreter.Stack.Count);
            Assert.AreEqual(0, interpreter.Stack.Pop());
            Assert.AreEqual(19, interpreter.PC);
        }

        [TestMethod()]
        public void IsLessIntPushesFalseIfFirstValueIsLarger()
        {
            var bytecode = new List<byte>();
            bytecode.Add(Bytecode.PUSH_INT);
            bytecode.AddRange(BitConverter.GetBytes(53L));
            bytecode.Add(Bytecode.PUSH_INT);
            bytecode.AddRange(BitConverter.GetBytes(29L));
            bytecode.Add(Bytecode.IS_LESS_INT);

            var interpreter = new Interpreter(bytecode.ToArray(), new List<string> { "" }, 0);
            interpreter.Run();

            Assert.AreEqual(1, interpreter.Stack.Count);
            Assert.AreEqual(0, interpreter.Stack.Pop());
            Assert.AreEqual(19, interpreter.PC);
        }

        [TestMethod()]
        public void IsLessBooleanPushesTrueIfFirstValueIsLess()
        {
            var bytecode = new List<byte>();
            bytecode.Add(Bytecode.IS_LESS_BOOLEAN);

            var interpreter = new Interpreter(bytecode.ToArray(), new List<string> { "" }, 0);
            interpreter.Stack.Push(0);
            interpreter.Stack.Push(1);
            interpreter.Run();

            Assert.AreEqual(1, interpreter.Stack.Count);
            Assert.AreEqual(1, interpreter.Stack.Pop());
            Assert.AreEqual(1, interpreter.PC);
        }

        [TestMethod()]
        public void IsLessBooleanPushesFalseIfValuesAreEqual()
        {
            var bytecode = new List<byte>();
            bytecode.Add(Bytecode.IS_LESS_BOOLEAN);

            var interpreter = new Interpreter(bytecode.ToArray(), new List<string> { "" }, 0);
            interpreter.Stack.Push(1);
            interpreter.Stack.Push(1);
            interpreter.Run();

            Assert.AreEqual(1, interpreter.Stack.Count);
            Assert.AreEqual(0, interpreter.Stack.Pop());
            Assert.AreEqual(1, interpreter.PC);
        }

        [TestMethod()]
        public void IsLessBooleanPushesFalseIfFirstValueIsLarger()
        {
            var bytecode = new List<byte>();
            bytecode.Add(Bytecode.IS_LESS_BOOLEAN);

            var interpreter = new Interpreter(bytecode.ToArray(), new List<string> { "" }, 0);
            interpreter.Stack.Push(1);
            interpreter.Stack.Push(0);
            interpreter.Run();

            Assert.AreEqual(1, interpreter.Stack.Count);
            Assert.AreEqual(0, interpreter.Stack.Pop());
            Assert.AreEqual(1, interpreter.PC);
        }

        [TestMethod()]
        public void IsEqualIntPushesFalseIfFirstValueIsLess()
        {
            var bytecode = new List<byte>();
            bytecode.Add(Bytecode.PUSH_INT);
            bytecode.AddRange(BitConverter.GetBytes(23L));
            bytecode.Add(Bytecode.PUSH_INT);
            bytecode.AddRange(BitConverter.GetBytes(29L));
            bytecode.Add(Bytecode.IS_EQUAL_INT);

            var interpreter = new Interpreter(bytecode.ToArray(), new List<string> { "" }, 0);
            interpreter.Run();

            Assert.AreEqual(1, interpreter.Stack.Count);
            Assert.AreEqual(0, interpreter.Stack.Pop());
            Assert.AreEqual(19, interpreter.PC);
        }

        [TestMethod()]
        public void IsEqualIntPushesTrueIfValuesAreEqual()
        {
            var bytecode = new List<byte>();
            bytecode.Add(Bytecode.PUSH_INT);
            bytecode.AddRange(BitConverter.GetBytes(23L));
            bytecode.Add(Bytecode.PUSH_INT);
            bytecode.AddRange(BitConverter.GetBytes(23L));
            bytecode.Add(Bytecode.IS_EQUAL_INT);

            var interpreter = new Interpreter(bytecode.ToArray(), new List<string> { "" }, 0);
            interpreter.Run();

            Assert.AreEqual(1, interpreter.Stack.Count);
            Assert.AreEqual(1, interpreter.Stack.Pop());
            Assert.AreEqual(19, interpreter.PC);
        }

        [TestMethod()]
        public void IsEqualIntPushesFalseIfFirstValueIsLarger()
        {
            var bytecode = new List<byte>();
            bytecode.Add(Bytecode.PUSH_INT);
            bytecode.AddRange(BitConverter.GetBytes(53L));
            bytecode.Add(Bytecode.PUSH_INT);
            bytecode.AddRange(BitConverter.GetBytes(29L));
            bytecode.Add(Bytecode.IS_EQUAL_INT);

            var interpreter = new Interpreter(bytecode.ToArray(), new List<string> { "" }, 0);
            interpreter.Run();

            Assert.AreEqual(1, interpreter.Stack.Count);
            Assert.AreEqual(0, interpreter.Stack.Pop());
            Assert.AreEqual(19, interpreter.PC);
        }

        [TestMethod()]
        public void IsEqualBooleanPushesFalseIfFirstValueIsLess()
        {
            var bytecode = new List<byte>();
            bytecode.Add(Bytecode.IS_EQUAL_BOOLEAN);

            var interpreter = new Interpreter(bytecode.ToArray(), new List<string> { "" }, 0);
            interpreter.Stack.Push(0);
            interpreter.Stack.Push(1);
            interpreter.Run();

            Assert.AreEqual(1, interpreter.Stack.Count);
            Assert.AreEqual(0, interpreter.Stack.Pop());
            Assert.AreEqual(1, interpreter.PC);
        }

        [TestMethod()]
        public void IsEqualBooleanPushesTrueIfValuesAreEqual()
        {
            var bytecode = new List<byte>();
            bytecode.Add(Bytecode.IS_EQUAL_BOOLEAN);

            var interpreter = new Interpreter(bytecode.ToArray(), new List<string> { "" }, 0);
            interpreter.Stack.Push(1);
            interpreter.Stack.Push(1);
            interpreter.Run();

            Assert.AreEqual(1, interpreter.Stack.Count);
            Assert.AreEqual(1, interpreter.Stack.Pop());
            Assert.AreEqual(1, interpreter.PC);
        }

        [TestMethod()]
        public void IsEqualBooleanPushesFalseIfFirstValueIsLarger()
        {
            var bytecode = new List<byte>();
            bytecode.Add(Bytecode.IS_EQUAL_BOOLEAN);

            var interpreter = new Interpreter(bytecode.ToArray(), new List<string> { "" }, 0);
            interpreter.Stack.Push(1);
            interpreter.Stack.Push(0);
            interpreter.Run();

            Assert.AreEqual(1, interpreter.Stack.Count);
            Assert.AreEqual(0, interpreter.Stack.Pop());
            Assert.AreEqual(1, interpreter.PC);
        }

        [TestMethod()]
        public void IsLessStringPushesTrueIfFirstStringIsAlphabeticallyBeforeSecond()
        {

            var bytecode = new List<byte>();
            bytecode.Add(Bytecode.IS_LESS_STRING);

            var interpreter = new Interpreter(bytecode.ToArray(), new List<string> { "hello", "world" }, 0);
            interpreter.Stack.Push(0);
            interpreter.Stack.Push(1);
            interpreter.Run();

            Assert.AreEqual(1, interpreter.Stack.Count);
            Assert.AreEqual(1, interpreter.Stack.Pop());
            Assert.AreEqual(1, interpreter.PC);
        }

        [TestMethod()]
        public void IsLessStringPushesFalseIfStringsAreEqual()
        {

            var bytecode = new List<byte>();
            bytecode.Add(Bytecode.IS_LESS_STRING);

            var interpreter = new Interpreter(bytecode.ToArray(), new List<string> { "hello", "hello" }, 0);
            interpreter.Stack.Push(0);
            interpreter.Stack.Push(1);
            interpreter.Run();

            Assert.AreEqual(1, interpreter.Stack.Count);
            Assert.AreEqual(0, interpreter.Stack.Pop());
            Assert.AreEqual(1, interpreter.PC);
        }

        [TestMethod()]
        public void IsLessStringPushesFalseIfFirstStringIsAlphabeticallyAfterSecond()
        {

            var bytecode = new List<byte>();
            bytecode.Add(Bytecode.IS_LESS_STRING);

            var interpreter = new Interpreter(bytecode.ToArray(), new List<string> { "world", "hello" }, 0);
            interpreter.Stack.Push(0);
            interpreter.Stack.Push(1);
            interpreter.Run();

            Assert.AreEqual(1, interpreter.Stack.Count);
            Assert.AreEqual(0, interpreter.Stack.Pop());
            Assert.AreEqual(1, interpreter.PC);
        }

        [TestMethod()]
        public void IsEqualStringPushesFalseIfFirstStringIsAlphabeticallyBeforeSecond()
        {

            var bytecode = new List<byte>();
            bytecode.Add(Bytecode.IS_EQUAL_STRING);

            var interpreter = new Interpreter(bytecode.ToArray(), new List<string> { "hello", "world" }, 0);
            interpreter.Stack.Push(0);
            interpreter.Stack.Push(1);
            interpreter.Run();

            Assert.AreEqual(1, interpreter.Stack.Count);
            Assert.AreEqual(0, interpreter.Stack.Pop());
            Assert.AreEqual(1, interpreter.PC);
        }

        [TestMethod()]
        public void IsEqualStringPushesTrueIfStringsAreEqual()
        {

            var bytecode = new List<byte>();
            bytecode.Add(Bytecode.IS_EQUAL_STRING);

            var interpreter = new Interpreter(bytecode.ToArray(), new List<string> { "hello", "hello" }, 0);
            interpreter.Stack.Push(0);
            interpreter.Stack.Push(1);
            interpreter.Run();

            Assert.AreEqual(1, interpreter.Stack.Count);
            Assert.AreEqual(1, interpreter.Stack.Pop());
            Assert.AreEqual(1, interpreter.PC);
        }

        [TestMethod()]
        public void IsEqualStringPushesFalseIfFirstStringIsAlphabeticallyAfterSecond()
        {

            var bytecode = new List<byte>();
            bytecode.Add(Bytecode.IS_EQUAL_STRING);

            var interpreter = new Interpreter(bytecode.ToArray(), new List<string> { "world", "hello" }, 0);
            interpreter.Stack.Push(0);
            interpreter.Stack.Push(1);
            interpreter.Run();

            Assert.AreEqual(1, interpreter.Stack.Count);
            Assert.AreEqual(0, interpreter.Stack.Pop());
            Assert.AreEqual(1, interpreter.PC);
        }

        [TestMethod()]
        public void NotTurnsTrueIntoFalse()
        {
            var bytecode = new List<byte>();
            bytecode.Add(Bytecode.NOT);

            var interpreter = new Interpreter(bytecode.ToArray(), new List<string> {}, 0);
            interpreter.Stack.Push(1);
            interpreter.Run();

            Assert.AreEqual(1, interpreter.Stack.Count);
            Assert.AreEqual(0, interpreter.Stack.Pop());
            Assert.AreEqual(1, interpreter.PC);
        }

        [TestMethod()]
        public void NotTurnsFalseIntoTrue()
        {
            var bytecode = new List<byte>();
            bytecode.Add(Bytecode.NOT);

            var interpreter = new Interpreter(bytecode.ToArray(), new List<string> { }, 0);
            interpreter.Stack.Push(0);
            interpreter.Run();

            Assert.AreEqual(1, interpreter.Stack.Count);
            Assert.AreEqual(1, interpreter.Stack.Pop());
            Assert.AreEqual(1, interpreter.PC);
        }

        [TestMethod()]
        public void AndPushesTrueIfStackContainsTrueAndTrue()
        {
            var bytecode = new List<byte>();
            bytecode.Add(Bytecode.AND);

            var interpreter = new Interpreter(bytecode.ToArray(), new List<string> { }, 0);
            interpreter.Stack.Push(1);
            interpreter.Stack.Push(1);
            interpreter.Run();

            Assert.AreEqual(1, interpreter.Stack.Count);
            Assert.AreEqual(1, interpreter.Stack.Pop());
            Assert.AreEqual(1, interpreter.PC);
        }

        [TestMethod()]
        public void AndPushesFalseIfStackContainsTrueAndFalse()
        {
            var bytecode = new List<byte>();
            bytecode.Add(Bytecode.AND);

            var interpreter = new Interpreter(bytecode.ToArray(), new List<string> { }, 0);
            interpreter.Stack.Push(1);
            interpreter.Stack.Push(0);
            interpreter.Run();

            Assert.AreEqual(1, interpreter.Stack.Count);
            Assert.AreEqual(0, interpreter.Stack.Pop());
            Assert.AreEqual(1, interpreter.PC);
        }

        [TestMethod()]
        public void AndPushesFalseIfStackContainsFalseAndTrue()
        {
            var bytecode = new List<byte>();
            bytecode.Add(Bytecode.AND);

            var interpreter = new Interpreter(bytecode.ToArray(), new List<string> { }, 0);
            interpreter.Stack.Push(0);
            interpreter.Stack.Push(1);
            interpreter.Run();

            Assert.AreEqual(1, interpreter.Stack.Count);
            Assert.AreEqual(0, interpreter.Stack.Pop());
            Assert.AreEqual(1, interpreter.PC);
        }

        [TestMethod()]
        public void AndPushesFalseIfStackContainsFalseAndFalse()
        {
            var bytecode = new List<byte>();
            bytecode.Add(Bytecode.AND);

            var interpreter = new Interpreter(bytecode.ToArray(), new List<string> { }, 0);
            interpreter.Stack.Push(0);
            interpreter.Stack.Push(0);
            interpreter.Run();

            Assert.AreEqual(1, interpreter.Stack.Count);
            Assert.AreEqual(0, interpreter.Stack.Pop());
            Assert.AreEqual(1, interpreter.PC);
        }

        [TestMethod()]
        public void PrintIntWorks()
        {
            var bytecode = new List<byte>();
            bytecode.Add(Bytecode.PUSH_INT);
            bytecode.AddRange(BitConverter.GetBytes(23L));
            bytecode.Add(Bytecode.PRINT_INT);

            var interpreter = new Interpreter(bytecode.ToArray(), new List<string> { "" }, 0);
            var output = new List<string>();
            interpreter.SetPrinter((string str) => output.Add(str));
            interpreter.Run();

            Assert.AreEqual(0, interpreter.Stack.Count);
            Assert.AreEqual(10, interpreter.PC);
            Assert.AreEqual(1, output.Count);
            Assert.AreEqual("23", output[0]);
        }

        [TestMethod()]
        public void PrintStringWorks()
        {
            var bytecode = new List<byte>();
            bytecode.Add(Bytecode.PUSH_STRING);
            bytecode.AddRange(BitConverter.GetBytes(1));
            bytecode.Add(Bytecode.PRINT_STRING);

            var interpreter = new Interpreter(bytecode.ToArray(), new List<string> { "hello", "world" }, 0);
            var output = new List<string>();
            interpreter.SetPrinter((string str) => output.Add(str));
            interpreter.Run();

            Assert.AreEqual(0, interpreter.Stack.Count);
            Assert.AreEqual(6, interpreter.PC);
            Assert.AreEqual(1, output.Count);
            Assert.AreEqual("world", output[0]);
        }

        [TestMethod()]
        public void AddWorks()
        {
            var bytecode = new List<byte>();
            bytecode.Add(Bytecode.PUSH_INT);
            bytecode.AddRange(BitConverter.GetBytes(23L));
            bytecode.Add(Bytecode.PUSH_INT);
            bytecode.AddRange(BitConverter.GetBytes(170L));
            bytecode.Add(Bytecode.ADD);

            var interpreter = new Interpreter(bytecode.ToArray(), new List<string> { "" }, 0);
            interpreter.Run();

            Assert.AreEqual(1, interpreter.Stack.Count);
            Assert.AreEqual(193, interpreter.Stack.Pop());
            Assert.AreEqual(19, interpreter.PC);
        }

        [TestMethod()]
        public void SubWorks()
        {
            var bytecode = new List<byte>();
            bytecode.Add(Bytecode.PUSH_INT);
            bytecode.AddRange(BitConverter.GetBytes(23L));
            bytecode.Add(Bytecode.PUSH_INT);
            bytecode.AddRange(BitConverter.GetBytes(170L));
            bytecode.Add(Bytecode.SUB);

            var interpreter = new Interpreter(bytecode.ToArray(), new List<string> { "" }, 0);
            interpreter.Run();

            Assert.AreEqual(1, interpreter.Stack.Count);
            Assert.AreEqual(23 - 170, interpreter.Stack.Pop());
            Assert.AreEqual(19, interpreter.PC);
        }

        [TestMethod()]
        public void MulWorks()
        {
            var bytecode = new List<byte>();
            bytecode.Add(Bytecode.PUSH_INT);
            bytecode.AddRange(BitConverter.GetBytes(23L));
            bytecode.Add(Bytecode.PUSH_INT);
            bytecode.AddRange(BitConverter.GetBytes(170L));
            bytecode.Add(Bytecode.MUL);

            var interpreter = new Interpreter(bytecode.ToArray(), new List<string> { "" }, 0);
            interpreter.Run();

            Assert.AreEqual(1, interpreter.Stack.Count);
            Assert.AreEqual(3910, interpreter.Stack.Pop());
            Assert.AreEqual(19, interpreter.PC);
        }

        [TestMethod()]
        public void DivWorksWithNonZeroDivider()
        {
            var bytecode = new List<byte>();
            bytecode.Add(Bytecode.PUSH_INT);
            bytecode.AddRange(BitConverter.GetBytes(20L));
            bytecode.Add(Bytecode.PUSH_INT);
            bytecode.AddRange(BitConverter.GetBytes(5L));
            bytecode.Add(Bytecode.DIV);

            var interpreter = new Interpreter(bytecode.ToArray(), new List<string> { "" }, 0);
            interpreter.Run();

            Assert.AreEqual(1, interpreter.Stack.Count);
            Assert.AreEqual(20 / 5, interpreter.Stack.Pop());
            Assert.AreEqual(19, interpreter.PC);
        }

        [TestMethod()]
        [ExpectedException(typeof(DivideByZeroException))]
        public void DivThrowsIfDividerIsZero()
        {
            var bytecode = new List<byte>();
            bytecode.Add(Bytecode.PUSH_INT);
            bytecode.AddRange(BitConverter.GetBytes(20L));
            bytecode.Add(Bytecode.PUSH_INT);
            bytecode.AddRange(BitConverter.GetBytes(0L));
            bytecode.Add(Bytecode.DIV);

            var interpreter = new Interpreter(bytecode.ToArray(), new List<string> { "" }, 0);
            interpreter.Run();
        }

        [TestMethod()]
        public void ConcatWorksWhenResultIsNewUniqueString()
        {
            var bytecode = new List<byte>();
            bytecode.Add(Bytecode.PUSH_STRING);
            bytecode.AddRange(BitConverter.GetBytes(0));
            bytecode.Add(Bytecode.PUSH_STRING);
            bytecode.AddRange(BitConverter.GetBytes(1));
            bytecode.Add(Bytecode.CONCAT);

            var interpreter = new Interpreter(bytecode.ToArray(), new List<string> { "hello ", "world" }, 0);
            interpreter.Run();

            Assert.AreEqual(1, interpreter.Stack.Count);
            Assert.AreEqual(2, interpreter.Stack.Pop());
            Assert.AreEqual(11, interpreter.PC);
            Assert.AreEqual("hello world", interpreter.Strings[2]);
        }

        [TestMethod()]
        public void ConcatWorksWhenResultIsNotNewUniqueString()
        {
            var bytecode = new List<byte>();
            bytecode.Add(Bytecode.PUSH_STRING);
            bytecode.AddRange(BitConverter.GetBytes(2));
            bytecode.Add(Bytecode.PUSH_STRING);
            bytecode.AddRange(BitConverter.GetBytes(3));
            bytecode.Add(Bytecode.CONCAT);

            var interpreter = new Interpreter(bytecode.ToArray(), new List<string> { "hello ", "world", "foo", "bar", "foobar" }, 0);
            interpreter.Run();

            Assert.AreEqual(1, interpreter.Stack.Count);
            Assert.AreEqual(4, interpreter.Stack.Pop());
            Assert.AreEqual(11, interpreter.PC);
        }

    }
}