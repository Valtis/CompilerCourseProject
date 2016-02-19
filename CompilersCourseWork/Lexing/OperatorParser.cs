using CompilersCourseWork.ErrorHandling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CompilersCourseWork.Tokens;

namespace CompilersCourseWork.Lexing
{
    internal class OperatorParser : TokenParser
    {
        private IDictionary<char, Type> operators;
        private IDictionary<char, IDictionary<char, Type>> two_character_operators; 

        internal OperatorParser(TextReader reader, ErrorReporter reporter) : base(reader, reporter)
        {

            two_character_operators = new Dictionary<char, IDictionary<char, Type>>();
            two_character_operators[':'] = new Dictionary<char, Type>();
            two_character_operators[':']['='] = typeof(AssignmentToken);

            two_character_operators['.'] = new Dictionary<char, Type>();
            two_character_operators['.']['.'] = typeof(RangeToken);


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
        }

        internal override bool Parses(char character)
        {
            return operators.ContainsKey(character) || two_character_operators.ContainsKey(character);
        }

        protected override Token DoParse()
        {
            var character = Reader.NextCharacter().Value;

            if (two_character_operators.ContainsKey(character))
            {
                var next = Reader.PeekCharacter();
                if (next.HasValue)
                {
                    if (two_character_operators[character].ContainsKey(next.Value))
                    {
                        Reader.NextCharacter();
                        return (Token)Activator.
                            CreateInstance(two_character_operators[character][next.Value]);
                    }
                }
            }

            return (Token)Activator.CreateInstance(operators[character]);            
        }
    }
}
