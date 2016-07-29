/*
 * An Event declaration adds a layer of abstraction and protection on the delegate instance. This protection prevents clients of the delegate from resetting the delegate and its invocation list and only allows adding or removing targets from the invocation list
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventHandle
{
    class Program
    {
        static void Main(string[] args)
        {
            MyClass mc = new MyClass();
            mc.myEvent += Mc_myEvent;
            mc.myDelegate += Mc_myDelegate;
            mc.myNonArgsEvent += Mc_myNonArgsEvent;
            // handle event is synchronous
            for (int i = 0; i < 10; i++)
            {
                mc.OnDelegate(new object());
                mc.OnEvent(EventArgs.Empty);
                mc.OnNonArgsEvent();
            }
            Console.WriteLine("ending");
            Console.Read();
        }

        private static void Mc_myNonArgsEvent(object sender)
        {
            Console.WriteLine("handling non-Args event");
        }

        private static void Mc_myDelegate(object obj)
        {
            Console.WriteLine("handling delegate");
        }

        private static void Mc_myEvent(object sender, EventArgs e)
        {
            Console.WriteLine("handling event");
        }
    }

    public class MyClass
    {
        public delegate void MyDelegate(object obj);
        public MyDelegate myDelegate;

        public delegate void MyEventHandler(object sender, EventArgs e);
        public event MyEventHandler myEvent;

        public delegate void MyNonArgsEventHandler(object sender);
        public event MyNonArgsEventHandler myNonArgsEvent;

        public virtual void OnDelegate(object obj)
        {
            myDelegate?.Invoke(obj);
        }

        public virtual void OnEvent(EventArgs e)
        {
            myEvent?.Invoke(this, e);
        }

        public virtual void OnNonArgsEvent()
        {
            myNonArgsEvent?.Invoke(this);
        }
    }
}
