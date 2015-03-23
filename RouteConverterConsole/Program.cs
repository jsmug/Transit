using System;
using Transit.Core;

namespace RouteConverterConsole
{
    class Program
    {

        static void Main(string[] args)
        {

            PublisherComponent publisher = new PublisherComponent();
            SubscriberComponent subscriber = new SubscriberComponent();
            PropertyToPropertyRoute route = PropertyToPropertyRoute.Create(publisher, x => publisher.DateTime, subscriber, x => subscriber.DateTime, new StringToDateTimeRouteConverter());
            Package package = new Package();

            package.RegisterPropertyRoute(route);

            for (int i = 0; i < 10; i++)
            {
                
                publisher.DateTime = DateTimeOffset.Now.Add(new TimeSpan(i, 0, 0, 0, 0)).ToString();
                Console.WriteLine(subscriber.DateTime);

            }

            package.UnregisterAll();

            Console.WriteLine();
            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();

        }

    }

}
