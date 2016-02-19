﻿using System;
using CompilersCourseWork.Tokens;
using CompilersCourseWork.ErrorHandling;

namespace CompilersCourseWork.Lexing
{
    class CommentParser : TokenParser
    {
        private bool isMultilineComment;

        internal CommentParser(TextReader reader, ErrorReporter reporter) : base(reader, reporter)
        {
            isMultilineComment = false;
        }

        internal override bool Parses(char character)
        {
            if (character == '/')
            {
                Reader.NextCharacter();

                var next = Reader.PeekCharacter();
                if (next.HasValue)
                {
                    if (next.Value == '/')
                    {
                        isMultilineComment = false;
                        return true;
                    }
                    else if (next.Value == '*')
                    {
                        isMultilineComment = true;
                        return true;
                    }
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
                                          
                    if (character.HasValue && character.Value == '*')
                    {
                        character = Reader.PeekCharacter();
                        if (character.HasValue && character.Value == '/')
                        {
                            Reader.NextCharacter();
                            break;
                        }                        
                    }
                }
                
            }
            else
            {
                while (Reader.PeekCharacter().HasValue && Reader.PeekCharacter().Value != '\n')
                {
                    Reader.NextCharacter();
                }
            }

            return new CommentToken();
        }
    }
}