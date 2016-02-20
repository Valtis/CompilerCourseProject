using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompilersCourseWork.ErrorHandling
{

    public class ErrorData
    {
        private readonly Error type;
        private readonly string msg;
        private readonly int line;
        private readonly int column;
        private string[] lines;

        public Error Type
        {
            get
            {
                return type;
            }
        }

        public int Line
        {
            get
            {
                return line;
            }
        }

        public int Column
        {
            get
            {
                return column;
            }
        }

        public string Message
        {
            get
            {
                return msg;
            }
        }

        public ErrorData(string[] lines, Error type, string msg, int line, int column)
        {
            this.lines = lines;
            this.type = type;
            this.msg = msg;
            this.line = line;
            this.column = column;
        }


        public void Print()
        {
            ConsoleColor color = Console.ForegroundColor;
            switch (Type)
            {
                case Error.NOTE:
                    color = ConsoleColor.Green;
                    Console.ForegroundColor = color;
                    Console.Write("Note: ");
                    break;

                case Error.WARNING:
                    color = ConsoleColor.Yellow;
                    Console.ForegroundColor = color;
                    Console.Write("Warning: ");
                    break;

                case Error.LEXICAL_ERROR:
                    color = ConsoleColor.Red;
                    Console.ForegroundColor = color;
                    Console.Write("Lexical error: ");
                    break;

                case Error.SYNTAX_ERROR:
                    color = ConsoleColor.Red;
                    Console.ForegroundColor = color;
                    Console.Write("Syntax error: ");
                    break;
                default:
                    Console.Write("<INVALID_ERROR_TYPE>: ");
                    break;
            }


            Console.ResetColor();
            // line, column are zero-based internally
            Console.Write(msg + " at line " + (line + 1) + " column " + (column + 1));
            Console.Write("\n\n" + lines[line]);

            Console.ForegroundColor = color;
            if (column > 10)
            {
                for (int i = 0; i < column - 6; ++i)
                {
                    Console.Write(" ");
                }

                for (int i = 0; i < 6; ++i)
                {
                    Console.Write("~");
                }
                Console.Write("^");

            } else
            {
                for (int i = 0; i < column; ++i)
                {
                    Console.Write(" ");
                }

                Console.Write("^");
                
                for (int i = 0; i < 6; ++i)
                {
                    Console.Write("~");
                }
            }
            Console.Write("\n");
            
        }

    }
}
