using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace OmnIoC.Portable
{
    /// <summary>
    ///     Generic container that holds registrations for type(s)
    /// </summary>
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    [SuppressMessage("ReSharper", "StaticMemberInGenericType")]
    public sealed class OmnIoCContainer<RegistrationType> : IOmnIoCContainer
    {
        #region IOmnIoCContainer members

        object IOmnIoCContainer.Get()
        {
            return Get();
        }

        object IOmnIoCContainer.GetNamed(string name)
        {
            return GetNamed(name);
        }

        /// <summary>
        ///     Set implementation type for <see cref="RegistrationType" /> with name
        /// </summary>
        /// <param name="implementationType"></param>
        /// <param name="reuse"></param>
        /// <param name="name"></param>
        void IOmnIoCContainer.Set(Type implementationType, Reuse reuse, string name)
        {
            if (!_registrationType.IsAssignableFrom(implementationType))
                throw new Exception(string.Format("{0} is not assignable from {1}", _registrationType.Name, implementationType.Name));

            switch (reuse)
            {
                case Reuse.Multiple:
                    Set(() => (RegistrationType) Activator.CreateInstance(implementationType), name);
                    break;
                case Reuse.Singleton:
                    var instance = (RegistrationType) Activator.CreateInstance(implementationType);
                    Set(() => instance, name);
                    break;
                default:
                    throw new ArgumentOutOfRangeException("reuse");
            }
        }

        IEnumerable<object> IOmnIoCContainer.All()
        {
            return _namedCollection.Values.Select(f => (object) f());
        }

        #endregion

        private static Dictionary<string, Func<RegistrationType>> _namedCollection = new Dictionary<string, Func<RegistrationType>>(StringComparer.OrdinalIgnoreCase);
        private static Func<RegistrationType> _defaultFactory = () => default(RegistrationType);
        private static readonly object _syncLock = new object();
        private static readonly Type _registrationType = typeof (RegistrationType);


        /// <summary>
        ///     The main registration (unamed registration) or default of (<see cref="RegistrationType" />)
        /// </summary>
        public static Func<RegistrationType> Get = () => _defaultFactory.Invoke();

        static OmnIoCContainer()
        {
            OmnIoCContainer.ClearAll += Clear;
            SetDefaults();
        }

        public static Func<RegistrationType> DefaultValueFactory
        {
            get { return _defaultFactory; }
            set { _defaultFactory = value; }
        }

        /// <summary>
        ///     Get all registered instances for <see cref="RegistrationType" />
        /// </summary>
        public static IEnumerable<RegistrationType> All
        {
            get { return _namedCollection.Values.Select(f => f()); }
        }

        /// <summary>
        ///     Get all names that are registered for <see cref="RegistrationType" />
        /// </summary>
        public static IEnumerable<string> Names
        {
            get { return _namedCollection.Keys; }
        }

        private static void SetDefaults()
        {
            _namedCollection = new Dictionary<string, Func<RegistrationType>>(StringComparer.OrdinalIgnoreCase);
            Get = () => _defaultFactory.Invoke();
            _defaultFactory = () => default(RegistrationType);
        }

        public static void Remove(string name)
        {
            lock(_syncLock)
            {
                if (name != null && _namedCollection.ContainsKey(name))
                {
                    var newCollection = new Dictionary<string, Func<RegistrationType>>(_namedCollection, StringComparer.OrdinalIgnoreCase);
                    newCollection.Remove(name);
                    _namedCollection = newCollection;
                }
            }
        }

        public static void Clear()
        {
            lock(_syncLock)
                SetDefaults();
        }

        /// <summary>
        ///     Get named instance for <see cref="RegistrationType" />
        /// </summary>
        public static RegistrationType GetNamed(string name)
        {
            var collection = _namedCollection;
            return collection.ContainsKey(name) ? collection[name].Invoke() : _defaultFactory.Invoke();
        }

        /// <summary>
        ///     Register factory/constructor for <see cref="RegistrationType" />
        /// </summary>
        public static void Set(Func<RegistrationType> factory, string name = null)
        {
            if (name == null)
            {
                Get = factory;
                return;
            }

            lock(_syncLock)
            {
                // Safely copy the old dictionary to new one and add the factory
                var newCollection = new Dictionary<string, Func<RegistrationType>>(_namedCollection, StringComparer.OrdinalIgnoreCase);
                newCollection[name] = factory;
                _namedCollection = newCollection;
            }
        }

        /// <summary>
        ///     Register instance/singleton
        /// </summary>
        public static void Set(RegistrationType instance, string name = null)
        {
            Set(() => instance, name);
        }

        /// <summary>
        ///     Register instance
        /// </summary>
        public static void Set<ImplementationType>(Reuse reuse = Reuse.Multiple, string name = null) where ImplementationType : RegistrationType, new()
        {
            if (reuse == Reuse.Singleton)
                Set(new ImplementationType(), name);
            else
                Set(() => new ImplementationType(), name);
        }
    }
}