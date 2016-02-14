using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompilersCourseWork.Tokens
{
    public class ForToken : Token
    {
        protected override Tuple<string, string> GetStringRepresentation()
        {
            return new Tuple<String, String>("Keyword", "for");
        }


        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            return obj is ForToken;
        }

        public override int GetHashCode()
        {
            // all for-tokens are considered to be equal (line\column are ignored), 
            // so return a static hashcode. In general, these should not be used as
            // keys for hashsets\dictionaries etc.
            return 0;
        }
    }
}
