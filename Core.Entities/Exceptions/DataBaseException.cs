using System;
using System.Collections.Generic;
using System.Text;

namespace PetShop.Core.Entities.Exceptions
{
    public class DataBaseException : Exception
    {
        public DataBaseException()
        {
        }
        
        public DataBaseException(string? message) : base(message)
        {
        }

        public DataBaseException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
