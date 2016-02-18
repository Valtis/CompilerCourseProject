using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompilersCourseWork.ErrorHandling
{
    public enum Error { LEXICAL_ERROR }

    public class ErrorReporter
    {

        private string[] lines;
        private IList<ErrorData> errors;

        public IList<ErrorData> Errors
        {
            get
            {
                return errors;
            }
        }

        public string[] Lines
        {
            get
            {
                return lines;
            }

            set
            {
                lines = value;
            }
        }

        public ErrorReporter()
        {
            errors = new List<ErrorData>();
        }

        public void ReportError(Error type, string msg, int line, int column)
        {
            Errors.Add(new ErrorData(Lines, type, msg, line, column));
        }   

        public void PrintErrors()
        {
            foreach (var error in Errors)
            {
                error.Print();
            }
        }

    }
}
