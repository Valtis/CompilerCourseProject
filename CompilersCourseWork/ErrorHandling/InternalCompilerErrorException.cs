using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompilersCourseWork.ErrorHandling
{
    public class InternalCompilerErrorException : Exception
    {
        public InternalCompilerErrorException(string msg) : base(msg)
        {
            
        }
    }
}
