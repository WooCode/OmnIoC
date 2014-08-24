using System;

namespace OmnIOC.Portable
{
    internal interface IOmnIoc
    {
        object GetTypeInstance(string name = null);
        void SetTypeInstance(Type implementationType, OmnIoc.Reuse reuse = OmnIoc.Reuse.Multiple, string name = null);
    }
}