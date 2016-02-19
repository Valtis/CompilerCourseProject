using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompilersCourseWork.Tokens
{
    public class SemicolonToken : OperatorToken
    {
        protected override string GetOperator()
        {
            return ";";
        }
    }
}
