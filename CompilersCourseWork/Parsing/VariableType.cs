using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompilersCourseWork.Parsing
{
    public enum VariableType
    {
        NONE,
        STRING,
        INTEGER,
        BOOLEAN,
        ERROR_TYPE,
    }
    /*
    Add Name() method for the VariableType enum
    */
    public static class Extensions
    {
        public static string Name(this VariableType type)
        {
            switch (type)
            {
                case VariableType.STRING:
                    return "string";
                case VariableType.INTEGER:
                    return "integer";
                case VariableType.BOOLEAN:
                    return "boolean";
                default:
                    return "<INVALID_TYPE>";
            }
        }
    }
}



