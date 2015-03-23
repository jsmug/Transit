using System;
using Transit.Core;

namespace SimpleConsole
{

    class Program
    {

        static void Main(string[] args)
        {


            PublisherComponent publisher = new PublisherComponent();
            SubscriberComponent subscriber = new SubscriberComponent();
            PropertyToMethodRoute route = PropertyToMethodRoute.Create(publisher, x => publisher.Count, subscriber, (x) => subscriber.Increment(x));
            Package package = new Package();

            package.RegisterPropertyRoute(route);

            foreach (Component component in package.Components)
            {
                Console.WriteLine(component.ToString() + " " + component.Id.ToString());
            }

            Console.WriteLine();
            Console.WriteLine();

            foreach (PropertyRoute propRoute in package.RegisteredPropertyRoutes)
            {
                Console.WriteLine("Route info -- " + propRoute.ToString());
            }

            Console.WriteLine();
            Console.WriteLine();

            for (int i = 0; i < 10; i ++)
            {
                publisher.Count = i;
            }

            package.UnregisterAll();

            Console.WriteLine();
            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();

        }

    }

}
