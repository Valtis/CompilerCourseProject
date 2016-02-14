using System;

namespace CompilersCourseWork.Tokens
{
    public class IdentifierToken : Token
    {
        private string identifier;


        public IdentifierToken(String identifier)
        {
            Identifier = identifier;

        }

        public string Identifier
        {
            get
            {
                return identifier;
            }

            set
            {
                identifier = value;
            }
        }

        public override bool Equals(Object obj)
        {
            if (obj == null)
            {
                return false;
            }

            var asIdentifierToken = obj as IdentifierToken;

            if (asIdentifierToken == null)
            {
                return false;
            }

            return asIdentifierToken.Identifier == Identifier;
        }

        public override int GetHashCode()
        {
            return Identifier.GetHashCode();
        }

        protected override Tuple<String, String> GetStringRepresentation()
        {
            return new Tuple<String, String>("Identifier", Identifier);
        }
    }
}
