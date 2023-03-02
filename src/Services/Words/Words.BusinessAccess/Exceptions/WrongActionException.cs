﻿namespace Words.BusinessAccess.Exceptions;

public class WrongActionException : Exception
{
    public WrongActionException()
    {
    }

    public WrongActionException(string message) : base(message)
    {
    }
}