using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompilersCourseWork
{
    class InternalCompilerError : SystemException
    {
        public InternalCompilerError(string message) : base(message)
        {
        }
    }
}
