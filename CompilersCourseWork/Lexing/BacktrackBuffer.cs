using CompilersCourseWork.ErrorHandling;
using CompilersCourseWork.Tokens;
using System.Collections.Generic;

namespace CompilersCourseWork.Lexing
{
    internal class BacktrackBuffer
    {
        private readonly int buffer_size;
        private IList<Token> backtrack_buffer;
        private int backtrack_position;

        internal BacktrackBuffer(int size)
        {
            backtrack_buffer = new List<Token>();
            buffer_size = size;
            backtrack_position = -1;
        }

        internal bool Empty()
        {
            return backtrack_position == -1;
        }

        internal void Backtrack()
        {

            ++backtrack_position;
            if (backtrack_position >= buffer_size ||
                backtrack_position >= backtrack_buffer.Count)
            {
                throw new InternalCompilerErrorException("Attempted to backtrack too many times");
            }
        }

        internal Token GetToken()
        {
            return backtrack_buffer[backtrack_position--];
        }

        internal Token PeekToken()
        {
            return backtrack_buffer[backtrack_position];
        }

        internal void AddToken(Token token)
        {
            if (backtrack_position != -1)
            {
                --backtrack_position;
                return;
            }

            backtrack_buffer.Insert(0, token);
            while (backtrack_buffer.Count > buffer_size)
            {
                backtrack_buffer.RemoveAt(backtrack_buffer.Count - 1);
            } 
        }

        
    }

}
