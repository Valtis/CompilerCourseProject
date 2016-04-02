using System.Text;
using CompilersCourseWork.Tokens;
using CompilersCourseWork.ErrorHandling;
using System;

namespace CompilersCourseWork.Lexing
{
    /*
    Scan integers
    */
    internal class IntegerScanner : TokenScanner
    {
        

        internal IntegerScanner(TextReader reader, ErrorReporter reporter) : base(reader, reporter)
        {
        }

        internal override bool Recognizes(char character)
        {
            return char.IsDigit(character);
        }

        protected override Token DoScan()
        {
            var line = Reader.Line;
            var column = Reader.Column;
            var builder = new StringBuilder();

            while (Reader.PeekCharacter().HasValue)
            {
                var character = Reader.PeekCharacter().Value;
                if (char.IsDigit(character))
                {
                    builder.Append(character);
                }
                else
                {
                    // prevent 1234hello from being lexed as integer - 1234 and identifier - hello
                    if (char.IsLetter(character))
                    {

                        Reporter.ReportError(
                            Error.LEXICAL_ERROR,
                            "Invalid character '" + character + "' encountered when parsing a number",
                            Reader.Line,
                            Reader.Column);
                    }

                    break;
                }

                Reader.NextCharacter();
            }
            
            try
            {
                return new NumberToken(long.Parse(builder.ToString()));
            }
            catch (OverflowException e)
            {
                Reporter.ReportError(
                    Error.LEXICAL_ERROR,
                    "Number does not fit 64 bit signed integer",
                    line,
                    column);
                
                return new NumberToken(1);
            }

        }
    }
}
