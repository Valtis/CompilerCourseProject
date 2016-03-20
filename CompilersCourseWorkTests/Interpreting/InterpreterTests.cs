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

    }
}