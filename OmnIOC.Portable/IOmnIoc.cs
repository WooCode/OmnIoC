using System;
using System.Collections.Generic;

namespace OmnIoC.Portable
{
    /// <summary>
    /// Interface for accessing OmnIoC without generics
    /// </summary>
    internal interface IOmnIoC
    {
        object Get();
        object GetNamed(string name);
        void Set(Type implementationType, Reuse reuse = Reuse.Multiple, string name = null);
        IEnumerable<object> All();
    }
}