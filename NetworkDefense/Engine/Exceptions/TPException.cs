using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine.Exceptions
{
    /// <summary>
    /// Engine specific exception class. This is used to more easily detect whether a error is related to 
    /// the engine.
    /// </summary>
    public class TPException : Exception
    {
        public TPException(string msg) : base(msg) { }
    }
}
