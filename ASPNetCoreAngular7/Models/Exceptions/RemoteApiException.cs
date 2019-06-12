using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASPNetCoreAngular7.Models.Exceptions
{
    public class RemoteApiException : Exception
    {
        public RemoteApiException()
        {
        }

        public RemoteApiException(string message) : base(message)
        {
        }

        public RemoteApiException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}
