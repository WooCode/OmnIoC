using System;
using System.Collections.Generic;

namespace OmnIoC.Portable
{
    /// <summary>
    /// Interface for accessing OmnIoC without generics
    /// </summary>
    internal interface IOmnIoCContainer
    {
        object Get();
        object GetNamed(string name);
        void Set(Type implementationType, Reuse reuse = Reuse.Multiple, string name = null);
        IEnumerable<object> All();
    }
}