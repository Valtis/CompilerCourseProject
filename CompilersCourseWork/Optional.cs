using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompilersCourseWork
{
    public struct Optional<T>
    {
        private static readonly Optional<T> none = new Optional<T>(default(T), false);
        
        private readonly bool hasValue;
        private readonly T value;

        public static Optional<T> None
        {
            get
            {
                return none;
            }
        }

        public T Value
        {
            get
            {
                if (!HasValue)
                {
                    throw new InvalidOperationException("Contract violation: " +
                        "Attempted to get value from None");
                }

                return value;
            }
        }

        public bool HasValue
        {
            get
            {
                return hasValue;
            }
        }

        // Private constructor for this class. Used by Some and None
        private Optional(T value, bool hasValue) 
        {
            this.hasValue = hasValue;
            this.value = value;            
        }

        public static Optional<T> Some(T value)
        {
            if (value == null)
            {
                throw new InvalidOperationException("Contract violation: " +
                    "Object cannot be null for Some");
            }

            return new Optional<T>(value, true);
        }
    }
}
