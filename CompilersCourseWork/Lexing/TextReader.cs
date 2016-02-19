using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompilersCourseWork.Lexing
{
    internal class TextReader
    {
        // reading and keeping all lines in memory makes implementation easier, but can be expensive
        // memory wise. This is an intentional tradeoff. 
        private string[] lines;

        private int line;

        // column is meant for human-readable notifications, where tab character gets converted
        // into a given number of spaces. array_pos tracks the position at given line, used internally
        private int column;
        private int array_pos;
        
        private int spaces_per_tab;

        public int Line
        {
            get
            {
                return line;
            }

            private set
            {
                line = value;
            }
        }

        public int Column
        {
            get
            {
                return column;
            }

            private set
            {
                column = value;
            }
        }

        internal void Backtrack()
        {
            if (array_pos != 0)
            {
                --array_pos;
            }
            else if (Line != 0)
            {
                --Line;
                array_pos = lines[Line].Length - 1;
            }
        }

        public string[] Lines
        {
            get
            {
                return lines;
            }

        }

        internal TextReader(string path, int spaces_per_tab)
        {
            Line = 0;
            Column = 0;
            array_pos = 0;
            this.spaces_per_tab = spaces_per_tab;

            lines = System.IO.File.ReadAllLines(path);

            // append newlines to every line, as ReadAllLines strips them but parsers
            // assume they are present
            for (int i = 0; i < Lines.Length; ++i)
            {
                Lines[i] += "\n";
            }
        }

        internal char? NextCharacter()
        {

            var value = PeekCharacter();

            ++array_pos;

            if (value.HasValue && value.Value == '\t')
            {
                Column += spaces_per_tab;
            }
            else
            {
                Column += 1;
            }

            if (array_pos >= Lines[Line].Length)
            {
                array_pos = 0;
                Column = 0;
                ++Line;
                if (Line >= Lines.Length)
                {
                    Line = Lines.Length;
                }
            }

            return value;
        }

        internal char? PeekCharacter()
        {
            if (Line == Lines.Length)
            {
                return null;
            }
            else
            {
                return Lines[Line][array_pos];
            }
        }
    }
}
