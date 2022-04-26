﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spoofer.Exceptions
{
    public class FileNotExistException : Exception
    {
        public override string Message => DEFAULT_MESSAGE;
        private const string DEFAULT_MESSAGE = "There is no I/Q sampeling file specefied for this action!!!";
        public FileNotExistException() :  base()
        {
                
        }
        public FileNotExistException(string message) : base(DEFAULT_MESSAGE)
        {

        }

        public FileNotExistException(string message, Exception innerException) : base(DEFAULT_MESSAGE, innerException)
        {
        }

    }
    public class FileAlreadyExistException : Exception
    {
        public override string Message => DEFAULT_MESSAGE;
        private const string DEFAULT_MESSAGE = "There is I/Q sampeling file specefied for this action already!!!";
        public FileAlreadyExistException() : base()
        {

        }
        public FileAlreadyExistException(string message) : base(DEFAULT_MESSAGE)
        {

        }

        public FileAlreadyExistException(string message, Exception innerException) : base(DEFAULT_MESSAGE, innerException)
        {
        }

    }
    public class CoordinateNotExistException : Exception
    {
        public override string Message => DEFAULT_MESSAGE;
        private const string DEFAULT_MESSAGE = "There is no Coordinate specified in the database for this action";
        public CoordinateNotExistException() : base()
        {

        }
        public CoordinateNotExistException(string message) : base(DEFAULT_MESSAGE)
        {
        }

        public CoordinateNotExistException(string message, Exception innerException) : base(DEFAULT_MESSAGE, innerException)
        {
        }
    }
    public class InvalidCoordinateException : Exception
    {

        public InvalidCoordinateException(string message) : base(message)
        {
        }

        public InvalidCoordinateException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
    public class CoordinateExistException : Exception
    {
        public override string Message => DEFAULT_MESSAGE;
        private const string DEFAULT_MESSAGE = "There is marker on this location already...";
        public CoordinateExistException() : base()
        {

        }
        public CoordinateExistException(string message) : base(DEFAULT_MESSAGE)
        {
        }

        public CoordinateExistException(string message, Exception innerException) : base(DEFAULT_MESSAGE, innerException)
        {
        }
    }
}