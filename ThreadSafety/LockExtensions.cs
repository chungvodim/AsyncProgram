﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ThreadSafety
{
    // Solution to acquiring monitors with timeouts
    public static class LockExtensions
    {
        public static LockHelper Lock(this object obj, TimeSpan timeout)
        {
            bool lockTaken = false;
            try
            {
                Monitor.TryEnter(obj, TimeSpan.FromSeconds(30), ref lockTaken);
                if (lockTaken)
                {
                    return new LockHelper(obj);
                }
                else
                {
                    throw new TimeoutException("Failed to acquire stateGuard");
                }
            }
            catch
            {
                if (lockTaken)
                {
                    Monitor.Exit(obj);
                }
                throw;
            }
        }

        // The LockHelper is a struct to prevent another heap allocation
        public struct LockHelper : IDisposable
        {
            private readonly object obj;
            public LockHelper(object obj)
            {
                this.obj = obj;
            }
            public void Dispose()
            {
                Monitor.Exit(obj);
            }
        }

    }
}
