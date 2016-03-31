using System;

namespace CompilersCourseWork
{
    public class InternalCompilerError : SystemException
    {
        public InternalCompilerError(string message) : base(message)
        {
        }
    }
}
