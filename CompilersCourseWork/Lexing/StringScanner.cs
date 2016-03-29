using System.Text;
using CompilersCourseWork.Tokens;
using CompilersCourseWork.ErrorHandling;

namespace CompilersCourseWork.Lexing
{
    internal class StringScanner : TokenScanner
    {


		internal StringScanner(TextReader reader, ErrorReporter reporter) : base(reader, reporter)
        {
        }

        internal override bool Parses(char character)
        {
            return character == '"';
        }
        
        protected override Token DoParse()
        {
            var builder = new StringBuilder();

			// skip first "
            Reader.NextCharacter();
            while (Reader.PeekCharacter().HasValue)
            {
                var character = Reader.PeekCharacter().Value;
                if (character == '"')
                {
                    Reader.NextCharacter();
                    break;
                }
                else if (character == '\n')
                {
                    Reporter.ReportError(Error.LEXICAL_ERROR,
						"Unmatched '\"'",
						Reader.Line,
						Reader.Column
						);
                    Reader.NextCharacter();
                    break;
                }
				else if (character == '\\')
                {
                    builder.Append(HandleEscapeSequence());
                }
                else
                {
                    builder.Append(character);
                }

                Reader.NextCharacter();
            }

            return new TextToken(builder.ToString());
        }


		private char HandleEscapeSequence()
        {
            Reader.NextCharacter();
			// there should be, at very least, a newline character remaining,
			// so getting the value without a check should not cause issues
            var nextChar = Reader.PeekCharacter().Value;

			if (nextChar == 'n')
            {
                return '\n';
            }
            else if (nextChar == 't')
            {
                return '\t';
            }
            else if (nextChar == 'r')
            {
                return '\r';
            }
            else if (nextChar == '\\')
            {
                return '\\';
            }
            else if (nextChar == '"')
            {
                return '"';
            }
            else
            {
                Reporter.ReportError(Error.LEXICAL_ERROR,
                    "Invalid escape sequence character '" + nextChar + "'",
                    Reader.Line,
                    Reader.Column
                    );

                return nextChar;
            }
        }
    }
}
