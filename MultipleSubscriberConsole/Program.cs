using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Transit.Core;

namespace MultipleSubscriberConsole
{

    class Program
    {

        static void Main(string[] args)
        {

            SynchronizationContext.SetSynchronizationContext(new SynchronizationContext());

            PublisherComponent publisher = new PublisherComponent();
            MethodSubscriberComponent methodSubscriber = new MethodSubscriberComponent();
            PropertySubscriberComponent propertySubscriber = new PropertySubscriberComponent();

            EventToMethodRoute eventToMethodRoute = EventToMethodRoute.Create<EventArgs>(publisher, "IntervalEvent", methodSubscriber, (x) => methodSubscriber.ReceiveInterval(x));
            EventToPropertyRoute eventToPropertyRoute = EventToPropertyRoute.Create(publisher, "IntervalEvent", propertySubscriber, x => propertySubscriber.ReceiveInterval);

            Package package = new Package("PubSub");

            package.RegisterEventRoute(eventToMethodRoute);
            package.RegisterEventRoute(eventToPropertyRoute);

            Console.WriteLine("Preparing to publish... Please wait or press any key to exit.");
            Console.ReadKey();

            package.UnregisterAll();
            publisher.Dispose();

        }

    }

}
