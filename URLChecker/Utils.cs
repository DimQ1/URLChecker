﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace URLChecker
{
    public static class Utils
    {
        public static async Task<T> WithCancellation<T>(this Task<T> task, CancellationToken cancellationToken, Action action, bool useSynchronizationContext = true)
        {
            using (cancellationToken.Register(action, useSynchronizationContext))
            {
                try
                {
                    return await task;
                }
                catch (Exception ex)
                {

                    if (cancellationToken.IsCancellationRequested)
                    {
                        // the Exception will be available as Exception.InnerException
                        throw new OperationCanceledException(ex.Message, ex, cancellationToken);
                    }

                    // cancellation hasn't been requested, rethrow the original Exception
                    throw;
                }
            }
        }
    }
}
