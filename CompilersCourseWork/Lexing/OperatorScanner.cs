using CompilersCourseWork.ErrorHandling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CompilersCourseWork.Tokens;

namespace CompilersCourseWork.Lexing
{
    internal class OperatorScanner : TokenScanner
    {
        private IDictionary<char, Type> operators;
        private IDictionary<char, IDictionary<char, Type>> twoCharacterOperators; 

        internal OperatorScanner(TextReader reader, ErrorReporter reporter) : base(reader, reporter)
        {

            twoCharacterOperators = new Dictionary<char, IDictionary<char, Type>>();
            twoCharacterOperators[':'] = new Dictionary<char, Type>();
            twoCharacterOperators[':']['='] = typeof(AssignmentToken);

            twoCharacterOperators['.'] = new Dictionary<char, Type>();
            twoCharacterOperators['.']['.'] = typeof(RangeToken);


            operators = new Dictionary<char, Type>();

            operators.Add('+', typeof(PlusToken));
            operators.Add('-', typeof(MinusToken));
            operators.Add('*', typeof(MultiplyToken));
            operators.Add('/', typeof(DivideToken));
            operators.Add('<', typeof(LessThanToken));
            operators.Add('=', typeof(ComparisonToken));
            operators.Add('&', typeof(AndToken));
            operators.Add('!', typeof(NotToken));
            operators.Add(';', typeof(SemicolonToken));
            operators.Add(':', typeof(ColonToken));
            operators.Add('(', typeof(LParenToken));
            operators.Add(')', typeof(RParenToken));
        }

        internal override bool Parses(char character)
        {
            return operators.ContainsKey(character) || twoCharacterOperators.ContainsKey(character);
        }

        protected override Token DoParse()
        {
            var line = Reader.Line;
            var column = Reader.Column;
            var character = Reader.NextCharacter().Value;

            // if we can match a two character operator, do so. Otherwise match single character operator
            if (twoCharacterOperators.ContainsKey(character))
            {
                var next = Reader.PeekCharacter();
                if (next.HasValue)
                {
                    if (twoCharacterOperators[character].ContainsKey(next.Value))
                    {
                        Reader.NextCharacter();
                        return (Token)Activator.
                            CreateInstance(twoCharacterOperators[character][next.Value]);
                    }
                }
            }

            try
            {
                return (Token)Activator.CreateInstance(operators[character]);
            }
            catch (KeyNotFoundException e)
            {
                // single '.' leads here
                Reporter.ReportError(
                    Error.LEXICAL_ERROR,
                    "Invalid operator " + "'" + character + "'",
                    line,
                    column);

                // pretend we read whitespace to force lexer to get next token
                return new WhitespaceToken();
            }            
        }
    }
}
