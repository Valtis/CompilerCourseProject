using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompilersCourseWork.Interpreter
{
    public class Bytecode
    {
        public static readonly byte NOP = 0;

        public static readonly byte PUSH_INTEGER = 1;
        public static readonly byte PUSH_STRING = 2;
        public static readonly byte PUSH_FALSE = 3;
        public static readonly byte PUSH_INT_VAR = 4;
        public static readonly byte PUSH_STRING_VAR = 5;
        public static readonly byte PUSH_BOOLEAN_VAR = 6;

        public static readonly byte STORE_VARIABLE = 40;

        public static readonly byte IS_LESS_INT = 60;
        public static readonly byte IS_EQUAL_INT = 61;
        public static readonly byte IS_LESS_STRING = 62;
        public static readonly byte IS_EQUAL_STRING = 63;
        public static readonly byte IS_LESS_BOOLEAN = 64;
        public static readonly byte IS_EQUAL_BOOLEAN = 65;
        public static readonly byte NOT = 66;
        public static readonly byte AND = 67;

        public static readonly byte PRINT_INT = 70;
        public static readonly byte PRINT_STRING = 71;

        public static readonly byte ADD = 80;
        public static readonly byte SUB = 81;
        public static readonly byte MUL = 82;
        public static readonly byte DIV = 83;

        public static readonly byte CONCAT = 90;
    }
}
