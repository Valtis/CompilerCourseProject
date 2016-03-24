using System;
using System.Collections.Generic;

namespace CompilersCourseWork.Interpreting
{
    public class Interpreter
    {
        private readonly byte[] bytecode;
        private readonly IList<string> strings;
        private int pc;

        private Action<String> printer;


        // could be optimized to not to always use 8 bytes, but simplicity is the goal here
        private readonly Stack<long> stack;
        private readonly long[] variables;

        public Stack<long> Stack
        {
            get
            {
                return stack;
            }
        }

        public int PC
        {
            get
            {
                return pc;
            }
        }

        public long[] Variables
        {
            get
            {
                return variables;
            }
        }

        public Interpreter(byte[] bytecode, IList<string> strings, int variableCnt)
        {
            this.bytecode = bytecode;
            this.strings = strings;
            pc = 0;
            stack = new Stack<long>();
            variables = new long[variableCnt];

            printer = (string str) => Console.Out.WriteLine(str);
        }

        public void SetPrinter(Action<string> printer)
        {
            this.printer = printer;
        }

        public void Run()
        {
            while (pc < bytecode.Length)
            {
                switch (bytecode[pc++])
                {
                    case Bytecode.PUSH_INT:
                        stack.Push(BitConverter.ToInt64(bytecode, pc));
                        pc += 8;
                        break;
                    case Bytecode.PUSH_STRING:
                        stack.Push(BitConverter.ToInt32(bytecode, pc));
                        pc += 4;
                        break;
                    case Bytecode.PUSH_FALSE:
                        stack.Push(0);
                        break;
                    case Bytecode.PUSH_INT_VAR:
                    case Bytecode.PUSH_STRING_VAR:
                    case Bytecode.PUSH_BOOLEAN_VAR:
                        stack.Push(variables[BitConverter.ToInt16(bytecode, pc)]);
                        pc += 4;
                        break;
                    case Bytecode.STORE_VARIABLE:
                        variables[BitConverter.ToInt32(bytecode, pc)] = stack.Pop();
                        pc += 4;
                        break;
                    case Bytecode.IS_LESS_INT:
                    case Bytecode.IS_LESS_BOOLEAN:
                        {
                            var rhs = stack.Pop();
                            var lhs = stack.Pop();
                            stack.Push(lhs < rhs ? 1 : 0);
                        }
                        break;
                    case Bytecode.IS_EQUAL_INT:
                    case Bytecode.IS_EQUAL_BOOLEAN:
                        {
                            var rhs = stack.Pop();
                            var lhs = stack.Pop();
                            stack.Push(lhs == rhs ? 1 : 0);
                        }
                        break;
                    case Bytecode.IS_LESS_STRING:
                        {
                            var rhs = strings[(int)stack.Pop()];
                            var lhs = strings[(int)stack.Pop()];

                            stack.Push(lhs.CompareTo(rhs) == -1 ? 1 : 0);
                        }
                        break;
                    case Bytecode.IS_EQUAL_STRING:
                        {
                            var rhs = strings[(int)stack.Pop()];
                            var lhs = strings[(int)stack.Pop()];

                            stack.Push(lhs == rhs ? 1 : 0);
                        }
                        break;
                        break;
                    case Bytecode.PRINT_INT:
                        printer(stack.Pop().ToString());
                        break;
                    case Bytecode.ADD:
                        {
                            var rhs = stack.Pop();
                            var lhs = stack.Pop();
                            stack.Push(lhs + rhs);
                        }
                        break;
                    case Bytecode.MUL:
                        {
                            var rhs = stack.Pop();
                            var lhs = stack.Pop();
                            stack.Push(lhs * rhs);
                        }
                        break;
                    default:
                        throw new InterpreterError("Invalid bytecode " + bytecode[pc - 1]);
                }
            }
        }
    }
}
