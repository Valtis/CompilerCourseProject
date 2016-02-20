using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompilersCourseWork.ErrorHandling
{
    public enum Error { NOTE, WARNING, LEXICAL_ERROR, SYNTAX_ERROR }

    public class ErrorReporter
    {

        private string[] lines;
        private IList<ErrorData> errors;

        public IList<ErrorData> Errors
        {
            get
            {
                var filtered_list = from e in errors
                                    where (e.Type != Error.NOTE && e.Type != Error.WARNING)
                                    select e;
                return new List<ErrorData>(filtered_list);
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
            errors.Add(new ErrorData(Lines, type, msg, line, column));
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
