

using CompilersCourseWork.CodeGeneration;
using CompilersCourseWork.ErrorHandling;
using CompilersCourseWork.Interpreting;
using CompilersCourseWork.Lexing;
using CompilersCourseWork.Parsing;
using CompilersCourseWork.SemanticChecking;
using System;

namespace CompilersCourseWork
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                Console.WriteLine("You need to give path to Mini-PL file as command line argument");
                return;
            }

            try
            {
                var reporter = new ErrorReporter();
                 var lexer = new Lexer(
                    args[0],
                    reporter);
                var parser = new Parser(lexer, reporter);

                var nodes = parser.Parse();

                var semChecker = new SemanticChecker(reporter);
                nodes.Accept(semChecker);
                reporter.PrintMessages();

                if (reporter.Errors.Count != 0)
                {
                    return;
                }


                var generator = new CodeGenerator(semChecker.SymbolTable, semChecker.Variables);
                nodes.Accept(generator);

                var interpreter = new Interpreter(generator.Bytecodes, generator.Strings, lexer.Lines, generator.Variables);
                interpreter.Run();
            }
            catch (InternalCompilerError e)
            {
                PrintError("Internal compiler error", e);
            }
            catch (InterpreterError e)
            {
                PrintError("Interpreter error", e);
            }
            catch (InvalidInputException e)
            {
                PrintError("Invalid input", e);
            }
            catch (DivideByZeroException e)
            {
                PrintError("Division by zero", e);
            }
            catch (System.IO.FileNotFoundException e)
            {
                PrintError("Invalid input file", e);
            }
        }

        private static void PrintError(string info, Exception e)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write(info + ": ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(e.Message);
        }
    }
}
