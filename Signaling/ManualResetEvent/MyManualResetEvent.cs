/*
 * ManualResetEvent is also a synchronization mechanism similar to AutoResetEvent which works on bool variable. If the bool variable is false, it blocks the thread. If true, it unblocks the threads
 * AutoResetEvent unblocks only one thread at a time and ManualResetEvent unblocks all the waiting threads.
 * AutoResetEvent automatically called the Reset() method after unblocking the thread. But in case of ManualResetEvent, we must call the Reset() event manually.
 * WaitOne() : Blocks the current thread
 * Set() : Unblock the current thread
 * Reset() : Set the state of the event to non-signaled.
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Signaling
{
    public class MyManualResetEvent
    {
        // Demonstrates:
        //      ManualResetEventSlim construction
        //      ManualResetEventSlim.Wait()
        //      ManualResetEventSlim.Set()
        //      ManualResetEventSlim.Reset()
        //      ManualResetEventSlim.IsSet
        public void MRES_SetWaitReset()
        {
            ManualResetEventSlim mres1 = new ManualResetEventSlim(false); // initialize as unsignaled
            ManualResetEventSlim mres2 = new ManualResetEventSlim(false); // initialize as unsignaled
            ManualResetEventSlim mres3 = new ManualResetEventSlim(true);  // initialize as signaled

            // Start an asynchronous Task that manipulates mres3 and mres2
            var observer = Task.Factory.StartNew(() =>
            {
                mres1.Wait();
                Console.WriteLine("observer sees signaled mres1!");
                Console.WriteLine("observer resetting mres3...");
                mres3.Reset(); // should switch to unsignaled
                Console.WriteLine("observer signalling mres2");
                mres2.Set();
            });

            Console.WriteLine("main thread: mres3.IsSet = {0} (should be true)", mres3.IsSet);
            Console.WriteLine("main thread signalling mres1");
            mres1.Set(); // This will "kick off" the observer Task
            mres2.Wait(); // This won't return until observer Task has finished resetting mres3
            Console.WriteLine("main thread sees signaled mres2!");
            Console.WriteLine("main thread: mres3.IsSet = {0} (should be false)", mres3.IsSet);

            // It's good form to Dispose() a ManualResetEventSlim when you're done with it
            observer.Wait(); // make sure that this has fully completed
            mres1.Dispose();
            mres2.Dispose();
            mres3.Dispose();
        }

        // Demonstrates:
        //      ManualResetEventSlim construction w/ SpinCount
        //      ManualResetEventSlim.WaitHandle
        public void MRES_SpinCountWaitHandle()
        {
            // Construct a ManualResetEventSlim with a SpinCount of 1000
            // Higher spincount => longer time the MRES will spin-wait before taking lock
            ManualResetEventSlim mres1 = new ManualResetEventSlim(false, 1000);
            ManualResetEventSlim mres2 = new ManualResetEventSlim(false, 1000);

            Task bgTask = Task.Factory.StartNew(() =>
            {
                // Just wait a little
                Thread.Sleep(100);

                // Now signal both MRESes
                Console.WriteLine("Task signalling both MRESes");
                mres1.Set();
                mres2.Set();
            });

            // A common use of MRES.WaitHandle is to use MRES as a participant in 
            // WaitHandle.WaitAll/WaitAny.  Note that accessing MRES.WaitHandle will
            // result in the unconditional inflation of the underlying ManualResetEvent.
            WaitHandle.WaitAll(new WaitHandle[] { mres1.WaitHandle, mres2.WaitHandle });
            Console.WriteLine("WaitHandle.WaitAll(mres1.WaitHandle, mres2.WaitHandle) completed.");

            // Clean up
            bgTask.Wait();
            mres1.Dispose();
            mres2.Dispose();
        }
    }
}
