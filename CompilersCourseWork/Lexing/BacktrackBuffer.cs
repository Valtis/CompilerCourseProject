﻿using CompilersCourseWork.ErrorHandling;
using CompilersCourseWork.Tokens;
using System.Collections.Generic;

namespace CompilersCourseWork.Lexing
{
    internal class BacktrackBuffer
    {
        private readonly int bufferSize;
        private IList<Token> backtrackBuffer;
        private int backtrackPosition;

        internal BacktrackBuffer(int size)
        {
            backtrackBuffer = new List<Token>();
            bufferSize = size;
            backtrackPosition = -1;
        }

        internal bool Empty()
        {
            return backtrackPosition == -1;
        }

        internal void Backtrack()
        {

            ++backtrackPosition;
            if (backtrackPosition >= bufferSize ||
                backtrackPosition >= backtrackBuffer.Count)
            {
                throw new InternalCompilerErrorException("Attempted to backtrack too many times");
            }
        }

        internal Token GetToken()
        {
            return backtrackBuffer[backtrackPosition--];
        }

        internal Token PeekToken()
        {
            return backtrackBuffer[backtrackPosition];
        }

        internal void AddToken(Token token)
        {
            if (backtrackPosition != -1)
            {
                --backtrackPosition;
                return;
            }

            backtrackBuffer.Insert(0, token);
            while (backtrackBuffer.Count > bufferSize)
            {
                backtrackBuffer.RemoveAt(backtrackBuffer.Count - 1);
            } 
        }

        
    }

}
