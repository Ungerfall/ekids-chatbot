using System;

namespace EKids.Chatbot.Homeworks.Application.Exceptions;

public class ApplicationException : Exception
{
    internal ApplicationException(string businessMessage)
           : base(businessMessage)
    {
    }

    internal ApplicationException(string message, Exception innerException)
        : base(message, innerException)
    {
    }

    private ApplicationException()
    {
    }
}
