using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ThreadSafety
{
    public class SmallBusiness
    {
        private decimal cash;
        private decimal receivables;
        //Objects are cheap to allocate, and so the simplest way to ensure you only have contention where necessary is to use private instance or static variables, depending on whether the state to be protected is instance or static data,respectively
        private readonly object stateGuard = new object();
        public SmallBusiness(decimal cash, decimal receivables)
        {
            this.cash = cash;
            this.receivables = receivables;
        }

        public void UnsafeReceivePayment(decimal amount)
        {
            cash += amount;
            receivables -= amount;
        }

        public void SafeReceivePayment(decimal amount)
        {
            // dead lock may happen
            lock (stateGuard)
            {
                cash += amount;
                receivables -= amount;
            }
        }

        public void SaferReceivePayment(decimal amount)
        {
            //need to ensure that you always release the monitor, and for that you can use a try . . . finally block => prevent dead lock (locked state forever)
            bool lockTaken = false;
            try
            {
                Monitor.Enter(stateGuard, ref lockTaken);
                cash += amount;
                receivables -= amount;
            }
            finally
            {
                if (lockTaken)
                {
                    Monitor.Exit(stateGuard);
                }
            }
        }

        public void MostSaferReceivePayment(decimal amount)
        {
            bool lockTaken = false;
            try
            {
                // Timeout out of Waits Using Monitor.TryEnter
                Monitor.TryEnter(stateGuard, TimeSpan.FromSeconds(30), ref lockTaken);
                if (lockTaken)
                {
                    cash += amount;
                    receivables -= amount;
                }
                else
                {
                    throw new TimeoutException("Failed to acquire stateGuard");
                }
            }
            finally
            {
                if (lockTaken)
                {
                    Monitor.Exit(stateGuard);
                }
            }
        }

        public void BestReceivePayment(decimal amount)
        {
            using (stateGuard.Lock(TimeSpan.FromSeconds(30)))
            {
                cash += amount;
                receivables -= amount;
            }
        }


        public decimal SafeNetWorth
        {
            get
            {
                Monitor.Enter(stateGuard);
                decimal netWorth = cash + receivables;
                Monitor.Exit(stateGuard);
                return netWorth;
            }
        }

        public decimal UnsafeNetWorth
        {
            get
            {
                decimal netWorth = cash + receivables;
                return netWorth;
            }
        }
    }
}
