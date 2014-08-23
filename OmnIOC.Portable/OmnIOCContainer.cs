using System;
using System.Collections.Generic;
using System.Linq;

namespace OmnIOC.Portable
{
    /// <summary>
    ///     Simple object factory / DiContainer
    ///     Implement custom methods in a partial file since any changes will be lost on update.
    /// </summary>
    public static class OmnIoc
    {
        /// <summary>
        ///     Resolve all names that <see cref="T" /> is registered under
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IEnumerable<T> GetAll<T>()
        {
            return TypeContainer<T>.Container.Values.Select(f => f()).ToList();
        }

        internal static event EventHandler ClearAll = (sender, args) => { };

        /// <summary>
        ///     Clear all registrations
        /// </summary>
        public static void Clear()
        {
            var handler = ClearAll;
            if (handler != null) handler(null, EventArgs.Empty);
        }

        /// <summary>
        ///     Register factory for <see cref="T" />
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="factory"></param>
        /// <param name="name"></param>
        public static void Set<T>(Func<T> factory, string name = null)
        {
            if (name == null)
            {
                TypeContainer<T>.Main = factory;
                TypeContainer<T>.Container[string.Empty] = factory;
            }
            else
                TypeContainer<T>.Container[name] = factory;
        }

        /// <summary>
        ///     Register instance
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="instance"></param>
        /// <param name="name"></param>
        public static void Set<T>(T instance, string name = null)
        {
            Set(() => instance, name);
        }

        /// <summary>
        ///     Register instance
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TR"></typeparam>
        /// <param name="name"></param>
        /// <param name="singleton"></param>
        public static void Set<T, TR>(string name = null, bool singleton = false) where TR : T, new()
        {
            if (singleton)
                Set<T>(new TR());
            else
                Set<T>(() => new TR(), name);

        }

        /// <summary>
        ///     Resolve named implementation of <see cref="T" />
        /// </summary>
        /// <typeparam name="T">Type to be resolved</typeparam>
        /// <param name="name"></param>
        /// <returns></returns>
        public static T Get<T>(string name)
        {
            return TypeContainer<T>.Container[name]();
        }
        /// <summary>
        ///     Resolve implementation of <see cref="T" />
        /// </summary>
        /// <typeparam name="T">Type to be resolved</typeparam>
        /// <returns></returns>
        public static T Get<T>()
        {
            return TypeContainer<T>.Main();
        }
            
        /// <summary>
        ///     Resolve all names that <see cref="T" /> is registered under
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static Dictionary<string, Func<T>> GetAllTuples<T>()
        {
            return TypeContainer<T>.Container.ToDictionary(t => t.Key, t => t.Value, StringComparer.OrdinalIgnoreCase);
        }

        private static class TypeContainer<T>
        {
            public static readonly Dictionary<string, Func<T>> Container = new Dictionary<string, Func<T>>(StringComparer.OrdinalIgnoreCase);
            /// <summary>
            ///     The main registration (unamed registration or first named registration)
            /// </summary>
            public static Func<T> Main = () => Container.Count > 0 ? Container.First().Value() : default(T);
            
            static TypeContainer()
            {
                ClearAll += (sender, args) => Container.Clear();
            }
        }
    }
}