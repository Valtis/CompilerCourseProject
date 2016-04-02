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
        private IDictionary<char, Type> singleCharacterOperators;
        private IDictionary<char, IDictionary<char, Type>> twoCharacterOperators; 

        internal OperatorScanner(TextReader reader, ErrorReporter reporter) : base(reader, reporter)
        {

            twoCharacterOperators = new Dictionary<char, IDictionary<char, Type>>();
            twoCharacterOperators[':'] = new Dictionary<char, Type>();
            twoCharacterOperators[':']['='] = typeof(AssignmentToken);

            twoCharacterOperators['.'] = new Dictionary<char, Type>();
            twoCharacterOperators['.']['.'] = typeof(RangeToken);


            singleCharacterOperators = new Dictionary<char, Type>();

            singleCharacterOperators.Add('+', typeof(PlusToken));
            singleCharacterOperators.Add('-', typeof(MinusToken));
            singleCharacterOperators.Add('*', typeof(MultiplyToken));
            singleCharacterOperators.Add('/', typeof(DivideToken));
            singleCharacterOperators.Add('<', typeof(LessThanToken));
            singleCharacterOperators.Add('=', typeof(ComparisonToken));
            singleCharacterOperators.Add('&', typeof(AndToken));
            singleCharacterOperators.Add('!', typeof(NotToken));
            singleCharacterOperators.Add(';', typeof(SemicolonToken));
            singleCharacterOperators.Add(':', typeof(ColonToken));
            singleCharacterOperators.Add('(', typeof(LParenToken));
            singleCharacterOperators.Add(')', typeof(RParenToken));
        }

        internal override bool Recognizes(char character)
        {
            return singleCharacterOperators.ContainsKey(character) || twoCharacterOperators.ContainsKey(character);
        }

        protected override Token DoScan()
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
                return (Token)Activator.CreateInstance(singleCharacterOperators[character]);
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
