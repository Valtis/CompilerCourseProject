using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CompilersCourseWork.Tokens;

namespace CompilersCourseWork.Lexing
{
    internal class IdentifierAndKeywordParser : Parser
    {

        private ISet<string> keywords;

        internal IdentifierAndKeywordParser(TextReader reader) : base(reader)
        {
            keywords = new HashSet<string>();


        }

        internal override bool Parses(char character)
        {
            return char.IsLetter(character);
        }

        protected override Optional<Token> DoParse()
        {
            var builder = new StringBuilder();
            
            while (Reader.PeekCharacter().HasValue && 
                (char.IsLetterOrDigit(Reader.PeekCharacter().Value) || 
                Reader.PeekCharacter().Value == '_'))
            {
                builder.Append(Reader.PeekCharacter().Value);
                Reader.NextCharacter();
            }
            
            return Optional<Token>.Some(new IdentifierToken(builder.ToString()));
        }


    }
}
