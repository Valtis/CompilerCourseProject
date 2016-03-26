using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompilersCourseWork.Interpreting
{
    public class InvalidInputException : Exception
    {
        public InvalidInputException(string msg) : base(msg)
        {

        }
    }
}
