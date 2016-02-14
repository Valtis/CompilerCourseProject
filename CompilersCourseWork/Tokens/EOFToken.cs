using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompilersCourseWork.Tokens
{
    public class EOFToken : Token
    {
        protected override Tuple<string, string> GetStringRepresentation()
        {
            return new Tuple<string, string>("EOF", "EOF");
        }

        public override bool Equals(object obj)
        {
            return obj != null && obj.GetType() == this.GetType();
        }
    }
}
