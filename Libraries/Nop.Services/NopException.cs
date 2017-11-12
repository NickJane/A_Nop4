using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Services
{
      [Serializable]
    public class WezhanException : Exception
    {
        /// <summary>
        /// Creates a new <see cref="WezhanException"/> object.
        /// </summary>
        public WezhanException()
        {

        }

        /// <summary>
        /// Creates a new <see cref="WezhanException"/> object.
        /// </summary>
        public WezhanException(SerializationInfo serializationInfo, StreamingContext context)
            : base(serializationInfo, context)
        {

        }

        /// <summary>
        /// Creates a new <see cref="WezhanException"/> object.
        /// </summary>
        /// <param name="message">Exception message</param>
        public WezhanException(string message)
            : base(message)
        {

        }

        /// <summary>
        /// Creates a new <see cref="WezhanException"/> object.
        /// </summary>
        /// <param name="message">Exception message</param>
        /// <param name="innerException">Inner exception</param>
        public WezhanException(string message, Exception innerException)
            : base(message, innerException)
        {

        }
    }
}
