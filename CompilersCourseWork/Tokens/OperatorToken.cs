using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompilersCourseWork.Tokens
{
    public abstract class OperatorToken : Token
    {

        protected override Tuple<string, string> GetStringRepresentation()
        {
            return new Tuple<String, String>("Operator", GetOperator());
        }

        protected abstract string GetOperator();

        // consider tokens to be equal if the types are identical
        // e.g. two for tokens are always considered to be equals
        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            return obj.GetType().Equals(this.GetType());
        }

        public override int GetHashCode()
        {
            // all keyword-tokens are considered to be equal (line\column are ignored), 
            // so return a static hashcode. In general, these should not be used as
            // keys for hashsets\dictionaries etc.
            return 0;
        }
    }
}
