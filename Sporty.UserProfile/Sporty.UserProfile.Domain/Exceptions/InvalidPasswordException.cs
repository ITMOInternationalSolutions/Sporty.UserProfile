﻿namespace Sporty.UserProfile.Domain.Exceptions;

public class InvalidPasswordException : Exception
{
    public InvalidPasswordException()
    {
    }
    
    public InvalidPasswordException(string message) 
        : base(message)
    {
    }

    public InvalidPasswordException(string message, Exception e)
        : base(message, e)
    {
    }
}