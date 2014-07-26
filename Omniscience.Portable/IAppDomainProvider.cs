using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Omniscience.Portable
{
    public interface IAppDomainTypeProvider
    {
        IEnumerable<Type> GetTypes();
    }
}
