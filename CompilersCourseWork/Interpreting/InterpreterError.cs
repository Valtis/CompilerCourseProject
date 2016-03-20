using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompilersCourseWork.Interpreting
{
    class InterpreterError : Exception
    {
        public InterpreterError(string err) : base(err)
        {

        }
    }
}
