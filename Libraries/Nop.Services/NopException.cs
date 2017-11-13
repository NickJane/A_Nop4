using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Services
{
      [Serializable]
    public class NopException : Exception
    {
        /// <summary>
        /// Creates a new <see cref="NopException"/> object.
        /// </summary>
        public NopException()
        {

        }

        /// <summary>
        /// Creates a new <see cref="NopException"/> object.
        /// </summary>
        public NopException(SerializationInfo serializationInfo, StreamingContext context)
            : base(serializationInfo, context)
        {

        }

        /// <summary>
        /// Creates a new <see cref="NopException"/> object.
        /// </summary>
        /// <param name="message">Exception message</param>
        public NopException(string message)
            : base(message)
        {

        }

        /// <summary>
        /// Creates a new <see cref="NopException"/> object.
        /// </summary>
        /// <param name="message">Exception message</param>
        /// <param name="innerException">Inner exception</param>
        public NopException(string message, Exception innerException)
            : base(message, innerException)
        {

        }
    }
}
