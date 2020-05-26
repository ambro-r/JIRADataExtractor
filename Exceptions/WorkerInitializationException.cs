using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace JIRADataExtractor.Exceptions
{
    class WorkerInitializationException : Exception
    {
        public WorkerInitializationException() : base(String.Format("Unable to initialize worker {0}", GetThrowingClass()))
        {
        }

        public WorkerInitializationException(string message) : base(String.Format("Unable to initialize worker {0}: {1}", GetThrowingClass(), message))
        {
        }

        private static string GetThrowingClass()
        {
            System.Diagnostics.StackTrace stackTrace = new System.Diagnostics.StackTrace();
            return stackTrace.GetFrame(2).GetMethod().ReflectedType.Name;
        }

    }
}
