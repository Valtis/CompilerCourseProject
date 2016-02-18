
using System;

namespace CompilersCourseWork.Tokens
{
    public class NumberToken : Token
    {
        private readonly long value;

        public NumberToken(long value)
        {
            this.value = value;
        }

        public long Value
        {
            get
            {
                return value;
            }
        }

        protected override Tuple<string, string> GetStringRepresentation()
        {
            return new Tuple<string, string>("number", "" + value);
        }


        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            var asToken = obj as NumberToken;
            
            if (asToken == null || asToken.Value != Value)
            {
                return false;
            }

            return true;
        }

        public override int GetHashCode()
        {
            return (int)(Value % int.MaxValue);
        }
    }
}
