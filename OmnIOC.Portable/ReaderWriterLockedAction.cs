using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace OmnIoC.Portable
{
    class ReaderWriterLockedAction
    {
        private readonly ReaderWriterLockSlim _lock = new ReaderWriterLockSlim(LockRecursionPolicy.SupportsRecursion);

        public void ExecuteInReadLock(Action action)
        {
            _lock.EnterReadLock();
            try
            {
                action.Invoke();
            }
            finally
            {
                _lock.ExitReadLock();
            }
        }

        public void ExecuteInWriteLock(Action action)
        {
            _lock.EnterWriteLock();
            try
            {
                action.Invoke();
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }

        public T ExecuteInReadLock<T>(Func<T> action)
        {
            _lock.EnterReadLock();
            try
            {
                return action.Invoke();
            }
            finally
            {
                _lock.ExitReadLock();
            }
        }

        public T ExecuteInWriteLock<T>(Func<T> action)
        {
            _lock.EnterWriteLock();
            try
            {
                return action.Invoke();
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }
    }
}
