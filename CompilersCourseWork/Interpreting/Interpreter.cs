using System;
using System.Collections.Generic;

namespace CompilersCourseWork.Interpreting
{
    public class Interpreter
    {
        private readonly byte[] bytecode;
        // list of the strings, used by the program
        private readonly IList<string> strings;

        // program counter
        private int pc;

        // printer and reader. By default uses regular stdin and stdout, but tests override these
        private Action<String> printer;
        private Func<String> reader;


        // could be optimized to not to always use 8 bytes, but simplicity is the goal here
        private readonly Stack<long> stack;
        private readonly long[] variables;

        // program lines, used for error messages
        private readonly IList<String> lines;

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

        public IList<string> Strings
        {
            get
            {
                return strings;
            }
        }

        public Interpreter(byte[] bytecode, IList<string> strings, IList<String> lines, int variableCnt)
        {
            this.bytecode = bytecode;
            this.strings = strings;
            this.lines = lines;
            pc = 0;
            stack = new Stack<long>();
            variables = new long[variableCnt];

            printer = (string str) => Console.Out.Write(str);
            reader = () => Console.In.ReadLine();
        }

        public void SetPrinter(Action<string> printer)
        {
            this.printer = printer;
        }

        public void SetReader(Func<string> reader)
        {
            this.reader = reader;
        }

        /*
        Stack machine bytecode interpreter
        */
        public void Run()
        {
            while (pc < bytecode.Length)
            {
                switch (bytecode[pc++])
                {
                    case Bytecode.NOP:
                        // do nothing
                        break;
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
                        stack.Push(variables[BitConverter.ToInt32(bytecode, pc)]);
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
                    case Bytecode.IS_LESS_OR_EQUAL_INT:
                        {
                            var rhs = stack.Pop();
                            var lhs = stack.Pop();
                            stack.Push(lhs <= rhs ? 1 : 0);
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
                            var rhs = Strings[(int)stack.Pop()];
                            var lhs = Strings[(int)stack.Pop()];

                            stack.Push(lhs.CompareTo(rhs) == -1 ? 1 : 0);
                        }
                        break;
                    case Bytecode.IS_EQUAL_STRING:
                        {
                            var rhs = Strings[(int)stack.Pop()];
                            var lhs = Strings[(int)stack.Pop()];

                            stack.Push(lhs == rhs ? 1 : 0);
                        }
                        break;
                    case Bytecode.NOT:
                        {
                            var value = stack.Pop();
                            stack.Push(value == 0 ? 1 : 0);
                        }
                        break;
                    case Bytecode.AND:
                        {
                            var rhs = stack.Pop();
                            var lhs = stack.Pop();
                            stack.Push(rhs == 1 && lhs == 1 ? 1 : 0);
                        }
                        break;
                    case Bytecode.PRINT_INT:
                        printer(stack.Pop().ToString());
                        break;
                    case Bytecode.PRINT_STRING:
                        {
                            var index = (int)stack.Pop();
                            printer(Strings[index]);
                        }
                        break;
                    case Bytecode.READ_INT:
                        {
                            var input = reader().Split(null)[0];
                            var variable = BitConverter.ToInt32(bytecode, pc);
                            pc += 4;
                            long result;
                            if (long.TryParse(input, out result))
                            {
                                variables[variable] = result;
                            }
                            else
                            {
                                throw new InvalidInputException(
                                    "Input must contain only numbers when reading to integer variable");
                            }
                        }
                        break;
                    case Bytecode.READ_STRING:
                        {
                            var input = reader().Split(null)[0];
                            var variable = BitConverter.ToInt32(bytecode, pc);
                            pc += 4;
                            var index = strings.IndexOf(input);
                            if (index == -1)
                            {
                                variables[variable] = strings.Count;
                                strings.Add(input);
                            }
                            else
                            {
                                variables[variable] = index;
                            }

                        }
                        break;
                    case Bytecode.ADD:
                        {
                            var rhs = stack.Pop();
                            var lhs = stack.Pop();
                            stack.Push(lhs + rhs);
                        }
                        break;
                    case Bytecode.SUB:
                        {
                            var rhs = stack.Pop();
                            var lhs = stack.Pop();
                            stack.Push(lhs - rhs);
                        }
                        break;
                    case Bytecode.MUL:
                        {
                            var rhs = stack.Pop();
                            var lhs = stack.Pop();
                            stack.Push(lhs * rhs);
                        }
                        break;
                    case Bytecode.DIV:
                        {
                            var rhs = stack.Pop();
                            var lhs = stack.Pop();
                            stack.Push(lhs / rhs);
                        }
                        break;
                    case Bytecode.CONCAT:
                        {
                            var rhs = (int)stack.Pop();
                            var lhs = (int)stack.Pop();
                            var result = strings[lhs] + strings[rhs];
                            var index = strings.IndexOf(result);
                            if (index == -1)
                            {
                                stack.Push(strings.Count);
                                strings.Add(result);
                            }
                            else
                            {
                                stack.Push(index);
                            }
                        }
                        break;
                    case Bytecode.JUMP_IF_TRUE:
                        {
                            var destination = BitConverter.ToInt32(bytecode, pc);
                            pc += 4;
                            if (stack.Pop() == 1) 
                            {
                                pc = destination;
                            }
                        }
                        break;
                    case Bytecode.ASSERT:
                        if (stack.Pop() == 0)
                        {
                            var line = BitConverter.ToInt32(bytecode, pc);
                            Console.ForegroundColor = ConsoleColor.Red;
                            printer("Assert failed at line " + (line + 1));
                            Console.ForegroundColor = ConsoleColor.Cyan;
                            printer("\n\t" + lines[line]);
                            Console.ResetColor();
                        }
                        pc += 4;
                        break;                    
                    default:
                        throw new InterpreterError("Invalid bytecode " + bytecode[pc - 1]);
                }
            }
        }
    }
}
