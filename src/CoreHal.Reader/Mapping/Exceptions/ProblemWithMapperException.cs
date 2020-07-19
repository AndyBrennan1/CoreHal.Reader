using System;

namespace CoreHal.Reader.Mapping.Exceptions
{
    public class ProblemWithMapperException : Exception
    {
        private const string message = "A mapper was found for the type specified but it has configuration problems that led to an exception being thrown.";
        public ProblemWithMapperException(Exception innerException) 
            : base(message, innerException)
        {
        }
    }
}