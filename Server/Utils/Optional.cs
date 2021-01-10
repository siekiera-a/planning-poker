using System;

namespace Server.Utils
{
    public class Optional<T>
    {
        private readonly T _value;

        public T Value
        {
            get
            {
                if (_value == null)
                {
                    throw new InvalidOperationException("Object is empty!");
                }

                return _value;
            }
        }

        public bool IsPresent { get; }
        public bool IsEmpty => !IsPresent;

        private Optional()
        {
        }

        private Optional(T value)
        {
            _value = value;
            IsPresent = true;
        }

        public static Optional<T> of(T value)
        {
            return new Optional<T>(value);
        }

        public static Optional<T> ofNullable(T value)
        {
            return value != null ? of(value) : Empty();
        }

        public static Optional<T> Empty()
        {
            return new Optional<T>();
        }

    }
}
