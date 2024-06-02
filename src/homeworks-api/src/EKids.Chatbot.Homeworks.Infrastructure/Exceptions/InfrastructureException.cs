using System;

namespace EKids.Chatbot.Homeworks.Infrastructure.Exceptions;

public class InfrastructureException : Exception
{
    internal InfrastructureException(string businessMessage)
           : base(businessMessage)
    {
    }

    internal InfrastructureException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
