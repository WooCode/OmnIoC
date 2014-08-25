using System;
using System.Collections.Generic;

namespace OmnIoc.Portable
{
    /// <summary>
    /// Interface for accessing OmnIoc without generics
    /// </summary>
    internal interface IOmnIoc
    {
        object Get();
        object GetNamed(string name);
        void Set(Type implementationType, IocReuse reuse = IocReuse.Multiple, string name = null);
        IEnumerable<object> All();
    }
}