using System;
using System.Threading;
using Transit.Core;

namespace EventConsole
{

    class Program
    {

        static void Main(string[] args)
        {

            SynchronizationContext.SetSynchronizationContext(new SynchronizationContext());

            PublisherComponent publisher = new PublisherComponent();
            SubscriberComponent subscriber = new SubscriberComponent();
            EventToMethodRoute route = EventToMethodRoute.Create<EventArgs>(publisher, "IntervalEvent", subscriber, (x) => subscriber.ReceiveInterval(x));
            Package package = new Package("PubSub");

            package.RegisterEventRoute(route);

            Console.WriteLine("Preparing to publish... Please wait or press any key to exit.");
            Console.ReadKey();

            package.UnregisterAll();
            publisher.Dispose();
            
        }

    }

}
