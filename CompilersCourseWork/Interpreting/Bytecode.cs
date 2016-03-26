using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompilersCourseWork.Interpreting
{
    public class Bytecode
    {
        public const byte NOP = 0;

        public const byte PUSH_INT = 1;
        public const byte PUSH_STRING = 2;
        public const byte PUSH_FALSE = 3;
        public const byte PUSH_INT_VAR = 4;
        public const byte PUSH_STRING_VAR = 5;
        public const byte PUSH_BOOLEAN_VAR = 6;

        public const byte STORE_VARIABLE = 40;

        public const byte IS_LESS_INT = 60;
        public const byte IS_LESS_OR_EQUAL_INT = 61;
        public const byte IS_EQUAL_INT = 62;
        public const byte IS_LESS_STRING = 63;
        public const byte IS_EQUAL_STRING = 64;
        public const byte IS_LESS_BOOLEAN = 65;
        public const byte IS_EQUAL_BOOLEAN = 66;
        public const byte NOT = 67;
        public const byte AND = 68;

        public const byte PRINT_INT = 70;
        public const byte PRINT_STRING = 71;
        public const byte READ_INT = 72;
        public const byte READ_STRING = 73;

        public const byte ADD = 80;
        public const byte SUB = 81;
        public const byte MUL = 82;
        public const byte DIV = 83;

        public const byte CONCAT = 90;

        public const byte JUMP_IF_TRUE = 100;

        public const byte ASSERT = 110;
    }
}
