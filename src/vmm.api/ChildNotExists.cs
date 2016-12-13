using System;
using System.Runtime.Serialization;

namespace vmm.api.Services
{
    [Serializable]
    internal class ChildNotExists : Exception
    {
        public ChildNotExists()
        {
        }

        public ChildNotExists(string message) : base(message)
        {
        }

        public ChildNotExists(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ChildNotExists(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}