using System;
using CompilersCourseWork.Tokens;
using CompilersCourseWork.ErrorHandling;

namespace CompilersCourseWork.Lexing
{
    /*
    Scans and removes comments
    */
    class CommentScanner : TokenScanner
    {
        private bool isMultilineComment;
        private int nesting;

        internal CommentScanner(TextReader reader, ErrorReporter reporter) : base(reader, reporter)
        {
            isMultilineComment = false;
        }

        internal override bool Parses(char character)
        {
            // reset nesting value
            nesting = 0;
            if (character == '/')
            {
                Reader.NextCharacter();

                var next = Reader.PeekCharacter();
                if (next.HasValue)
                {
                    // single line comment
                    if (next.Value == '/')
                    {
                        isMultilineComment = false;
                        return true;
                    }
                    // multiline comment
                    else if (next.Value == '*')
                    {
                        isMultilineComment = true;
                        return true;
                    }
                    // was something else; likely divide-symbol
                    else
                    {
                        Reader.Backtrack();
                    }
                }
            }

            return false;
        }

        protected override Token DoParse()
        {
            if (isMultilineComment)
            {
                char? character = ' ';
                while (character.HasValue)
                {                    
                    character = Reader.NextCharacter();
                                          
                    if (character.HasValue && character.Value == '/')
                    {
                        // if we see a '/*', increase nesting
                        character = Reader.NextCharacter();
                        if (character.Value == '*')
                        {
                            ++nesting;
                            character = Reader.NextCharacter();
                        }
                    }

                    if (character.HasValue && character.Value == '*')
                    {
                        character = Reader.PeekCharacter();
                        if (character.HasValue && character.Value == '/')
                        {
                            Reader.NextCharacter();
                            --nesting;
                            // only break out if every '/*' has a matching '*/'
                            if (nesting < 0)
                            {
                                break;
                            }
                        }                        
                    }
                }
                
            }
            else
            {
                // single line comment - read and discard characters until the end of line
                while (Reader.PeekCharacter().HasValue && Reader.PeekCharacter().Value != '\n')
                {
                    Reader.NextCharacter();
                }
            }

            // Comment token could include the comment string, if needed.
            return new CommentToken();
        }
    }
}
