using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CompilersCourseWork.Tokens;
using CompilersCourseWork.ErrorHandling;

namespace CompilersCourseWork.Lexing
{
    internal class IdentifierAndKeywordParser : TokenParser
    {

        private IDictionary<string, Type> keywords;

        internal IdentifierAndKeywordParser(TextReader reader, ErrorReporter reporter) : base(reader, reporter)
        {
            keywords = new Dictionary<string, Type>();

            keywords.Add("var", typeof(VarToken));
            keywords.Add("for", typeof(ForToken));
            keywords.Add("end", typeof(EndToken));
            keywords.Add("in", typeof(InToken));
            keywords.Add("do", typeof(DoToken));
            keywords.Add("read", typeof(ReadToken));
            keywords.Add("print", typeof(PrintToken));
            keywords.Add("int", typeof(IntToken));
            keywords.Add("string", typeof(StringToken));
            keywords.Add("bool", typeof(BoolToken));
            keywords.Add("assert", typeof(AssertToken));
        }

        internal override bool Parses(char character)
        {
            return char.IsLetter(character);
        }

        protected override Token DoParse()
        {
            var builder = new StringBuilder();
            
            while (Reader.PeekCharacter().HasValue && 
                (char.IsLetterOrDigit(Reader.PeekCharacter().Value) || 
                Reader.PeekCharacter().Value == '_'))
            {
                builder.Append(Reader.PeekCharacter().Value);
                Reader.NextCharacter();
            }

            var text = builder.ToString();
            if (keywords.ContainsKey(text))
            {
                return (Token)Activator.CreateInstance(keywords[text]);
            }

            return new IdentifierToken(text);
        }


    }
}
