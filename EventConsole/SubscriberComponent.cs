using System;
using Transit.Core;

namespace EventConsole
{
    public class SubscriberComponent : Component
    {

        public SubscriberComponent() : base("Subscriber")
        {
        }


        #region public

        [RouteIn]
        public void ReceiveInterval(EventArgs e)
        {
            Console.WriteLine("Received Interval Event " + DateTimeOffset.Now.ToString("hh:mm:ss"));
        }

        #endregion

    }

}
